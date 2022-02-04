using UnityEngine;

public class Zombie : BaseEntity {
  private void OnMouseUp() {
    ApplyDamage(10);
  }
}
