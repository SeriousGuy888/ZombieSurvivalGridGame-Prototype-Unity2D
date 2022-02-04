using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorial used https://www.youtube.com/watch?v=tdSmKaJvCoA
public class ObjectPooler : MonoBehaviour {
  public static ObjectPooler Instance;

  [System.Serializable]
  public class Pool {
    public string tag;
    public GameObject prefab;
    public int size;
  }
  
  public List<Pool> pools;
  public Dictionary<string, Queue<GameObject>> poolDictionary;

  private void Awake() {
    Instance = this;
  }

  private void Start() {
    poolDictionary = new Dictionary<string, Queue<GameObject>>();

    foreach(var pool in pools) {
      Queue<GameObject> objectPool = new Queue<GameObject>();
      for(int i = 0; i < pool.size; i++) {
        GameObject obj = Instantiate(pool.prefab);
        obj.SetActive(false);
        objectPool.Enqueue(obj);
      }

      poolDictionary.Add(pool.tag, objectPool);
    }
  }

  public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation) {
    if(!poolDictionary.ContainsKey(tag)) {
      Debug.LogWarning($"Pool with tag {tag} does not exist.");
      return null;
    }

    GameObject obj = poolDictionary[tag].Dequeue();
    
    obj.SetActive(true);
    obj.transform.position = position;
    obj.transform.rotation = rotation;

    poolDictionary[tag].Enqueue(obj);

    return obj;
  }
}