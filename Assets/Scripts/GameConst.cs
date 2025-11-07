using UnityEngine;

public static class GameConst
{
    // 判定パラメータ
    public const float shakeThrethld = 0.8f; // g単位
    public const float limitTime = 10;
    public const int initCountDown = 3;
    
    public const float shakeInterval = 200f;
    
    public const int maxShakeCount = 100;
    
    //発射用のパワー計算要定数
    public const float powerPerShot = 100f;
}
