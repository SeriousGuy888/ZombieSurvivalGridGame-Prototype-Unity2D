using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class NewZombie : BaseEntity {
  [SerializeField] private int raycastDistance = 32;

  AStarPathfinder pathfinder;
  private Player targetPlayer;

  private List<Vector2Int> waypoints;

  private void Start() {
    pathfinder = gameObject.AddComponent<AStarPathfinder>();
    targetPlayer = GameManager.Instance.player;
    ResetWaypoints();
  }

  private void FixedUpdate() {
    // entity.rb.MovePosition(
    //   Vector2.MoveTowards(
    //     transform.position,
    //     targetPlayer.transform.position,
    //     entity.moveSpeed * Time.fixedDeltaTime));
    

    Vector2 raycastDir = targetPlayer.transform.position - transform.position;
    RaycastHit2D hit = Physics2D.Raycast(
      transform.position,
      raycastDir, raycastDistance,
      LayerMask.GetMask(new string[] { "Obstacle", "Entity" }));
    
    Debug.Log(hit.collider.gameObject.name);

    if(hit.collider != null && hit.collider.gameObject.Equals(targetPlayer.gameObject)) {
      entity.SetMovement(raycastDir);
      ResetWaypoints();
    } else {
      if(waypoints == null || waypoints.Count == 0) {
        Debug.Log(waypoints?.Count);
        
        Collider2D[] nearbyColliders = Physics2D
          .OverlapCircleAll(transform.position, 5, LayerMask.GetMask(new string[] { "Obstacle" }));

        var obstructedCoords = new List<Vector2>();
        foreach(var collider in nearbyColliders) {
          Vector2 coords = collider.transform.position;

          if(!obstructedCoords.Contains(coords))
            obstructedCoords.Add(coords);
        }
        
        
        waypoints = pathfinder.FindPath(transform.position, targetPlayer.transform.position, obstructedCoords);

        Debug.Log("a");
      } else {
        Debug.Log("b");


        for(int i = 0; i < waypoints.Count; i++) {
          if(Vector2.Distance(transform.position, waypoints[i]) < 1) {
            waypoints.Remove(waypoints[i]);
            break;
          }
        }

        bool waypointFound = false;
        foreach(var waypoint in waypoints) {
          raycastDir = waypoint - (Vector2) transform.position;
          hit = Physics2D.Raycast(
            transform.position,
            raycastDir, raycastDistance,
            LayerMask.GetMask(new string[] { "Obstacle" }));
          
          Debug.Log("c");
          if(hit.collider != null) {
            Debug.Log("d");
            entity.SetMovement(raycastDir);
            waypointFound = true;
            break;
          }
        }

        if(!waypointFound) {
          ResetWaypoints();
        }
      }


      // foreach(var scentMarkerObj in targetPlayer.scentTrail) {
      //   raycastDir = scentMarkerObj.transform.position - transform.position;
      //   hit = Physics2D.Raycast(
      //     transform.position,
      //     raycastDir, raycastDistance,
      //     LayerMask.GetMask(new string[] { "Obstacle", "ScentMarker" }));
        
      //   Destroy(scentMarkerObj);
      //   // Debug.Log(hit.collider.gameObject.name);

      //   if(hit.collider != null) {
      //     ScentMarker scentMarker = hit.collider.gameObject.GetComponent<ScentMarker>();
      //     if(scentMarker != null && scentMarker.player.Equals(targetPlayer)) {
      //       entity.SetMovement(raycastDir);
      //     }
      //   }
      // }
    }
  }

  private void ResetWaypoints() {
    waypoints = new List<Vector2Int>();
  }

  
  private void OnMouseUp() {
    ApplyDamage(10);
  }
}
