using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class IngamePresenter : IPresenter
{   
    private IngameModel model;
    private IngameView view;
    private IMUInputManager inputManager;

    private CompositeDisposable disposables;
    
    public IngamePresenter(IngameModel model, IngameView view, IMUInputManager inputManager)
    {
        this.model = model;
        this.view = view;
        this.inputManager = inputManager;
        
        disposables = new CompositeDisposable();
    }

    public void Enter()
    {
        ShakeStartCoraGame();
        view.Initialize();
        model.Initialize();
    }

    private void Bind()
    {
        inputManager.AccelerationSub.Subscribe(acc => model.SetAcceleration(acc))
            .AddTo(disposables);

        inputManager.GyroSub.Subscribe(gyro => model.SetGyro(gyro))
            .AddTo(disposables);

        inputManager.AhrsSub.Subscribe(ahrs => model.SetAhrs(ahrs))
            .AddTo(disposables);

        //判定
        model.acceleration
            .Where(_=>model.isReceivingShake == false)
            .Subscribe(acc => model.DetectShake(acc))
            .AddTo(disposables);
        
        // //シェイクになった時
        model.isShaking
            .Where(shake => shake) 
            .Subscribe(_ =>
            {
                model.OnShake();
            })
            .AddTo(disposables);
        
        model.time.Subscribe(time =>view.SetTimer(time))
            .AddTo(disposables);

        //ゲーム終了時
        model.time.Where(time => time <= 0)
            .Subscribe(_ =>
            {
                ShakeEnd();
            }).AddTo(disposables);

        model.shakeCount.Subscribe(count =>
            {
                Debug.Log(count);
                switch (count)
                {
                    case(0):
                        view.OnBarUP(0);
                        break;
                    case(5):
                        view.OnBarUP(1);
                        break;
                    case(10):
                        view.OnBarUP(2);
                        break;
                    case(15):
                        view.OnBarUP(3);
                        break;
                    case(20):
                        view.OnBarUP(4);
                        break;
                }
                view.OnShakeCora(count);
            })
            .AddTo(disposables);
        
        // //テスト用
        // Observable.Interval(TimeSpan.FromSeconds(0.2f))
        //     .Subscribe(_=>model.OnShake())
        //     .AddTo(disposables);
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
    
    //ゲーム終了
    public void ShakeEnd()
    {
        model.OnShakeEnd();
        view.OnShakeEnd();
        disposables.Dispose();
        disposables = new CompositeDisposable();
    }
}
