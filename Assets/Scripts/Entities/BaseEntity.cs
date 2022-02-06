using UnityEngine;

public class BaseEntity : MonoBehaviour {
  public Rigidbody2D rb;
  public BoxCollider2D boxCollider;
  public HealthBar healthBar;
  public int maxHealth = 100;
  public int health;

  public virtual void Init() {
    health = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
  }

  public virtual void Tick() {
    if(health <= 0)
      Die();
  }

  public virtual void ApplyDamage(int damage) {
    health -= damage;
    healthBar.SetHealth(health);
  }

  private void OnMouseEnter() {
    healthBar.gameObject.SetActive(true);
  }

  private void OnMouseExit() {
    healthBar.gameObject.SetActive(false);
  }
  
  private void OnMouseUp() {
    ApplyDamage(10);
  }

  public virtual void Die() {
    Debug.Log(gameObject.name + ": oeuf");
    Destroy(gameObject);
  }
}
