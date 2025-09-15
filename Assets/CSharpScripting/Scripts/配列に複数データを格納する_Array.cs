using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 配列に複数データを格納する_Array : MonoBehaviour
{
    // string型の配列を作成する
    string[] compassDirs = { "東", "西", "南", "北" };

    public string[] Directions;

    void Start()
    {
        for (int i = 0; i < compassDirs.Length; i++)
        {
            Debug.Log(compassDirs[i]);
        }

        Debug.Log("----------------------------");

        foreach (string dir in Directions)
        {
            Debug.Log(dir);
        }

    }
}
