using UnityEngine;

public class ResultModel
{
    public int score { get; private set; }

    public void SetScore(int score,int normalCoraQuantity,int goldCoraQuantity)
    {
        this.score = score+normalCoraQuantity*100+goldCoraQuantity*1000;
    }
}
