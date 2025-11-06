using System;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject ingame;
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject result;

    public void ChangePanel(GameState state)
    {
        ingame.SetActive(false);
        home.SetActive(false);
        result.SetActive(false);
        
        switch (state)
        {
            case GameState.home:
                home.SetActive(true);
                break;
            case GameState.ingame:
                ingame.SetActive(true);
                break;
            case GameState.result:
                ingame.SetActive(true);
                result.SetActive(true);
                break;
        }
    }
}

public enum GameState
{
    home,
    ingame,
    result
}
