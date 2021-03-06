using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileStructure {
  public GameTile parentTile;
  public StructureType type;
  public int health;
  public int maxHealth;

  public GameTileStructure(GameTile parentTile, StructureType? structureType) {
    this.parentTile = parentTile;
    if(structureType.HasValue)
      SetType(structureType.Value);
  }

  public void SetType(StructureType structureType) {
    this.type = structureType;
    if(structureType == StructureType.None)
      SetMaxHealth(0);
    else
      SetMaxHealth(10); // temp
  }

  private void SetMaxHealth(int maxHealth) {
    this.maxHealth = maxHealth;
    this.health = maxHealth;
  }
  public virtual void SetHealth(int health) {
    this.health = health;
    Debug.Log($"Set health of structure at {parentTile.coords} - new health: {health}");

    if(health <= 0) {
      var structureType = type;
      SetType(StructureType.None);

      if(structureType == StructureType.Tree) {
        DroppedItemsManager.Instance.SpawnItemWithRandVel(
          parentTile.coords, DroppedItemsManager.Instance.itemRegistry.log);
        DroppedItemsManager.Instance.SpawnItemWithRandVel(
          parentTile.coords, DroppedItemsManager.Instance.itemRegistry.apple);
      }

      MapManager.Instance.QueueMapRerender();
    }
  }
  public virtual void ApplyDamage(int damage) => SetHealth(health - damage);
}
