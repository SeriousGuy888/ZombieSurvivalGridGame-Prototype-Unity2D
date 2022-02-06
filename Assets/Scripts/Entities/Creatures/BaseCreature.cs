using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreature : BaseEntity {
  public Vector2 movement;
  public float moveSpeed = 5;


  [SerializeField] private bool enablePathfinding;
  [SerializeField] private int maxRaycastDistance = 32; // the maximum distance the entity can see the player from
  [SerializeField] private LayerMask obstacleMask;      // the layers that are considered obstacles
  [SerializeField] private LayerMask lineOfSightMask;   // the layers that can block line of sight
  private AStarPathfinder pathfinder; // an instance of the pathfinder
  private Player targetPlayer;        // player that the entity is currently targeting
  private List<Vector2Int> waypoints; // the waypoints found by the pathfinder

  // used for circlecasts so the entity will discard paths too narrow for it
  // also used to stop the entity from pushing on the player too much and causing lag from a lot of physics collisions
  private float hitboxRadius;



  public override void Init() {
    base.Init();

    // initiate pathfinding
    if(enablePathfinding) {
      pathfinder = gameObject.AddComponent<AStarPathfinder>();
      targetPlayer = GameManager.Instance.player;
      hitboxRadius = boxCollider.size.x / 2;
      waypoints = new List<Vector2Int>();
    }
  }

  public override void Tick() {
    base.Tick();

    if(enablePathfinding)
      RunPathfinding();

    rb.MovePosition(rb.position + (movement * moveSpeed * Time.fixedDeltaTime));
    this.movement = Vector2.zero;
  }


  public void SetMovement(float x, float y) {
    SetMovement(new Vector2(x, y));
  }
  public void SetMovement(Vector2 vec) {
    this.movement = vec.normalized;
  }



  private void RunPathfinding() {
    Vector2 vecBetween = targetPlayer.transform.position - transform.position;
    RaycastHit2D hit = Physics2D.CircleCast(
      origin:    transform.position,
      radius:    hitboxRadius,
      direction: vecBetween.normalized,
      distance:  maxRaycastDistance,
      layerMask: lineOfSightMask);
    

    if(hit.collider != null && hit.collider.gameObject.Equals(targetPlayer.gameObject)) {
      SetMovement(vecBetween);

      // FindNewPath(null);
    } else {
      if(waypoints == null || waypoints.Count == 0) {
        FindNewPath(null);
      } else {
        for(int i = 0; i < waypoints.Count; i++) {
          if(Vector2.Distance(transform.position, waypoints[i]) < 0.1) {
            waypoints.Remove(waypoints[i]);
            break;
          }
        }

        bool waypointFound = false;
        foreach(var waypoint in waypoints) {
          vecBetween = waypoint - (Vector2) transform.position;
          hit = Physics2D.CircleCast(
            origin:    transform.position,
            radius:    hitboxRadius,
            direction: vecBetween.normalized,
            distance:  Math.Min(maxRaycastDistance, vecBetween.magnitude),
            layerMask: obstacleMask);
          
          if(hit.collider != null)
            continue;
          
          // don't move if already touching the player, as that will create rapid unnecessary physics calculations
          if(Vector2.Distance(transform.position, targetPlayer.transform.position) > hitboxRadius)
            SetMovement(vecBetween);


          waypointFound = true;
          break;
        }

        if(!waypointFound) {
          if(waypoints.Count > 0)
            FindNewPath(waypoints[0]);
          else
            FindNewPath(null);
        }
      }
    }
  }


  private void FindNewPath(Vector2? obstructedWaypoint) {
    var obstructedCoords = new List<Vector2>();
    if(obstructedWaypoint.HasValue)
      obstructedCoords.Add(obstructedWaypoint.Value);
    

    Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, 5, obstacleMask);
    foreach(var collider in nearbyColliders) {
      Vector2 coords = collider.transform.position;
      if(!obstructedCoords.Contains(coords))
        obstructedCoords.Add(coords);
    }
    
    waypoints = pathfinder.FindPath(transform.position, targetPlayer.transform.position, obstructedCoords);
  }
}