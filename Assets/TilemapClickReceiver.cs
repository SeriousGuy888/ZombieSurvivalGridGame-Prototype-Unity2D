using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapClickReceiver : MonoBehaviour {
  private void Update() {
    if(Input.GetMouseButtonDown(0))
      TileSelection.Instance.Click();
  }
}
