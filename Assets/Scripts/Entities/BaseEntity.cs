using UnityEngine;

public class BaseEntity : MonoBehaviour {
  public Entity entity;

  public void ApplyDamage(int damage) {
    entity.health -= damage;
    entity.healthBar.SetHealth(entity.health);
  }
}
