using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 条件分岐_2 : MonoBehaviour
{
    /// <summary>
    /// 年齢
    /// </summary>
    public int Age = 20;

    // 毎フレーム処理する
    void Update()
    {
        // もし、Age が 18 より小さければ、
        if (Age < 18)
        {
            Debug.Log("未成年です。");
        }
        // そうでなければ、
        else
        {
            Debug.Log("成人");
        }
    }
}
