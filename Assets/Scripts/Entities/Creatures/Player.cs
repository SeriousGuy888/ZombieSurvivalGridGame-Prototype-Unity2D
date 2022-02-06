using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCreature {
  [SerializeField] private Transform cam;
  [SerializeField] private HealthBar hudHealthBar;

  public override void Init() {
    base.Init();
    hudHealthBar.SetMaxHealth(maxHealth);
  }

  public override void Tick() {
    base.Tick();
    SetMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    cam.position = new Vector3(transform.position.x, transform.position.y, -10);
  }

  public override void ApplyDamage(int damage) {
    base.ApplyDamage(damage);
    hudHealthBar.SetHealth(health);
  }

  public void Spawn() {
    BaseTile spawnTile = MapManager.Instance.GetTile(MapManager.Instance.GetSpawnPoint);
    gameObject.transform.position = spawnTile.transform.position;
  }

  public override void Die() {
    base.Die();
    GameManager.Instance.UpdateGameState(GameState.GameOver);
  }
}