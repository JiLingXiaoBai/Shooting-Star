using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] GameState gameState = GameState.Playing;

    public static System.Action onGameOver;

    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }
}

public enum GameState
{
    Playing, Paused, GameOver, Scoring
}
