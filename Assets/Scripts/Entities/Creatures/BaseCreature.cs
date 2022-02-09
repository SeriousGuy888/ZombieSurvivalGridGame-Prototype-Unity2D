using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseCreature : BaseEntity {
  public Vector2 movement;
  public float moveSpeed = 5;


  public bool enablePathfinding;
  public Player targetPlayer;
  public NavMeshAgent agent;


  public override void Init() {
    base.Init();

    // initiate pathfinding
    if(enablePathfinding) {
      targetPlayer = GameManager.Instance.player;

      agent = GetComponent<NavMeshAgent>();
      agent.updateRotation = false;
      agent.updateUpAxis = false;
      agent.transform.rotation = Quaternion.identity;
    }
  }

  public override void Tick() {
    base.Tick();

    rb.MovePosition(rb.position + (movement * moveSpeed * Time.fixedDeltaTime));
    this.movement = Vector2.zero;

    if(enablePathfinding)
      agent.SetDestination(targetPlayer.transform.position);
  }


  public void SetMovement(float x, float y) {
    SetMovement(new Vector2(x, y));
  }
  public void SetMovement(Vector2 vec) {
    this.movement = vec.normalized;
  }
}