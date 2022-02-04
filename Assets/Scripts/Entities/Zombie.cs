using System;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : BaseEntity {
  [SerializeField] private int raycastDistance = 32;

  AStarPathfinder pathfinder;
  private Player targetPlayer;

  private List<Vector2Int> waypoints;

  private float circleCastRadius;


  private void Start() {
    pathfinder = gameObject.AddComponent<AStarPathfinder>();
    targetPlayer = GameManager.Instance.player;
    ResetWaypoints();

    circleCastRadius = entity.boxCollider.size.x / 2;
  }

  private void FixedUpdate() {

    Vector2 vecBetween = targetPlayer.transform.position - transform.position;
    RaycastHit2D hit = Physics2D.CircleCast(
      origin:    transform.position,
      radius:    circleCastRadius,
      direction: vecBetween.normalized,
      distance:  raycastDistance,
      layerMask: LayerMask.GetMask(new string[] { "Obstacle", "Entity" }));
    

    if(hit.collider != null && hit.collider.gameObject.Equals(targetPlayer.gameObject)) {
      entity.SetMovement(vecBetween);
      ResetWaypoints();
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
            radius:    circleCastRadius,
            direction: vecBetween.normalized,
            distance:  Math.Min(raycastDistance, vecBetween.magnitude),
            layerMask: LayerMask.GetMask(new string[] { "Obstacle" }));
          
          if(hit.collider != null)
            continue;
          
          entity.SetMovement(vecBetween);
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
    

    Collider2D[] nearbyColliders = Physics2D
      .OverlapCircleAll(transform.position, 5, LayerMask.GetMask(new string[] { "Obstacle" }));

    foreach(var collider in nearbyColliders) {
      Vector2 coords = collider.transform.position;
      if(!obstructedCoords.Contains(coords))
        obstructedCoords.Add(coords);
    }
    
    waypoints = pathfinder.FindPath(transform.position, targetPlayer.transform.position, obstructedCoords);
  }

  private void ResetWaypoints() {
    waypoints = new List<Vector2Int>();
  }

  
  private void OnMouseUp() {
    ApplyDamage(10);
  }
}
