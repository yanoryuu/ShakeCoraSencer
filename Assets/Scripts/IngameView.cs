
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject coraImage;
    [SerializeField] private GameObject handImage;
    
    [SerializeField] private RawImage coraCO2Bar;
    [SerializeField] private Image coraTimerBar;
    private bool isUpCora;
    
    private CompositeDisposable disposables = new CompositeDisposable();
    
    public void Initialize()
    {
        
    }

    //降った時の演出
    public void OnShake()
    {
        Debug.Log("OnShake");
        if (isUpCora)
        {
            
        }
        else
        {
            
        }
    }

    public void SetCountDownText(float countDown)
    {
        countDownText.text = countDown.ToString("0.0");
    }
    
    public void SetTimerBar(float time)
    {

        Debug.Log("Start Timer Bar Animation");
        
        coraTimerBar.fillAmount = 0f;
        coraTimerBar.DOFillAmount(1f, time); // 5秒かけて 0 → 1 へ
        
        float uvY = 0f;
        
        DOTween.To(() => uvY, x =>
            {
                uvY = x;
                Rect uv = coraCO2Bar.uvRect;
                uv.y = uvY;
                coraCO2Bar.uvRect = uv;
            }, -1f, time)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart); // 無限ループ
    }
}
