using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 複数の比較式を組み合わせる : MonoBehaviour
{
    public int Age = 20;

    void Start()
    {
        // 6 以上 かつ 15 以下
        if (Age >= 6 && Age <= 15)
        {
            Debug.Log("義務教育の対象");
        }

        // 5 以下 または 65 以上 
        if (Age <= 5 || Age >= 65)
        {
            Debug.Log("幼児と高齢者");
        }
    }
}
