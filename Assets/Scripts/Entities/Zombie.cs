using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Zombie : BaseEntity {
  AStarPathfinder pathfinder;
  
  List<Vector2Int> waypoints;
  private int waypointIndex;
  private float lastPathfindSeconds;

  private void Start() {
    pathfinder = gameObject.AddComponent<AStarPathfinder>();
    PathfindToPlayer();
  }

  private void FixedUpdate() {
    MoveTowardsWaypoint();
  }

  private void OnMouseUp() {
    ApplyDamage(25);
    Debug.Log("waypoints " + waypoints.Count);
    Debug.Log("index " + waypointIndex);
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Entity")) && Time.time - lastPathfindSeconds > 3)
      PathfindToPlayer();
  }

  private void MoveTowardsWaypoint() {
    if(Time.time - lastPathfindSeconds > 15
        // || Vector2.SqrMagnitude(entity.rb.velocity) < 1
        || waypoints == null
        || waypoints.Count == 0
        || waypointIndex >= waypoints.Count - 1) {
      PathfindToPlayer();
      return;
    }

    // entity.rb.velocity = Vector2.zero;
    // transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex], entity.moveSpeed * Time.fixedDeltaTime);
    entity.rb.MovePosition(Vector2.MoveTowards(transform.position, waypoints[waypointIndex], entity.moveSpeed * Time.fixedDeltaTime));

    // if(Vector2.Distance((Vector2) transform.position, waypoints[waypointIndex]) <= 0.25)
    if((Vector2) transform.position == waypoints[waypointIndex])
      waypointIndex++;
  }

  public void PathfindToPlayer() {
    Player player = GameManager.Instance.player;

    if(Vector2.Distance(transform.position, player.transform.position) <= 1)
      return;

    Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, 5, LayerMask.NameToLayer("Entities"));

    var obstructedCoords = new List<int2>();
    foreach(var collider in nearbyColliders) {
      if(collider.gameObject.Equals(player.gameObject) || collider.gameObject.Equals(gameObject))
        continue;

      int2 coords = Vector2ToInt2(collider.transform.position);
      if(coords.Equals(Vector2ToInt2(transform.position)))
        continue;

      if(!obstructedCoords.Contains(coords))
        obstructedCoords.Add(coords);
    }

    // waypoints = pathfinder.FindPath(transform.position, player.transform.position, obstructedCoords);
    waypointIndex = 0;
    lastPathfindSeconds = Time.time;
  }

  private int2 Vector2ToInt2(Vector2 vec) {
    return new int2((int) vec.x, (int) vec.y);
  }
}
