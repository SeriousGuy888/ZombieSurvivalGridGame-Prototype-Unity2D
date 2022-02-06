using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;
  public GameState State;

  public Player player;

  private void Awake() {
    Instance = this;
  }
  
  private void Start() {
    UpdateGameState(GameState.GenerateMap);
  }

  public void UpdateGameState(GameState newState) {
    State = newState;

    switch(newState) {
      case GameState.GenerateMap:
        MapManager.Instance.GenerateMap();
        break;
      case GameState.SpawnPlayer:
        player.Spawn();
        UpdateGameState(GameState.Play);
        break;
      case GameState.Play:
        break;
    }
  }
}

public enum GameState {
  GenerateMap,
  SpawnPlayer,
  Play,
}