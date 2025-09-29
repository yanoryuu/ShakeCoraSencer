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
        inputManager.Acceleration.Subscribe(acc => model.SetAcceleration(acc))
            .AddTo(disposables);

        inputManager.Gyro.Subscribe(gyro => model.SetGyro(gyro))
            .AddTo(disposables);

        inputManager.Ahrs.Subscribe(ahrs => model.SetAhrs(ahrs))
            .AddTo(disposables);

        //判定
        model.acceleration
            .Where(_=>model.isReceivingShake == false)
            .Subscribe(acc => model.DetectShake(acc))
            .AddTo(disposables);
        
        //シェイクになった時
        model.isShaking
            .Where(shake => shake) 
            .Subscribe(_ =>
            {
                view.OnShake();
                model.OnShake();
                // Observable.Timer(TimeSpan.FromMilliseconds(GameConst.cooldown))
                //     .Subscribe(_ =>
                //     {
                //         
                //     })
                //     .AddTo(disposables);
            })
            .AddTo(disposables);

        //ゲーム終了時
        model.time.Where(time => time <= 0)
            .Subscribe(_ =>
            {
                ShakeEnd();
            }).AddTo(disposables);
        
        //タイマー反映
        model.time.Subscribe(time =>
            {
                view.GameStart(time);
                Debug.Log(time);
            })
            .AddTo(disposables);
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
