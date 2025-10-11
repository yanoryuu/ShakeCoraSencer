using DG.Tweening;
using R3;
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
    [SerializeField] private float uvLoopSeconds = 10f;  // UVが1タイル分進む時間
    [SerializeField] private bool uvIgnoreTimeScale = true;

    private Tween _timerTween;
    private Tween _uvTween;

    private Tween hueTween;

    private bool isUp;

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
        
        handImage.transform.localPosition = new Vector3(-16, 0, 0);
        
        shakeImg.SetActive(false);
    }

    //カウントダウン表示
    public void SetCountDownText(float countDown)
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

        coraImage.transform.DOLocalMoveY(0, 2f)
            .SetEase(Ease.OutCubic);

    }

    //ゲーム開始
    public void GameStart()
    {
        shakeImg.SetActive(true);
    }
    
    //チャージバー表示
    public void SetTimer(float t)
    {
        timerText.text = t.ToString("0.00");
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

    //コーラを振った数が変わったとき
    public void OnShakeCora(int shakeCount)
    {
        float p = Mathf.Clamp01(shakeCount / 20f); // 進行度 0→1
        coraTimerBar.fillAmount = p;
        coraCO2Mask.fillAmount = p;
        
        OnShake();
    }
    
    // 降った時の演出（必要に応じて）
    public void OnShake()
    {
        Debug.Log("OnShake");
        if (isUp)
        {
            handImage.transform.localPosition = new Vector3(-16, 100, 0);
            handImage.transform.localEulerAngles = new Vector3(0, 0, -20);
        }
        else
        {
            handImage.transform.localPosition = new Vector3(-16, -100, 0);
            handImage.transform.localEulerAngles = new Vector3(0, 0, 20);       
        }
        
        isUp = !isUp;
    }

    //チャージのバーが一つ上に行ったときの演出
    public void OnBarUP(int barStage)
    {
        StopUVScroll();

        switch (barStage)
        {
            case 0:
                StartUVScrollY(10f);
                coraCO2Mask.DOColor(new Color32(120, 0,   0,   255), 0.5f);
                Debug.Log("Color32(120,0,0,255)");
                break;

            case 1:
                StartUVScrollY(5f);
                coraCO2Mask.DOColor(new Color32(180, 0,   0,   255), 0.5f);
                Debug.Log("Color32(180,0,0,255)");
                break;

            case 2:
                StartUVScrollY(2f);
                coraCO2Mask.DOColor(new Color32(200, 0, 0, 255), 0.5f);
                Debug.Log("Color32(200,255,255,255)");
                break;

            case 3:
                StartUVScrollY(1f);
                coraCO2Mask.DOColor(new Color32(255, 63,  63,  255), 0.5f);
                Debug.Log("Color32(255,63,63,255)");
                break;

            case 4:
                StartRainbow();
                StartUVScrollY(0.1f);
                Debug.Log("StartRainbow");
                break;

            case 5:
                StartUVScrollY(0.1f);
                break;
        }
    }
    
    private void StartRainbow()
    {
        // 既存のHueアニメを止める
        if (hueTween != null)
        {
            hueTween.Kill();
            hueTween = null;
        }

        // 現在色をHSVへ
        Color.RGBToHSV(coraCO2Mask.color, out float h, out float s, out float v);
        float startHue = h;

        // Hueを0→1へ回し続ける（durationはお好みで）
        const float duration = 0.1f; // ← -3 は無効。正の値に！

        hueTween = DOTween.To(
                () => startHue,
                x =>
                {
                    startHue = x % 1f; // 0〜1ループ
                    coraCO2Mask.color = Color.HSVToRGB(startHue, s, v);
                },
                1f,
                duration
            )
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public void OnShakeEnd()
    {
        _timerTween?.Kill();
        _timerTween = null;
    }

    public void StopUVScroll()
    {
        _uvTween?.Kill();
        _uvTween = null;
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