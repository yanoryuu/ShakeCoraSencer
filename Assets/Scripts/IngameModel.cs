using R3;
using UnityEngine;
using System;
using DG.Tweening;

public class IngameModel
{
    public ReactiveProperty<Vector3> acceleration { get; private set; }
    public ReactiveProperty<Vector3> gyro { get; private set; }
    public ReactiveProperty<Vector3> ahrs { get; private set; }

    public ReactiveProperty<bool> isShaking { get; private set; }

    private float prevAccelMag = 1f;
    private long lastShakeMs = 0;

    public ReactiveProperty<int> shakeCount { get; private set; }
    
    public bool isReceivingShake { get;private set; } = false;

    public ReactiveProperty<float> time;
    
    public void Initialize()
    {
        isShaking.Value = false;
        
        shakeCount.Value = 0;
        
        time.Value = GameConst.limitTime;
    }

    public IngameModel()
    {
        acceleration = new ReactiveProperty<Vector3>();
        gyro = new ReactiveProperty<Vector3>();
        ahrs = new ReactiveProperty<Vector3>();
        isShaking = new ReactiveProperty<bool>(false);
        shakeCount = new ReactiveProperty<int>(0);
        time = new ReactiveProperty<float>();
    }

    public void SetAcceleration(Vector3 acceleration)
    {
        this.acceleration.Value = acceleration;
    }

    public void SetGyro(Vector3 gyro)
    {
        this.gyro.Value = gyro;
    }

    public void SetAhrs(Vector3 ahrs)
    {
        this.ahrs.Value = ahrs;
    }

    /// <summary>
    /// 振った動作を判定して isShaking を更新
    /// </summary>
    public void DetectShake(Vector3 acc)
    {
        float mag = acc.magnitude; // g
        float delta = Mathf.Abs(mag - prevAccelMag);
        prevAccelMag = mag;
        if (delta >= GameConst.shakeThrethld)
        {
            isShaking.Value = true;
        }
        
    }

    public float CalculateColaLaunchPower(int shakeCount)
    {
        var power = Mathf.Clamp(shakeCount*shakeCount*8,1,17200);
        
        Debug.Log("power = " + power);
        return power;
    }

    public float CalculateCoraLaunchTime(int shakeCount)
    {
        var time = Mathf.Clamp(Mathf.Sqrt(shakeCount*7), 1, 20);

        Debug.Log("time="+time);
        return time;
    }

    public float CalculateCoraSwipingPower(float pitch)
    {
        if (8 < pitch + 8 && pitch + 8 < 8) return 0f;

        float swipingPower = pitch * 10f;

        return swipingPower;
    }
    
    public void OnShake()
    {
        isShaking.Value = false;
        shakeCount.Value++;
        Debug.Log("OnShake");
    }

    public void OnShakeStart()
    {
        Debug.Log("OnShakeStart");
        
        isReceivingShake = true;
        time.Value = GameConst.limitTime;

        DOVirtual.Float(GameConst.limitTime, 0.0f, GameConst.limitTime, value => time.Value = value);
    }

    public void OnShakeEnd()
    {
        isReceivingShake = false;
    }
}