using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class IngamePresenter : IPresenter
{   
    private IngameModel model;
    private IngameView view;
    private ResultModel resultModel;
    private IMUInputManager inputManager;
    private StateManager stateManager;
    private ObstacleSpawner obstacleSpawner;

    private CompositeDisposable gameDisposables;
    private CompositeDisposable uiDisposables;
    private CompositeDisposable launchDisposables;
    
    public IngamePresenter(
        IngameModel model, 
        IngameView view, 
        IMUInputManager inputManager,
        ResultModel resultModel,
        StateManager stateManager,
        ObstacleSpawner obstacleSpawner)
    {
        this.model = model;
        this.view = view;
        this.inputManager = inputManager;
        this.resultModel = resultModel;
        this.stateManager = stateManager;
        this.obstacleSpawner = obstacleSpawner;
        gameDisposables = new CompositeDisposable();
        launchDisposables = new CompositeDisposable();
        uiDisposables = new CompositeDisposable();
        
        stateManager.RegisterOnEnter(GameState.ingame,Enter);
    }

    public void Enter()
    {
        
        view.Initialize();
        model.Initialize();
        ShakeStartCoraGame();
    }

    private void Bind()
    {
        inputManager.AccelerationSub.Subscribe(acc => model.SetAcceleration(acc))
            .AddTo(launchDisposables);
        //
        // inputManager.GyroSub.Subscribe(gyro => model.SetGyro(gyro))
        //     .AddTo(launchDisposables);

        inputManager.AhrsSub.Subscribe(ahrs => model.SetAhrs(ahrs))
            .AddTo(launchDisposables);

        //判定
        model.acceleration
            .Where(_=>model.isReceivingShake)
            .Subscribe(acc => model.DetectShake(acc))
            .AddTo(gameDisposables);
        
        //シェイクになった時
        model.isShaking
            .Where(shake => shake) 
            .Subscribe(_ => 
            {
                model.OnShake();
            })
            .AddTo(gameDisposables);
        
        model.time.Subscribe(time =>view.SetTimer(time))
            .AddTo(gameDisposables);

        //ゲーム終了時
        model.time.Where(time => time <= 0)
            .Subscribe(_ =>
            {
                ShakeEnd();
            }).AddTo(gameDisposables);

        model.shakeCount.Subscribe(count =>
            {
                Debug.Log(count);
                switch (count)
                {
                    case 0:
                        view.OnBarUP(0);
                        break;
                    case (int)(GameConst.maxShakeCount*0.2f):
                        view.OnBarUP(1);
                        break;
                    case (int)(GameConst.maxShakeCount*0.5f):
                        view.OnBarUP(2);
                        break;
                    case (int)(GameConst.maxShakeCount*0.7f):
                        view.OnBarUP(3);
                        break;
                    case GameConst.maxShakeCount:
                        view.OnBarUP(4);
                        break;
                }
                view.OnShakeCora(count);
                SoundManager.Instance.PlaySE("Shake");
            })
            .AddTo(gameDisposables);

        view.onlaunch.Subscribe(_ =>
            {
                Debug.Log("発射");
                LaunchCora();
            })
            .AddTo(uiDisposables);

        //発射アニメーション終了時
        view.onlaunchend.Subscribe(_ =>
            {
                LaunchEnd();
            })
            .AddTo(uiDisposables);
        
        // //テスト用
        view.onshakedone.Subscribe(_=>model.OnShake())
            .AddTo(gameDisposables);
    }
    
    //ゲーム開始
    private async UniTaskVoid ShakeStartCoraGame()
    {
        int countNum = GameConst.initCountDown;

        for (int i = 3; i > 0; i--)
        {
            view.SetCountDownText(i);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
        
        SoundManager.Instance.PlayBGM("Preparation");
        
        view.SetCountDownText(0);
        view.GameStart();
        Bind();
        model.OnShakeStart();
    }
    
    //Shake時間終了
    private void ShakeEnd()
    {
        model.OnShakeEnd();
        view.OnShakeEnd();
        gameDisposables.Dispose();
        gameDisposables = new CompositeDisposable();
        PreparationLaunchCora();
    }
    
    //発射までの演出
    private void PreparationLaunchCora()
    {
        view.PreparationLaunchCora();
        SoundManager.Instance.FadeOutBGM(1f);
    }
    
    //発射
    private void LaunchCora()
    {
        var launchPower = model.CalculateColaLaunchPower(model.shakeCount.Value);
        var launchTime = model.CalculateCoraLaunchTime(model.shakeCount.Value);
        
        //発射BGMに変更、発射図のSEを流す
        SoundManager.Instance.PlaySE("LaunchCora");
        SoundManager.Instance.PlayBGM("Launch");

        resultModel.SetScore((int)launchPower);

        view.LaunchCora(launchPower, launchTime);
        obstacleSpawner.BeginSpawn();

        // === ① コーラ左右移動 ===
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                view.MoveCora(model.CalculateCoraSwipingPower(model.ahrs.Value.x));
            })
            .AddTo(launchDisposables);

        // === ② 障害物との矩形当たり判定 ===
        var coraRect = view.CoraRectTransform; // ← Viewにプロパティを追加する（下記で説明）

        Observable.EveryUpdate()
            .Where(_ => obstacleSpawner != null)
            .Subscribe(_ =>
            {
                foreach (var obs in obstacleSpawner.ActiveObstacles)
                {
                    if (!obs.activeInHierarchy) continue;
                    var obsRect = obs.GetComponent<RectTransform>();
                    if (coraRect.IsOverlapping(obsRect))
                    {
                        view.onHitObstacle.OnNext(obsRect.transform.gameObject);
                        break;
                    }
                }
            })
            .AddTo(launchDisposables);

        view.onHitObstacle
            .Subscribe(async hitObj =>
            {
                if (hitObj.CompareTag("Obstacle"))
                {
                    SoundManager.Instance.PlaySE("Boom");
                    await view.hitObstacle();
                    LaunchEnd();
                }else if (hitObj.CompareTag("NomalCora"))
                {
                    SoundManager.Instance.PlaySE("GetItem");
                    model.GetItem();
                }
            })
            .AddTo(launchDisposables);
    }

    
    //発射完了
    private void LaunchEnd()
    {
        view.LaunchEnd();
        uiDisposables.Dispose();
        uiDisposables = new CompositeDisposable();

        launchDisposables.Dispose();
        launchDisposables = new CompositeDisposable();
        
        obstacleSpawner.StopSpawn();
        
        Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                stateManager.ChangeState(GameState.result);
            });
    }
}