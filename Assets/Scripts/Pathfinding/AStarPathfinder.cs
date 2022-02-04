using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class AStarPathfinder : MonoBehaviour {
  private PathNode[,] grid;
  
  private const int ORTHOGONAL_COST = 1000;
  private const int DIAGONAL_COST = 1414;
  private static readonly int2[] allNeighbourOffsets = new int2[] {
    new int2(1, 0),  new int2(-1, 0),
    new int2(0, 1),  new int2(0, -1),
    // new int2(1, 1),  new int2(-1, 1),
    // new int2(1, -1), new int2(-1, -1),
  };



  private void Awake() {
    this.grid = new PathNode[
      GridManager.Instance.width,
      GridManager.Instance.height
    ];

    var tilePairs = GridManager.Instance.GetAllTiles();
    if(tilePairs == null)
      return;

    foreach(var keyValuePair in tilePairs) {
      var coords = keyValuePair.Key;
      var tile = keyValuePair.Value;

      PathNode newNode = new PathNode();
      newNode.coords = new int2(tile.coords.x, tile.coords.y);
      newNode.isWalkable = tile.GetWalkable();
      
      grid[coords.x, coords.y] = newNode;
    }
  }

  public List<Vector2Int> FindPath(Vector2 startPos, Vector2 endPos, List<Vector2> obstructedNodes) {
    List<int2> obstructedNodesInt2 = new List<int2>();
    foreach(var node in obstructedNodes) {
      obstructedNodesInt2.Add(new int2((int) node.x, (int) node.y));
    }

    var path = CalcPath(
      new int2((int) startPos.x, (int) startPos.y),
      new int2((int) endPos.x, (int) endPos.y),
      obstructedNodesInt2
    );
  
    var pathVector2Int = new List<Vector2Int>();
    path.ForEach(coords => pathVector2Int.Add(new Vector2Int(coords.x, coords.y)));

    return pathVector2Int;
  }

  private List<int2> CalcPath(int2 startPos, int2 endPos, List<int2> obstructedNodes) {
    for(int x = 0; x < grid.GetLength(0); x++) {
      for(int y = 0; y < grid.GetLength(0); y++) {
        var node = grid[x, y];

        node.gCost = int.MaxValue;
        node.hCost = CalcDistanceCost(node.coords, endPos);
        node.CalcFCost();

        node.prevNodeCoords = new int2(-1);

        if(obstructedNodes.Contains(node.coords))
          node.isObstructed = true;

        grid[x, y] = node;
      }
    }

    int2 gridSize = new int2(grid.GetLength(0), grid.GetLength(1));


    List<int2> openList = new List<int2>(); // coords of nodes to be explored
    List<int2> closedList = new List<int2>(); // coords of nodes already explored


    if(!IsPosInGrid(startPos, gridSize) || !IsPosInGrid(endPos, gridSize))
      return new List<int2>();



    PathNode startNode = grid[startPos.x, startPos.y];
    startNode.gCost = 0;
    startNode.CalcFCost();
    openList.Add(startNode.coords);

    PathNode endNode = grid[endPos.x, endPos.y];
    
    while(openList.Count > 0) {
      int2 currentNodeCoords = GetLowestFCostNodeIndex(openList, grid);
      PathNode currentNode = grid[currentNodeCoords.x, currentNodeCoords.y];

      // destination reached
      if(currentNodeCoords.Equals(endNode.coords))
        break;

      openList.Remove(currentNodeCoords);
      closedList.Add(currentNodeCoords);

      foreach(var neighbourOffset in allNeighbourOffsets) {
        int2 neighbourCoords = new int2(currentNodeCoords.x + neighbourOffset.x, currentNodeCoords.y + neighbourOffset.y);

        if(!IsPosInGrid(neighbourCoords, gridSize))
          continue;
        
        if(closedList.Contains(neighbourCoords))
          continue;
        
        PathNode neighbourNode = grid[neighbourCoords.x, neighbourCoords.y];
        if(neighbourNode.isObstructed || !neighbourNode.isWalkable)
          continue;

        int tentativeGCost = currentNode.gCost + CalcDistanceCost(currentNodeCoords, neighbourCoords);
        if(tentativeGCost < neighbourNode.gCost) {
          neighbourNode.prevNodeCoords = currentNodeCoords;
          neighbourNode.gCost = tentativeGCost;
          neighbourNode.CalcFCost();
          grid[neighbourCoords.x, neighbourCoords.y] = neighbourNode;
          
          if(!openList.Contains(neighbourCoords))
            openList.Add(neighbourCoords);
        }
      }
    }


    endNode = grid[endPos.x, endPos.y];
    return RetracePath(grid, endNode);
  }

  

  private List<int2> RetracePath(PathNode[,] nodeGrid, PathNode endNode) {
    if(endNode.prevNodeCoords.Equals(new int2(-1))) { // no path found
      return new List<int2>(); // empty list
    } else { // path found
      // retrace the steps and add the nodes to a list
      List<int2> path = new List<int2>();
      path.Add(endNode.coords);

      int a = 0;

      PathNode currentNode = endNode;
      while(!currentNode.prevNodeCoords.Equals(new int2(-1))) {
        PathNode prevNode = nodeGrid[currentNode.prevNodeCoords.x, currentNode.prevNodeCoords.y];
        path.Add(prevNode.coords);
        currentNode = prevNode;
        a++;
      }

      path.Reverse();

      return path;
    }
  }

  
  /// <summary>
  /// Calculate the heuristic cost between two points
  /// by not considering which nodes are walkable.
  /// </summary>
  /// <param name="aPos">Start coords</param>
  /// <param name="bPos">End coords</param>
  /// <returns>Heuristic Distance (hCost)</returns>
  private int CalcDistanceCost(int2 aPos, int2 bPos) {
    int xDist = math.abs(aPos.x - bPos.x);
    int yDist = math.abs(aPos.y - bPos.y);
    int remainingStraightMovement = math.abs(xDist - yDist);
    
    return
      DIAGONAL_COST * math.min(xDist, yDist) +
      ORTHOGONAL_COST * remainingStraightMovement;
  }

  
  private int2 GetLowestFCostNodeIndex(List<int2> openList, PathNode[,] nodeGrid) {
    PathNode lowestCostNode = nodeGrid[openList[0].x, openList[0].y];
    
    for(int i = 0; i < openList.Count; i++) {
      PathNode testNode = nodeGrid[openList[i].x, openList[i].y];
      if(testNode.fCost < lowestCostNode.fCost)
        lowestCostNode = testNode;
    }

    return lowestCostNode.coords;
  }
  
  private bool IsPosInGrid(int2 gridPos, int2 gridSize) =>
    gridPos.x >= 0 &&
    gridPos.y >= 0 &&
    gridPos.x < gridSize.x &&
    gridPos.y < gridSize.y;



  public struct PathNode {
    public int2 coords;
    public int2 prevNodeCoords;

    public int gCost; // known cost based on already traversed path
    public int hCost; // heuristic cost based on distance between this node and the end node
    public int fCost; // gCost + hCost

    public bool isWalkable;
    public bool isObstructed;

    public void CalcFCost() {
      fCost = gCost + hCost;
    }

    public override string ToString() => $"Node: {coords}, Prev: {prevNodeCoords}";
  }
}