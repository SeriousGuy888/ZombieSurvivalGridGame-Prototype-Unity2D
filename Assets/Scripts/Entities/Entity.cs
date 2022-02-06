using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
  public BaseEntity specificEntityScript;

  private void Start() {
    specificEntityScript.Init();
  }

  private void FixedUpdate() {
    specificEntityScript.Tick();
  }
}