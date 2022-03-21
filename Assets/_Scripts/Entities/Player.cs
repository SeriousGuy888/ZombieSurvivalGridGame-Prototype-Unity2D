using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCreature {
  [SerializeField] private Transform cam;
  public InventorySystem inventorySystem;

  public override void Start() {
    base.Start();
  }

  public override void FixedUpdate() {
    base.FixedUpdate();
    SetMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    cam.position = new Vector3(transform.position.x, transform.position.y, -10);
  }

  public override void SetHealth(int health) {
    base.SetHealth(health);
    GameManager.Instance.healthBar.SetHealth(health);
  }

  public override void Die() {
    base.Die();
    GameManager.Instance.UpdateGameState(GameState.GameOver);
  }

  public void Spawn() {
    GameTile spawnTile = MapManager.Instance.GetTile(new Vector2Int(32, 32));
    gameObject.transform.position = MapManager.Instance.terrainTilemap.CellToWorld((Vector3Int) spawnTile.coords);
  }

  // on trigger enter, if the collider is a dropped item, pick it up
  private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.tag == "DroppedItem") {
      DroppedItem droppedItem = other.gameObject.GetComponent<DroppedItem>();
      droppedItem.PickUp();
    }
  }
}