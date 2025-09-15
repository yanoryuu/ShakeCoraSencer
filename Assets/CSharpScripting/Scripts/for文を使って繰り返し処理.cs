using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class for文を使って繰り返し処理 : MonoBehaviour
{
    void Start()
    {
        // for文
        // for(初期化; 継続条件; 最終式)
        // {
        //   繰り返したい文
        // }

        // 変数 i を 整数 0 で初期化
        // 継続条件 i が 5 より小さい
        // {}ブロック内の処理を行う
        // iを1つ増やす
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i + "回目 こんにちは");
        }

        Debug.Log("----------------------------------------");

        // 逆順
        for (int i = 5; i > 0; i--)
        {
            Debug.Log(i + "回目 こんにちは");
        }
    }
}
