using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameObject[] enemies;

    public GameObject player;

    public GameState state;

    public static event Action<GameState> OnGameStateChange;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Active);
    }
    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Active:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);

    }


    public enum GameState
    {
        Active,
        Victory,
        Lose
    }
}
