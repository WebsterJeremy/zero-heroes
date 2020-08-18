/* PathRequestManager.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 18/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.world;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ai.path
{
    public class PathRequestManager
    {
        Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        PathRequest currentPathRequest;
        bool isProcessingPath;

        struct PathRequest
        {
            public Position start;
            public Position end;
            public Action<Position[], bool> callback;

            public PathRequest(Position _start, Position _end, Action<Position[], bool> _callback) {
                start = _start;
                end = _end;
                callback = _callback;
            }
        }

        public void RequestPath(Entity _entity, Position _end, Action<Position[], bool> _callback) {
            PathRequest newRequest = new PathRequest(_entity.Position, _end, _callback);
            pathRequestQueue.Enqueue(newRequest);
            TryProcessNext();
        }

        public void FinishedProcessingPath(Position[] _path, bool _success) {
            if (_path != null) {
                currentPathRequest.callback(_path, _success);
                isProcessingPath = false;
                TryProcessNext();
            }
        }

        void TryProcessNext() {
            if (!isProcessingPath && pathRequestQueue.Count > 0) {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                GameController.instance.PathFinder.StartFindPath(currentPathRequest.start, currentPathRequest.end);
            }
        }


    }
}
