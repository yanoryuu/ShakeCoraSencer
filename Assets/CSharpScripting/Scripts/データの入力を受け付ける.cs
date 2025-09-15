using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class データの入力を受け付ける : MonoBehaviour
{// クラスのブロック - 開始
    /*
     * パブリック変数 …
     * コンポーネント間で、データのやり取りをするために用意された特殊な変数。
     * 
     * [Inspector]ウィンドウに、入力ボックスが表示されるようになり、インスペクタ上から
     * 変数に値の代入が可能になる
     *
     * クラスのブロック直下に書く必要がある。
     * 型名の前に public を書く。
     */
    // public 
    public string text;

    void Start()
    {
        Debug.Log("入力したテキストは : " + text);
    }
}// クラスのブロック - 終了
