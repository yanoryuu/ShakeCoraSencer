using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameView : MonoBehaviour
{
    [Header("CountDown")]
    [SerializeField] private GameObject countDownObj;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI countDownBackGroundText;

    [Header("Visuals")]
    [SerializeField] private GameObject coraImage;
    [SerializeField] private GameObject handImage;

    [Header("CO2 UI")]
    [SerializeField] private RawImage coraCO2Bar;   // スクロールさせるRawImage
    // [SerializeField] private RectMask2D coraCO2Mask;     // type=Filled にしてfillAmountでマスク
    [SerializeField] private Image coraCO2Mask;
    [Header("Timer UI")]
    [SerializeField] private Image coraTimerBar;    // type=Filled
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Shake!")] 
    [SerializeField] private GameObject shakeImg;

    // UVスクロール設定
    [SerializeField] private float uvLoopSeconds = 2f;  // UVが1タイル分進む時間
    [SerializeField] private bool uvIgnoreTimeScale = true;

    private Tween _timerTween;
    private Tween _uvTween;

    public void Initialize()
    {
        // Filled必須
        coraTimerBar.type = Image.Type.Filled;
        coraCO2Mask.type = Image.Type.Filled;

        // 初期表示
        coraTimerBar.fillAmount = 0f;
        coraCO2Mask.fillAmount = 0f;
        

        // 残り時間表示の初期化（必要なら外部で渡す値に合わせて書き換え）
        timerText.text = GameConst.limitTime.ToString("0.00");

        // RawImage設定（Repeat必須）
        if (coraCO2Bar != null && coraCO2Bar.texture != null)
        {
            coraCO2Bar.texture.wrapMode = TextureWrapMode.Repeat;
            var uv = coraCO2Bar.uvRect;
            uv.x = Mathf.Repeat(uv.x, 1f);
            uv.y = Mathf.Repeat(uv.y, 1f);
            uv.width  = Mathf.Max(uv.width, 0.0001f);
            uv.height = Mathf.Max(uv.height, 0.0001f);
            coraCO2Bar.uvRect = uv;
        }
        
        shakeImg.SetActive(false);
    }

    // 降った時の演出（必要に応じて）
    public void OnShake()
    {
        Debug.Log("OnShake");
    }

    //カウントダウン表示
    public void SetCountDownText(int countDown)
    {
        //初期化
        countDownObj.SetActive(true);
        countDownObj.transform.localScale = Vector3.one;
        countDownObj.GetComponent<CanvasGroup>().alpha = 1.0f;
        
        //表示
        string s = countDown.ToString("0.0");
        countDownText.text = s;
        countDownBackGroundText.text = s;
        
        //アニメーション
        var sequence = DOTween.Sequence();
        sequence.Join(countDownObj.transform.DOScale(3f,0.2f))
            .SetEase(Ease.OutCubic)
            .Join(countDownObj.GetComponent<CanvasGroup>().DOFade(0.0f,0.4f))
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                countDownObj.SetActive(false);
                sequence.Kill();
            });
        
    }

    //ゲーム開始
    public void GameStart(float time)
    {
        //チャージバー表示
        SetTimerBar(time);
        
        shakeImg.SetActive(true);
    }
    
    //チャージバー表示
    public void SetTimerBar(float t)
    {
        timerText.text = t.ToString("0.00");

        float p = 1f - (t / GameConst.limitTime); // 進行度 0→1
        coraTimerBar.fillAmount = p;
        coraCO2Mask.fillAmount = p;

        // UV無限スクロール開始（下方向に1タイル/周）
        StartUVScrollY(uvLoopSeconds);
    }

    private void StartUVScrollY(float loopSeconds)
    {
        if (coraCO2Bar == null) return;
        if (_uvTween != null) return;

        float uvY = coraCO2Bar.uvRect.y; // 現在値から
        _uvTween = DOTween.To(() => uvY, v =>
                {
                    uvY = v;
                    var r = coraCO2Bar.uvRect;
                    r.y = Mathf.Repeat(uvY, 1f); // 0..1 に正規化
                    coraCO2Bar.uvRect = r;
                },
                uvY - 1f, // 1タイル分進める
                Mathf.Max(0.01f, loopSeconds))
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental) // ループのたびに+1ずつ
            .SetUpdate(UpdateType.Late);
    }

    public void OnShakeEnd()
    {
        _timerTween?.Kill();
    }

    public void StopUVScroll()
    {
        _uvTween?.Kill();
    }

    private void OnDisable()
    {
        _timerTween?.Kill();
        _uvTween?.Kill();
    }

    private void OnDestroy()
    {
        _timerTween?.Kill();
        _uvTween?.Kill();
    }
}