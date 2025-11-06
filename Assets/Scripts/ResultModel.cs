using UnityEngine;

public class ResultModel
{
    public int score { get; private set; }

    public void SetScore(int score)
    {
        this.score = score;
    }
}
