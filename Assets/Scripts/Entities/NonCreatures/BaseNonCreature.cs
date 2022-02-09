using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNonCreature : BaseEntity {
  private void Start() {
    transform.SetParent(MapManager.Instance.navMeshParent.transform);
  }
}