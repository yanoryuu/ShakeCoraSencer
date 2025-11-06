using System;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField] private GameObject resultTitle;

    [SerializeField] private TextMeshProUGUI resultScore;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button retryButton;

    public Subject<Unit> onHome = new();
    public Subject<Unit> onRetry = new();

    private void Awake()
    {
        if (homeButton != null)
            homeButton.onClick.AddListener(() => onHome.OnNext(Unit.Default));

        if (retryButton != null)
            retryButton.onClick.AddListener(() => onRetry.OnNext(Unit.Default));
    }

    public void Initialize()
    {
        resultTitle.transform.localPosition =  Vector3.zero;
    }

    public void SetScore(int score)
    {
        resultTitle.transform.DOLocalMoveY(200, 1)
            .SetEase(Ease.Linear);
        
        DOVirtual.Int(0, score, 3, value =>
            {
                resultScore.text = value.ToString();
            })
            .SetEase(Ease.InCubic);

    }
}
