using UnityEngine;

public class SetButtonToColor : MonoBehaviour
{

    public IMUInputManager _imuInputManager;

    public Material _material;
    
    void Start()
    {
        
    }

    void Update()
    {
        if ( _imuInputManager != null && _material != null)
        {
            if (_imuInputManager.ButtonA)
            {
                _material.SetColor("_BaseColor", new Color(1, 0, 0, 1));
            }
            else if (_imuInputManager.ButtonB)
            {
                _material.SetColor("_BaseColor", new Color(0, 1, 0, 1));
            }
            else if (_imuInputManager.ButtonC)
            {
                _material.SetColor("_BaseColor", new Color(0, 0, 1, 1));
            }
            else
            {
                _material.SetColor("_BaseColor", new Color(1, 1, 1, 1));
            }
        }        
    }
}
