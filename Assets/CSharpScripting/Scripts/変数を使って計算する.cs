using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 変数を使って計算する : MonoBehaviour
{
    // 値 …
    //   数値や文字列などのデータ類をまとめて「値」という
    // 変数とは …
    // 　値を記憶できる箱のようなもの。
    // 　格納する値のタイプによって、さまざまな「型」が存在する。

    void Start()
    {
        // ------- 文字列を変数に格納して、コンソールに表示する -------
        // string text → 変数の「宣言」をする
        // text = "こんにちは。" → 変数に値を「代入」する
        // string → string型 文字列を格納する変数
        // text → textという名前の変数
        string text = "こんにちは。";

        // textの中の値を表示して
        Debug.Log(text);

        // ------- 変数を定義し、変数を使った計算を行い、結果をコンソールに表示する -------
        // float → float型 浮動小数点数
        float kakaku = 100f;
        // kakaku に 少数1.1を掛け合わせた結果を、zeikomiKakaku に代入する
        float zeikomiKakaku = kakaku * 1.1f;
        // コンソールに表示
        Debug.Log(zeikomiKakaku);

    }
}
