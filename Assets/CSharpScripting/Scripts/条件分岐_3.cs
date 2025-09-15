using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 条件分岐_3 : MonoBehaviour
{
    /// <summary>
    /// 年齢
    /// </summary>
    public int Age = 20;

    void Start()
    {
        if (Age < 18)
        {
            Debug.Log("未成年");
        }
        else if (Age < 65)
        {
            Debug.Log("成人");
        }
        else
        {
            Debug.Log("高齢者");
        }
    }
}
