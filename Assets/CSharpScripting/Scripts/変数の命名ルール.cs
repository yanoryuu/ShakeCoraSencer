using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 変数の命名ルール : MonoBehaviour
{
    /**
     * アルファベットのa～z, アンダースコア, 数字を組み合わせてつける
     */

    /**
     * OK
     * answer name1 name2 my_value text BALL
     */

    public int answer;
    public int name1;
    public int name2;
    public int my_value;
    public int text;
    public int BALL;

    // 漢字などの全角文字も許可されていますが、
    // 実際のところ、半角との切り替えが面倒なので、おすすめしません。
    public int 日本語の変数;

    /**
     * NG
     * !mark 12345 1day a+b x-y
     */

    // 以下に挙げるキーワードは「予約語」と呼ばれ、
    // C# で特定の目的のための文字列であり、変数名に使用することはできない。
    /**
     * abstract  as  base  bool  break  byte  case  catch  char  checked
     * class  const  continue  decimal  default  delegate  do  double
     * else  enum  event  explicit  extern  false  finally  fixed  float
     * for  foreach  goto  if  implicit  in  int  interface  internal  is
     * lock  long  namespace  new  null  object  operator  out  override
     * params  private  protected  public  readonly  ref  return  sbyte
     * sealed  short  sizeof  stackalloc  static  string  struct  switch
     * this  throw  true  try  typeof  uint  ulong  unchecked  unsafe
     * unshort  using  using static  virtual  void  volatile  while
     */



}
