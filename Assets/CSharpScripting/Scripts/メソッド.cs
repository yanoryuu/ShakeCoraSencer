using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class メソッド : MonoBehaviour
{
    void Start()
    {
        // Mathf → Mathfクラス（算数、数学系のメソッドがいろいろ）
        // Max → 引数の中の最大値を求める
        // value = Mathf.Max() → Mathf.Max()の「戻り値」…計算結果として関数が返す数 を代入
        int value = Mathf.Max(10, 40, 20, 30);
        // 値を表示
        Debug.Log(value);

        // var → Variable（変数） 演算結果の型に応じて、整数なのか、数値なのか、文字列なのか、宣言時に変化する
        // Sqrtメソッド → 平方根を求める。√16 = 4
        var result = Mathf.Sqrt(16) + 1.0f;
        // 値を表示
        Debug.Log(result);
    }
}
