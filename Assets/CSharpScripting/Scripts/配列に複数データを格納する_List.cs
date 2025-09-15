using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 配列に複数データを格納する_List : MonoBehaviour
{
    // List → 動的配列。後から配列の数を増やすことができる
    // C#の場合、[]Arrayの方の配列でも、要素数を変更できるので大きな違いはない
    // Listの方が便利なため、こちらを使うことが多い

    // 色のリスト
    public List<string> ColorList = new List<string>();
    
    void Start()
    {
        for (int i = 0; i < ColorList.Count; i++)
        {
            Debug.Log(ColorList[i]);
        }
    }
}
