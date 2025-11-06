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

    private CompositeDisposable gameDisposables;
    private CompositeDisposable uiDisposables;
    
    public IngamePresenter(
        IngameModel model, 
        IngameView view, 
        IMUInputManager inputManager,
        ResultModel resultModel,
        StateManager stateManager)
    {
        this.model = model;
        this.view = view;
        this.inputManager = inputManager;
        this.resultModel = resultModel;
        this.stateManager = stateManager;
        gameDisposables = new CompositeDisposable();
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
            .AddTo(gameDisposables);

        inputManager.GyroSub.Subscribe(gyro => model.SetGyro(gyro))
            .AddTo(gameDisposables);

        inputManager.AhrsSub.Subscribe(ahrs => model.SetAhrs(ahrs))
            .AddTo(gameDisposables);

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
                    case 5:
                        view.OnBarUP(1);
                        break;
                    case 10:
                        view.OnBarUP(2);
                        break;
                    case 30:
                        view.OnBarUP(3);
                        break;
                    case 45:
                        view.OnBarUP(4);
                        break;
                }
                view.OnShakeCora(count);
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
    }
    
    //発射
    private void LaunchCora()
    {
        var launchPower = model.CalculateColaLaunchPower(model.shakeCount.Value);
        var launchTime = model.CalculateCoraLaunchTime(model.shakeCount.Value);

        resultModel.SetScore((int)launchPower);
        
        view.LaunchCora(launchPower,launchTime);
    }
    
    //発射完了
    private void LaunchEnd()
    {
        view.LaunchEnd();
        uiDisposables.Dispose();
        uiDisposables = new CompositeDisposable();

        Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                stateManager.ChangeState(GameState.result);
            });
    }
}