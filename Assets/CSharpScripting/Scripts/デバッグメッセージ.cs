using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class デバッグメッセージ : MonoBehaviour
{
    void Start()
    {
        // Console（コンソール）ウィンドウに こんにちは。 というデバッグメッセージを表示してください。
        // Debug → UnityEngie.Debugクラス → デバッグ機能のクラス
        // Log → コンソールウィンドウに、ログメッセージを表示するメソッド（関数）
        // "こんにちは。" → C#では、文字列は ""（ダブルクオーテーション）で閉じる
        Debug.Log("こんにちは。");
    }
}
