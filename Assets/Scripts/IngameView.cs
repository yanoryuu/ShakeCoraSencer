using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using R3.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameView : MonoBehaviour
{
    [Header("CountDown")]
    [SerializeField] private GameObject countDownObj;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI countDownBackGroundText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Visuals")]
    [SerializeField] private GameObject coraImage;
    [SerializeField] private GameObject handImage;
    [SerializeField] private GameObject thumbImage;
    [SerializeField] private GameObject fingerImage;
    [SerializeField] private GameObject paramImage;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject coraEneImage;
    [SerializeField] private GameObject jetEffect;

    private Vector3 backGroundInitPos;
 
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
    
    //デバッグ用
    [SerializeField] private Button debugButton;

    private Tween _timerTween;
    private Tween _uvTween;

    private Tween hueTween;
    
    private Sequence launchSequence;
    private Sequence preparationLaunchSequence;

    private bool isUp;

    public Subject<Unit> onshakedone = new Subject<Unit>();
    
    public Subject<Unit> onlaunch = new Subject<Unit>();
    
    public Subject<Unit> onlaunchend = new Subject<Unit>();
    
    public Subject<Unit> onHitObstacle = new Subject<Unit>();
    
    private CompositeDisposable launchDisposables = new CompositeDisposable();

    private void Awake()
    {
        debugButton.onClick.AddListener(() => onshakedone.OnNext(Unit.Default));
        backGroundInitPos = backGround.transform.localPosition;
    }
    public void Initialize()
    {
        launchDisposables = new CompositeDisposable();
        
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
        //手の画像の初期化
        handImage.transform.localPosition = new Vector3(-16, 0, 0);
        
        thumbImage.GetComponent<Image>().DOFade(1,0.5f);
        fingerImage.GetComponent<Image>().DOFade(1,0.5f);
        paramImage.GetComponent<Image>().DOFade(1,0.5f);
        
        //コーラ画像場所の初期化
        coraImage.transform.localPosition = new Vector3(0, 900, 0);
        coraImage.transform.localEulerAngles = new Vector3(0, 0, 0);
        
        //「ふれ！」画像表示
        shakeImg.SetActive(false);
        
        //コーラエネルギー表示
        coraEneImage.SetActive(true);
        
        //タイマー表示
        timerText.gameObject.SetActive(true);
        
        //スコア用テキスト非表示
        scoreText.gameObject.SetActive(false);
        
        //発射エフェクト非表示
        jetEffect.SetActive(false);
        jetEffect.GetComponent<ParticleSystem>().Stop();
        
        //背景画像位置の初期化
        backGround.transform.localPosition = backGroundInitPos;
        backGround.transform.DOScale(new Vector3(3,3,3),0.5f);

        handImage.transform.DOScale(new Vector3(1, 1, 1), 1);
        
        scoreText.gameObject.SetActive(false);
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
        float p = Mathf.Clamp01(shakeCount / 45f); // 進行度 0→1
        coraTimerBar.fillAmount = p;
        coraCO2Mask.fillAmount = p;
        
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

    public void PreparationLaunchCora()
    {
        handImage.transform.localPosition = new Vector3(0, 0, 0);
        handImage.transform.localEulerAngles = new Vector3(0, 0, 0);
        
        shakeImg.SetActive(false);
        timerText.gameObject.SetActive(false);
        shakeImg.SetActive(false);
        preparationLaunchSequence = DOTween.Sequence()
            .Append(backGround.transform.DOScale(new Vector3(1,1,1),1))
            .Join(handImage.transform.DOScale(new Vector3(0.5f,0.5f,0.5f),1))
            .Append(coraImage.transform.DOLocalMove(new Vector3(0, -500, 0), 0.5f))
            .Join(coraImage.transform.DORotate(new Vector3(0, 0, 180), 0.5f, RotateMode.FastBeyond360))
            .Join(thumbImage.GetComponent<Image>().DOFade(0,0.5f))
            .Join(fingerImage.GetComponent<Image>().DOFade(0,0.5f))
            .Join(paramImage.GetComponent<Image>().DOFade(0,0.5f))
            .AppendInterval(0.25f)
            .Append(coraImage.transform.DOShakePosition(3f, 10f, 100, 90f, false, false))
            .Append(coraImage.transform.DOLocalMove(new Vector3(0, -240, 0), 0.01f))
            .OnComplete(() =>
            {
                onlaunch.OnNext(Unit.Default);
                preparationLaunchSequence.Kill();
            });
    }

    public void LaunchCora(float power,float time)
    {
        scoreText.gameObject.SetActive(true);
        jetEffect.SetActive(true);

        var ps = jetEffect.GetComponent<ParticleSystem>();
        ps.Play();
        
        Debug.Log($"Power = {power}");
        
        coraEneImage.SetActive(false);

        coraImage.gameObject.OnTriggerEnter2DAsObservable()
            .Subscribe(hit =>
            {
                if (hit.gameObject.CompareTag("Obstacle")) onHitObstacle.OnNext(Unit.Default);
            }).AddTo(launchDisposables);
        
        launchSequence = DOTween.Sequence();
        launchSequence.Append(backGround.transform.DOLocalMoveY(backGround.transform.localPosition.y-power, time))
            .Join(DOVirtual.Float(0,power,time, value =>
            {
                scoreText.text = value.ToString("0");
            }))
            .SetEase(Ease.InOutCubic)
            .Join(DOVirtual.Float(100,0,time, value =>
            {
                var psem = ps.emission;
                psem.rateOverTimeMultiplier = value;
            }))
            .SetEase(Ease.InCubic)
            .Join(coraImage.transform.DOLocalMoveY(200,time/5))
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                onlaunchend.OnNext(Unit.Default);
            });
    }

    public void MoveCora(float swipVct)
    {
        if (-1750 < coraImage.transform.localPosition.x || coraImage.transform.localPosition.x < 1750)
        {
            coraImage.transform.Translate(new Vector3(swipVct * Time.deltaTime, 0, 0));
        }else if (coraImage.transform.localPosition.x < -1750)
        {
            if (swipVct < 0) return;
            coraImage.transform.Translate(new Vector3(swipVct * Time.deltaTime, 0, 0));
        }else if (1750 < coraImage.transform.localPosition.x)
        {
            if (swipVct > 0) return;
            coraImage.transform.Translate(new Vector3(swipVct * Time.deltaTime, 0, 0));       
        }
    }

    public void LaunchEnd()
    {
        //シークエンスを切る
        launchSequence.Kill();
        
        launchDisposables.Dispose();
        
        jetEffect.GetComponent<ParticleSystem>().Stop();
        jetEffect.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    public async UniTask hitObstacle()
    {
        Debug.Log("💥 障害物ヒット！揺れ開始");

        // DOTween シーケンス（揺れアニメーション）
        Sequence seq = DOTween.Sequence();
        seq.Append(coraImage.transform.DOShakePosition(0.3f, 30, 10, 90f, false, true))
            .Join(coraImage.transform.DOShakeRotation(0.7f, 10, 10))
            .SetEase(Ease.OutCubic);

        // DOTween は await で待機できる（AsyncWaitForCompletion）
        await seq.AsyncWaitForCompletion();

        Debug.Log("✅ 揺れ完了");
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