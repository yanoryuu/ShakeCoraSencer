using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 比較演算子 : MonoBehaviour
{
    public int A = 0;
    public int B = 0;
    
    /*
     * 比較演算子
     * 
     * <  : 左辺は右辺より小さい    a <  b
     * <= : 左辺は右辺以下          a <= b 
     * >  : 左辺は右辺より大きい    a >  b
     * >= : 左辺は右辺以上          a >= b
     * == : 左辺と右辺は等しい      a == b
     * != : 左辺と右辺は等しくない  a != b
     * 
     */

    // Gameビュー上に、テキストやテクスチャ、GUI（スライダー、ボタン）などを表示する。
    void OnGUI()
    {
        // Labelのフォントサイズを一時保管
        var storeFontSize = GUI.skin.label.fontSize;
        
        // ラベルテキスト 開始位置X
        var sX = 32;
        // ラベルテキスト 行の間隔
        var dY = 48;

        // Labelのフォントサイズを代入
        GUI.skin.label.fontSize = 48;
        GUI.Label(new Rect(sX, dY * 0, 512, 64), "A <  B は " + (A <  B).ToString());        
        GUI.Label(new Rect(sX, dY * 1, 512, 64), "A <= B は " + (A <= B).ToString());        
        GUI.Label(new Rect(sX, dY * 2, 512, 64), "A >  B は " + (A >  B).ToString());        
        GUI.Label(new Rect(sX, dY * 3, 512, 64), "A >= B は " + (A >= B).ToString());        
        GUI.Label(new Rect(sX, dY * 4, 512, 64), "A == B は " + (A == B).ToString());        
        GUI.Label(new Rect(sX, dY * 5, 512, 64), "A != B は " + (A != B).ToString());
        
        // Labelのフォントサイズ に 一時保管していた数値を代入
        GUI.skin.label.fontSize = storeFontSize;
    }
}
