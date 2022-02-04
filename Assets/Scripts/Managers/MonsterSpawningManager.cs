using UnityEngine;

public class MonsterSpawningManager : MonoBehaviour {
  public static MonsterSpawningManager Instance;

  [SerializeField] BaseEntity zombie;

  private void Awake() {
    Instance = this;
  }

  // private void Update() {
  //   if(Input.GetKeyDown(KeyCode.Z)) {
  //     SpawnZombie(GameManager.Instance.player.transform.position);
  //   }
  // }

  public void SpawnZombie(Vector2 position) {
    Instantiate(zombie, position, Quaternion.identity);
  }
}
