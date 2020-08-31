/* EntityMovementHelper.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 18/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   1/09/2020
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
                StopFollowingCurrentPath();
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


                if (path.Length > 0) {
                    goalPosition = path[path.Length - 1];
                }

                StopFollowingCurrentPath();
                lastMoveRoutine = GameController.Instance.StartChildCoroutine(FollowPath());
            } else {
                Debug.Log("path not successful... movement helper");
            }
        }

        public static float DEFAULT_WALK_SPEED = 6.0f;//todo move this elsewhere.. its referenced below.


        IEnumerator FollowPath() {
            isMoving = true;

            if (path.Length > 0) {
                goalPosition = path[path.Length - 1];

                Position currentWayPoint = path[0];

                while (isMoving) {
                    if (owner.Position().Equals(currentWayPoint)) {
                        targetIndex++;

                        if (targetIndex < path.Length) {
                            currentWayPoint = path[targetIndex];
                        }

                        if (targetIndex >= path.Length) {
                            isMoving = false;
                            yield break;
                        }
                    }

                    Vector2 targetPositionInWorld = currentWayPoint.ToVector();

                    do {
                        if (owner.GameObject != null) {
                            //todo should walk speed be specified elsewhere? in each entity type definition data perhaps? (if each entity can move at different speeds)
                            owner.GameObject.transform.position = Vector3.MoveTowards(owner.GameObject.transform.position, targetPositionInWorld, DEFAULT_WALK_SPEED * Time.deltaTime);
                            yield return new WaitForEndOfFrame();

                            if (owner.GameObject.transform.position.x.Equals(targetPositionInWorld.x) &&
                                owner.GameObject.transform.position.y.Equals(targetPositionInWorld.y)) {
                                owner.UpdatePosition(currentWayPoint, false);
                            }
                        } else {
                            yield break;
                        }

                        yield return new WaitForEndOfFrame();
                    } while (!owner.Position().Equals(currentWayPoint));


                    yield return new WaitForEndOfFrame();
                }
            }
        }

        void StopFollowingCurrentPath() {
            if (lastMoveRoutine != null) {
                GameController.Instance.StopChildCoroutine(lastMoveRoutine);
            }
            lastMoveRoutine = null;
            //    lastLerpRoutine = null;

            //set closest position
            owner.UpdatePosition(new Position(Mathf.FloorToInt(owner.GameObject.transform.position.x), Mathf.FloorToInt(owner.GameObject.transform.position.y)), false);

            goalPosition = null;

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
