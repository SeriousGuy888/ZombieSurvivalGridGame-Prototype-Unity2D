using UnityEngine;

public class BaseEntity : MonoBehaviour {
  public int maxHealth = 100;
  public int health;

  public Rigidbody2D rb;

  public virtual void Start() {
    health = maxHealth;
  }

  public virtual void FixedUpdate() {
    if(health <= 0)
      Die();
  }

  public virtual void SetHealth(int health) {
    this.health = health;
  }
  public virtual void ApplyDamage(int damage) {
    SetHealth(health - damage);
  }
  
  private void OnMouseUp() {
    ApplyDamage(10);
  }

  public virtual void Die() {
    Debug.Log(gameObject.name + ": oeuf");
    Destroy(gameObject);
  }

  public void SetVelocity(Vector2 vec) => rb.velocity = vec;
}