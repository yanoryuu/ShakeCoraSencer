using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class for文を入れ子にして繰り返し処理 : MonoBehaviour
{
    void Start()
    {
        // 九九の計算
        for (int i = 1; i <= 9; i++)
        {
            for (int j = 1; j <= 9; j++)
            {
                Debug.Log(i + " × " + j + " = " + i * j);
            }
        }
    }
}
