using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoisyAutoRotation : MonoBehaviour
{

    public float RotateSpeed = 25.0f;
    public float NoiseSpeed  = 0.25f;

    float _noiseTimer = 0.0f;

    void Update()
    {
        Vector3 rotateAxis = new Vector3
        (
            Mathf.PerlinNoise(13.7f, _noiseTimer),
            Mathf.PerlinNoise(43.1f, _noiseTimer),
            Mathf.PerlinNoise(66.6f, _noiseTimer)
        );

        transform.Rotate(rotateAxis, Time.deltaTime * RotateSpeed);

        _noiseTimer += Time.deltaTime * NoiseSpeed;
    }
}
