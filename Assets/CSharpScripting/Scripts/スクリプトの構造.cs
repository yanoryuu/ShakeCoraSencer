// System名前空間 の中の Collections名前空間 を使用する 
using System.Collections;
// System名前空間 の中の Generic名前空間 を使用する
using System.Collections.Generic;
// UnityEngine名前空間 を使用する。UnityEngine内に格納されたクラスたちを、このスクリプト内で使用することができる。
using UnityEngine;

// ; (セミコロン) → 文の終わりを表す記号 関数には不要。

// public → パブリック設定 → 外部のクラスやライブラリからこのクラスが利用できるようになる。
// class クラスを作成
// スクリプトの構造 という名前のクラス
// : MonoBehaviour → MonoBehaviourというクラスを親クラスとして、継承します
public class スクリプトの構造 : MonoBehaviour
{
    // Start() → 最初のフレームに呼び出される関数
    // void    → この関数は、値として何も返しません
    // Start   → という名前の関数
    // ()      → 引数はない
    void Start()
    {// ブロック開始
        // ここに処理を書く
    }// ブロック終了

    // Update() → 毎フレーム実行されるたびに呼び出される関数 ※フレームレートによって呼び出される回数が変化します
    // void     → この関数は、値として何も返しません
    // Update   → という名前の関数
    // ()       → 引数はない
    void Update()
    {// ブロック開始
        // ここに処理を書く
    }// ブロック終了


    // スクリプトのライフサイクル
    // https://docs.unity3d.com/ja/2021.3/Manual/ExecutionOrder.html
}
