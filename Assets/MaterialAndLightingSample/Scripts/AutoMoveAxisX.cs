using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveAxisX : MonoBehaviour
{

    public float Amplitude = 2.0f;
    public float Speed = 1.0f;

    Vector3 _defaultPosition = Vector3.zero;

    private void Awake()
    {
        _defaultPosition = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = _defaultPosition + new Vector3(Amplitude * Mathf.Sin(Time.time * Speed), 0.0f, 0.0f);
    }
}
