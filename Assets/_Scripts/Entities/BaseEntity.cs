using UnityEngine;

public class BaseEntity : MonoBehaviour {
  // public HealthBar healthBar;
  public int maxHealth = 100;
  public int health;

  public virtual void Start() {
    health = maxHealth;
    // healthBar.SetMaxHealth(maxHealth);
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
    // healthBar.SetHealth(health);
  }

  // private void OnMouseEnter() {
  //   healthBar.gameObject.SetActive(true);
  // }

  // private void OnMouseExit() {
  //   healthBar.gameObject.SetActive(false);
  // }
  
  private void OnMouseUp() {
    ApplyDamage(10);
  }

  public virtual void Die() {
    Debug.Log(gameObject.name + ": oeuf");
    Destroy(gameObject);
  }
}