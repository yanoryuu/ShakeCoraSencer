using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 組み込み型と型変換 : MonoBehaviour
{
    /// <summary>
    /// 整数 32bit
    /// </summary>
    int 整数 = 128;
    /// <summary>
    /// 実数（少数も含む）64bit
    /// </summary>
    double 倍精度実数 = 8.0;
    /// <summary>
    /// 実数（少数も含む）32bit
    /// 数値の最後に f をつける！！！
    /// </summary>
    float 単精度実装 = 10.2f;
    /// <summary>
    /// 文字列 
    /// ""で囲む.
    /// </summary>
    string 文字列 = "こんにちは";
    /// <summary>
    /// true（真） または false（偽） 
    /// </summary>
    bool   真偽値 = true;

    void Start()
    {
        // int型に型変換する
        int 整数 = (int)3.5f;
        // テキストにする
        string text  = "" + 100;
        // 整数を文字列に変換する
        string text2 = 整数.ToString();        
    }

    // 数値を格納する型の精度の話
    // http://www.design.kyushu-u.ac.jp/~samejima/prl/08.html
}
