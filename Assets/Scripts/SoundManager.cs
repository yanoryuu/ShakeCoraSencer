using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Mixers")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> bgmClips = new();
    [SerializeField] private List<AudioClip> seClips = new();

    private Dictionary<string, AudioClip> bgmDict = new();
    private Dictionary<string, AudioClip> seDict = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var clip in bgmClips)
            bgmDict[clip.name] = clip;

        foreach (var clip in seClips)
            seDict[clip.name] = clip;
    }

    // === BGM ===
    public async UniTask PlayBGM(string name, float fadeTime = 1f)
    {
        if (!bgmDict.ContainsKey(name))
        {
            Debug.LogWarning($"BGM '{name}' not found!");
            return;
        }
        
        Debug.Log($"PlayBGM: {name}");

        var clip = bgmDict[name];

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        await FadeOutBGM(fadeTime);
        bgmSource.clip = clip;
        bgmSource.Play();
        await FadeInBGM(fadeTime);
    }

    public async UniTask FadeOutBGM(float time)
    {
        float startVol = bgmSource.volume;
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVol, 0f, t / time);
            await UniTask.Yield();
        }
        bgmSource.volume = 0f;
        bgmSource.Stop();
    }

    public async UniTask FadeInBGM(float time)
    {
        float t = 0f;
        bgmSource.volume = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, 1f, t / time);
            await UniTask.Yield();
        }
        bgmSource.volume = 1f;
    }

    public void StopBGM() => bgmSource.Stop();

    // === SE ===
    public void PlaySE(string name)
    {
        if (!seDict.ContainsKey(name))
        {
            Debug.LogWarning($"SE '{name}' not found!");
            return;
        }
        
        Debug.Log($"PlaySE: {name}");
        
        seSource.PlayOneShot(seDict[name]);
    }

    public void StopAllSE() => seSource.Stop();

    // === Volume Control ===
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
    }

    public void SetSEVolume(float value)
    {
        audioMixer.SetFloat("SEVolume", Mathf.Log10(value) * 20);
    }
}