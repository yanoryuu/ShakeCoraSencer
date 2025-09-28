
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject coraImage;
    [SerializeField] private GameObject handImage;
    private bool isUpCora;
    
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
}
