﻿/* EntityMovementHelper.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 18/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   24/08/2020
 */

using Assets.Scripts.world;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ai
{
    public class EntityMovementHelper
    {
        Entity owner;
        Position[] path;
        int targetIndex;
        bool isMoving = false;
        public Coroutine lastMoveRoutine = null;
//        public Coroutine lastLerpRoutine = null;
        Position goalPosition;


        public EntityMovementHelper(Entity _owner) {
            this.owner = _owner;
        }

        public void MoveTo(Position _position) {
            if (goalPosition != null && goalPosition.Equals(_position)) {
                //goal is already set
                return;
            }

            if (GameController.Instance.World.IsTileTraversable(_position) /*&&
                !WorldController.instance.Grid.IsTileOccupied(newPos)*/) {
                StopFollowingPath();
                GameController.Instance.PathRequestManager.RequestPath(owner, _position, OnPathFound);
            } else {
                Debug.Log("tile is not traversable");
            }
        }

        #region PathFinding
        public Position GoalPosition {
            get { return goalPosition; }
        }
        public void OnPathFound(Position[] newPath, bool _pathSuccessful) {
            if (_pathSuccessful) {
                path = newPath;
                targetIndex = 0;

                if (lastMoveRoutine != null) {
                    StopFollowingPath();
                }
                
                AttemptFollowPath();
            } else {
                Debug.Log("path not successful... movement helper");
            }
        }

        void CheckIfAtGoalPosition() {
            if (owner.Position().Equals(goalPosition)) {
                goalPosition = null;
            }
        }

        void AttemptFollowPath() {
            StopFollowingPath();
            lastMoveRoutine = GameController.Instance.StartChildCoroutine(FollowPath());
        }

        public static float DEFAULT_WALK_SPEED = 3.0f;//todo move this elsewhere.. its referenced below.


        IEnumerator FollowPath() {
            isMoving = true;

            if (path.Length > 0) {
                goalPosition = path[path.Length - 1];

                Position currentWayPoint = path[0];

              //  lastLerpRoutine = GameController.instance.StartChildCoroutine(LerpToPosition(currentWayPoint));

                while (isMoving) {
                    if (owner.Position().Equals(currentWayPoint)) {
                        targetIndex++;

                        if (targetIndex < path.Length) {
                            currentWayPoint = path[targetIndex];
                        }

                        if (targetIndex >= path.Length) {
                            CheckIfAtGoalPosition();
                            isMoving = false;
                            yield break;
                        }
                    }


                    Vector2 targetPositionInWorld = currentWayPoint.ToVector();

                    do {
                        if (owner.GameObject != null) {
                            owner.GameObject.transform.position = Vector3.MoveTowards(owner.GameObject.transform.position, targetPositionInWorld, DEFAULT_WALK_SPEED * Time.deltaTime);
                            //     yield return null;

                            if (owner.GameObject.transform.position.x.Equals(targetPositionInWorld.x) &&
                                owner.GameObject.transform.position.y.Equals(targetPositionInWorld.y)) {
                                owner.UpdatePosition(currentWayPoint, false);
                            }
                        } else {
                            yield break;
                        }
                        yield return null;

                    } while (!owner.Position().Equals(currentWayPoint));



                    yield return null;
                }
            }
        }


        void StopFollowingPath() {
            if (lastMoveRoutine != null) {
                GameController.Instance.StopChildCoroutine(lastMoveRoutine);
            }
                lastMoveRoutine = null;
            //    lastLerpRoutine = null;

                //set closest position
                owner.UpdatePosition(new Position(Mathf.FloorToInt(owner.GameObject.transform.position.x), Mathf.FloorToInt(owner.GameObject.transform.position.y)), false);
                Debug.Log("stopping player, current Pos; " + owner.Position().ToString());

                isMoving = false;
        }


        public int TargetIndex {
            get { return targetIndex; }
            set { targetIndex = value; }
        }

        public Position[] Path {
            get { return path; }
        }


        public bool IsMoving {
            get { return isMoving; }
            set { isMoving = value; }
        }
        #endregion
    }
}
