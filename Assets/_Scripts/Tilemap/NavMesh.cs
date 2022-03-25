using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour {
  public NavMeshSurface2d Surface2D;

  public void Bake() {
    Surface2D.BuildNavMeshAsync();
  }

  public void Rebake() {
    if(Surface2D.navMeshData != null)
      Surface2D.UpdateNavMesh(Surface2D.navMeshData);
  }
}
