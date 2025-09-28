using System;
using R3;
using DG.Tweening;

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
    }
    
    //ゲーム開始
    public void ShakeStartCoraGame()
    { 
        int startNumber = 3;
        int current = startNumber;

        DOTween.To(() => current, x => current = x, 0, startNumber)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                view.SetCountDownText(current);
            })
            .OnComplete(() =>
            {
                model.OnShakeStart();
                view.SetTimerBar(10f);
                Bind();
            });
    }
    
    public void ShakeEnd()
    {
        model.OnShakeEnd();
        disposables.Dispose();
        disposables = new CompositeDisposable();
    }
}
