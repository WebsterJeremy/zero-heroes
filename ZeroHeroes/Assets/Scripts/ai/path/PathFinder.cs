/* PathFinder.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 18/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.world;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ai.path
{
    public class PathFinder
    {
        IEnumerator FindPath(Position start, Position end) {
            Position[] waypoints = new Position[0];
            bool pathSuccess = false;

            Tile entityTile = null;
            Tile targetTile = null;

            if (start != null && end != null) {
                entityTile = GameController.Instance.World.GetTileFromPosition(start);
                targetTile = GameController.Instance.World.GetTileFromPosition(end);

                if (targetTile != null /*&& !targetTile.IsOccupied*/ &&

                    GameController.Instance.World.IsTileTraversable(end)) {
                    Heap<Tile> openSet = new Heap<Tile>(GameController.Instance.World.MaxSize);
                    HashSet<Tile> closedSet = new HashSet<Tile>();

                    openSet.Add(entityTile);

                    while (openSet.Count > 0) {
                        Tile currentTile = openSet.RemoveFirst();
                        closedSet.Add(currentTile);

                        if (currentTile == targetTile) {
                            pathSuccess = true;
                            break;
                        }

                        foreach (Tile neighbour in GameController.Instance.World.GetTileNeighbours(currentTile)) {
                            if (!GameController.Instance.World.IsTileTraversable(neighbour.Position()) ||
                                /*neighbour.IsOccupied ||*/
                                closedSet.Contains(neighbour)) {
                                continue;
                            }

                            int newMovementCostToNeighbour = currentTile.gCost + GameController.Instance.World.GetDistance(currentTile, neighbour);
                            if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                                neighbour.gCost = newMovementCostToNeighbour;
                                neighbour.hCost = GameController.Instance.World.GetDistance(neighbour, targetTile);
                                neighbour.Parent = currentTile;

                                if (!openSet.Contains(neighbour)) {
                                    openSet.Add(neighbour);
                                } else {
                                    openSet.UpdateItem(neighbour);
                                }
                            }
                        }
                    }


                    yield return null;

                    if (pathSuccess) {
                        Position[] points = RetracePath(entityTile, targetTile);
                        waypoints = points;
                    } else {
                        Debug.Log("path not successful: " + start.ToString() + "..." + end.ToString());
                    }

                    GameController.Instance.PathRequestManager.FinishedProcessingPath(waypoints, pathSuccess);
                }
            }

        }

        public void StartFindPath(Position start, Position end) {
            GameController.Instance.StartChildCoroutine(FindPath(start, end));
        }

        Position[] RetracePath(Tile startTile, Tile endTile) {
            List<Position> path = new List<Position>();
            Tile currentTile = endTile;

            while (currentTile != startTile) {
                path.Add(currentTile.Position());
                currentTile = currentTile.Parent;
            }

            Position[] wayPoints = new Position[0];

            if (path != null && path.Count > 0) {
                wayPoints = SimplifyPath(path);
                Array.Reverse(wayPoints);
            }

            return wayPoints;
        }

        Position[] SimplifyPath(List<Position> path) {
            List<Position> wayPoints = new List<Position>();
            Position directionOld = new Position(0, 0);

            wayPoints.Add(path[0]);

            for (int i = 1; i < path.Count; i++) {
                Position directionNew = new Position(path[i - 1].X - path[i].X, path[i - 1].Y - path[i].Y);
                if (directionNew != directionOld) {
                    wayPoints.Add(path[i]);
                }

                directionOld = directionNew;
            }

            return wayPoints.ToArray();
        }

    }
}
