using Assets.Scripts.ai;
using Assets.Scripts.ai.state;
using Assets.Scripts.Gameplay;
using UnityEngine;

namespace Assets.Scripts.world
{
    public class Entity
    {
        private string id;
        private Position position;
        private GameObject gameObject;
        public float walkSpeed = 6f;

        //an entity is a moving or living object... eg npc, etc... 
        //this will utilize, pathfinding, but also utilize the FMS (finite state machine) system (which the player does not use)

        //this is the finite state machine (brain) of an entity..
        //it controls the states the ai is in and determines its behaviour based on the environment
        protected FSM brain;

        //this is the sight helper for the entity.
        //it tells the brain what it can see, to help it to determine the next behaviour
     //   protected EntitySightHelper sightHelper;

        //this is the movement helper for the entity.
        //it allows the entity to move
        protected EntityMovementHelper movementHelper;
        public Coroutine lastAction;


        protected Inventory inventory;

        public Inventory Inventory {
            get { return inventory; }
        }

        public float WalkSpeed
        {
            get { return walkSpeed; }
        }


        public Entity(string _id, string _definitionId, Position _position) {
            this.id = _id;
            this.position = _position;
           // this.sightHelper = new EntitySightHelper(this);
            this.movementHelper = new EntityMovementHelper(this);

            inventory = new Inventory();

            this.brain = new FSM(this, new FSMStateIdle(this));
            brain.OnInitialized();
        }

        public EntityMovementHelper MovementHelper {
            get { return movementHelper; }
        }


        public FSM FSM {
            get { return brain; }
        }

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

/*
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
*/
    }
}
