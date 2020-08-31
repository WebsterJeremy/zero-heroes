using Assets.Scripts.ai;
using UnityEngine;

namespace Assets.Scripts.world
{
    public class Entity
    {
        private string id;
        private Position position;
        private GameObject gameObject;


        public string Id {
            get { return id; }
        }
        public GameObject GameObject {
            get { return gameObject; }
            set { gameObject = value; }
        }
        //tood initalize

        public Position Position() {
           return position; 
        }

        public void UpdatePosition(Position _position, bool _updateGameObject) {
            this.position = _position;

            if (_updateGameObject && gameObject != null) {
                gameObject.transform.position = this.position.ToVector();
            }
        }

        //this is the movement helper for the entity.
        //it allows the entity to move
        protected EntityMovementHelper movementHelper;
        public Coroutine lastAction;



        public Entity(string _id, Position _position){
            this.id = _id;
            this.movementHelper = new EntityMovementHelper(this);

            // An entity could be considered anything not just something that moves, such as an Dropped Item or Objective Item on the floor or a Door
            //todo initialize the fsm (brain) and add callbacks to the game controller
        }


        public void MoveTo(Position _position) {
            StopLastAction();
            movementHelper.MoveTo(_position);
        }


        private void StopLastAction() {
            Debug.Log("stopping last action!");
            if (lastAction != null) {
                GameController.Instance.StopChildCoroutine(lastAction);
                lastAction = null;
            }
        }

    }
}
