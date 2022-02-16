using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;
  private void Awake() {
    Instance = this;
  }


  public GameState gameState;
  public Player player;

  private void Start() {
    UpdateGameState(GameState.BuildMap);
  }

  public void UpdateGameState(GameState gameState) {
    this.gameState = gameState;

    switch(gameState) {
      case GameState.BuildMap:
        MapManager.Instance.BuildMap();
        break;
      case GameState.Play:
        break;
    }
  }
}

public enum GameState {
  BuildMap,
  Play,
  GameOver,
}