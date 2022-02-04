using System.Collections.Generic;
using UnityEngine;

public class Player : BaseEntity {
  [SerializeField] private Transform cam;
  [SerializeField] private HealthBar hudHealthBar;

  // public List<ScentMarker> scentTrail;

  private void Start() {
    // scentTrail = new List<ScentMarker>();
    // InvokeRepeating("AddScentMarker", 0f, 0.1f);

    hudHealthBar.SetMaxHealth(entity.maxHealth);
  }

  private void Update() {
    entity.SetMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
  }

  private void FixedUpdate() {
    cam.position = new Vector3(transform.position.x, transform.position.y, -10);
  }

  // private void AddScentMarker() {
  //   ScentMarker newScentMarker = ObjectPooler.Instance
  //     .SpawnFromPool("ScentMarker", transform.position, Quaternion.identity)
  //     .GetComponent<ScentMarker>();

  //   if(newScentMarker == null)
  //     return;

  //   newScentMarker.player = this;
  //   newScentMarker.transform.position = transform.position;
  //   scentTrail.Add(newScentMarker);
  // }


  public void UpdateHUDHealthBar() {
    hudHealthBar.SetHealth(entity.health);
  }

  new public void ApplyDamage(int damage) {
    entity.health -= damage;
    entity.healthBar.SetHealth(entity.health);
    hudHealthBar.SetHealth(entity.health);
  }

  public void Spawn() {
    BaseTile spawnTile = GridManager.Instance.GetTile(GridManager.Instance.GetSpawnPoint);
    gameObject.transform.position = spawnTile.transform.position;
  }
}