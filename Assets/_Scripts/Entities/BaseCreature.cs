using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreature : BaseEntity {
  public BoxCollider2D boxCollider;
  public Vector2 movement;
  public float moveSpeed = 5;


  public override void FixedUpdate() {
    base.FixedUpdate();

    rb.velocity = movement * moveSpeed;
    this.movement = Vector2.zero;
  }

  public void SetMovement(float x, float y) => SetMovement(new Vector2(x, y));
  public void SetMovement(Vector2 vec) => this.movement = vec.normalized;
}