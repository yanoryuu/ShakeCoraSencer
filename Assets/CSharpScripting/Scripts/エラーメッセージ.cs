using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class エラーメッセージ : MonoBehaviour
{
    void Start()
    {
        // エラーメッセージは、Consoleウィンドウに赤いマークとともに表示されます。
        // そのままでは実行できなくなりますが、
        // 焦らずに、書かれているメッセージを読み解きましょう。
        // プログラムは書いた通りに動きます。
        // 人間の方がどう行動するのかわかりません。

        // Lagメソッドなど見つかりません
        // 'Debug' does not contain a definition for 'Lag'
        // Debug.Lag("テスト");

        float f = 0.2f;
        // 少数を整数の変数に代入する
        // Cannot implicitly convert type 'float' to 'int'. An explicit conversion exists (are you missing a cast?)
        // 型「float」を「int」に暗黙的に変換することはできません。 明示的な変換が存在します (キャストが欠落していますか?)
        // int i = f;
    }
}
