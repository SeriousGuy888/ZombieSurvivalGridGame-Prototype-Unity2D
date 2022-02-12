using UnityEngine;

public class BaseTile : MonoBehaviour {
  [SerializeField] protected SpriteRenderer _renderer;
  [SerializeField] public GameObject highlight; // aaaa

  [SerializeField] private GameObject cratePrefab;

  public Vector2Int coords;
  private bool isWalkable = true;


  public void Init(int x, int y) {
    coords = new Vector2Int(x, y);

    if(LayerMask.LayerToName(gameObject.layer) == "Obstacle")
      isWalkable = false;

    InitTile();
  }
  protected virtual void InitTile() {}

  public bool GetWalkable() => isWalkable;

  void OnMouseEnter() {
    highlight.SetActive(true);
  }

  void OnMouseExit() {
    highlight.SetActive(false);
  }

  private void OnMouseOver() {
    if(Input.GetMouseButtonDown(0))
      MonsterSpawningManager.Instance.SpawnZombie(transform.position);
    if(Input.GetMouseButtonDown(1)) {
      if(cratePrefab != null) {
        Instantiate(cratePrefab, transform.position, Quaternion.identity);
        MapManager.Instance.RefreshNavMesh();
      }
    }
  }
}