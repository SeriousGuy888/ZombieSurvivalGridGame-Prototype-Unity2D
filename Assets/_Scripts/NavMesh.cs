using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour {
  public NavMeshSurface2d Surface2D;

  public void Bake() {
    Surface2D.BuildNavMeshAsync();
  }
}
