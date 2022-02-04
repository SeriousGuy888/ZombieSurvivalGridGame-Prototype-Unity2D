using UnityEngine;

public class Entity : MonoBehaviour {
  public Rigidbody2D rb;
  public BoxCollider2D boxCollider;
  public HealthBar healthBar;
  public float moveSpeed = 5;
  public int maxHealth = 100;
  public Vector2 movement;
  public int health;

  private void Start() {
    health = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
  }

  private void FixedUpdate() {
    rb.MovePosition(rb.position + (movement * moveSpeed * Time.fixedDeltaTime));
    this.movement = Vector2.zero;

    if(health <= 0)
      Die();
  }

  private void OnMouseEnter() {
    healthBar.gameObject.SetActive(true);
  }

  private void OnMouseExit() {
    healthBar.gameObject.SetActive(false);
  }

  public void Die() {
    Debug.Log(gameObject.name + ": oeuf");
    Destroy(gameObject);
  }

  public void SetMovement(float x, float y) {
    SetMovement(new Vector2(x, y));
  }
  public void SetMovement(Vector2 vec) {
    this.movement = vec.normalized;
  }
}
