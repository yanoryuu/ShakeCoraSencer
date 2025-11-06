using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class StateManager
{
    public ReactiveProperty<GameState> currentState { get; private set; }
    
    /// <summary>フェーズ遷移時に実行する処理を登録するディクショナリ</summary>
    private readonly Dictionary<GameState, Action> onEnter = new();

    private CompositeDisposable disposables;

    private PanelManager panelManager;

    public StateManager(PanelManager panelManager)
    {
        currentState = new ReactiveProperty<GameState>(GameState.home);
        disposables = new CompositeDisposable();
        this.panelManager = panelManager;
        Bind();
        ChangeState(GameState.home);
    }

    private void Bind()
    {
        currentState.Subscribe(state => panelManager.ChangePanel(state));
        
        currentState
            .Subscribe(phase =>
            {
                try
                {
                    // まずUI切替（GamePanelManager）
                    panelManager?.ChangePanel(phase);

                    // 登録されたEnterイベント実行
                    if (onEnter.TryGetValue(phase, out var handler))
                    {
                        handler?.Invoke();
                    }

                    Debug.Log($"[StateManager] Phase changed to {phase}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[StateManager] OnEnter error at {phase}: {e}");
                }
            })
            .AddTo(disposables);
    }
    public void RegisterOnEnter(GameState phase, Action handler)
    {
        if (onEnter.ContainsKey(phase))
        {
            onEnter[phase] += handler; // 追加登録（複数OK）
        }
        else
        {
            onEnter[phase] = handler;
        }
    }
    
    public void ChangeState(GameState nextState)
    {
        if (currentState.Value == nextState) return;
        Debug.Log($"[StateManager] Changing phase: {currentState.Value} → {nextState}");
        currentState.Value = nextState;
    }
}
