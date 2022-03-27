using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : BaseCreature {
  public Player targetPlayer;
  public NavMeshAgent agent;

  public override void Start() {
    base.Start();

    targetPlayer = GameManager.Instance.player;
    agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
    agent.transform.rotation = Quaternion.identity;
  }

  public override void FixedUpdate() {
    base.FixedUpdate();

    if(gameObject != null && targetPlayer != null)
      agent.SetDestination(targetPlayer.transform.position);
  }

  private void OnMouseDown() {
    ApplyDamage(10);
    Debug.Log("ow! new health: " + health);
  }
}
