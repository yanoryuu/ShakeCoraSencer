using System;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : MonoBehaviour
{
    [SerializeField] private GameObject Title;
    [SerializeField] private Button startButton;

    public Subject<Unit> onStart { get; private set; } = new();

    private void Awake()
    {
        startButton.onClick.AddListener(()=>onStart.OnNext(Unit.Default));
    }

    public void Initialize()
    {
        
    }
}
