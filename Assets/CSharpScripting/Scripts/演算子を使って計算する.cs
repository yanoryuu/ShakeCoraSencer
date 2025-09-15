using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 演算子を使って計算する : MonoBehaviour
{
    void Start()
    {
        // ------- 演算子を使って計算をした結果をコンソールに表示する -------
        // 加算（足し算）
        Debug.Log(10 + 5);
        // 減算（引き算）
        Debug.Log(10 - 5);
        // 乗算（掛け算）
        Debug.Log(10 * 5);
        // 除算（割り算）
        Debug.Log(10 / 5);
        // 剰余（割った余りの数）
        Debug.Log(11 % 5);

        // ------- 文字列 と 演算結果 を 文字列として足し合わせる -------
        // 加算（足し算）
        Debug.Log("10 + 5 = " + (10 + 5));
        // 減算（引き算）
        Debug.Log("10 - 5 = " + (10 - 5));
        // 乗算（掛け算）
        Debug.Log("10 * 5 = " + (10 * 5));
        // 除算（割り算）
        Debug.Log("10 / 5 = " + (10 / 5));
        // 剰余（割った余りの数）
        Debug.Log("11 % 5 = " + (11 % 5));
    }
}
