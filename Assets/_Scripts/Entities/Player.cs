using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCreature {
  [SerializeField] private Transform cam;
  // [SerializeField] private HealthBar hudHealthBar;

  public override void Start() {
    base.Start();
    // hudHealthBar.SetMaxHealth(maxHealth);
  }

  public override void FixedUpdate() {
    base.FixedUpdate();
    SetMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    cam.position = new Vector3(transform.position.x, transform.position.y, -10);
  }

  public override void ApplyDamage(int damage) {
    base.ApplyDamage(damage);
    // hudHealthBar.SetHealth(health);
  }

  public void Spawn() {
    GameTile spawnTile = MapManager.Instance.GetTile(new Vector2Int(32, 32));
    gameObject.transform.position = MapManager.Instance.terrainTilemap.CellToWorld((Vector3Int) spawnTile.coords);
  }

  public override void Die() {
    base.Die();
    GameManager.Instance.UpdateGameState(GameState.GameOver);
  }
}