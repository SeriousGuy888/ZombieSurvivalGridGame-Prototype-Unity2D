using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour {
  public NavMeshSurface2d Surface2D;


  private void Start() {
    InvokeRepeating("UpdateNavMesh", 1f, 1f);
  }

  public void Bake() {
    Surface2D.BuildNavMeshAsync();
  }

  private void UpdateNavMesh() { 
    Surface2D.UpdateNavMesh(Surface2D.navMeshData);
  }
}