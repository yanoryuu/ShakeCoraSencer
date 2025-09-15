using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class while文を使って繰り返し処理 : MonoBehaviour
{
    void Start()
    {
        int 資金 = 30000;
        
        // 資金 が 0 以上 である限り {} 内の処理を実行する
        while (資金 >= 0)
        {
            // コンソールに表示
            Debug.Log(資金);
            // 整数 5080 を 引く
            資金 = 資金 - 5080;
        }

        Debug.Log("資金が尽きました…");
    }
}
