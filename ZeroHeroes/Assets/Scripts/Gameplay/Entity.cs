using Assets.Scripts.ai;
using Assets.Scripts.ai.state;
using Assets.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.world
{
    public class Entity
    {
        protected string id;
        protected Position position;
        protected GameObject gameObject;


        //an entity is a moving or living object... eg npc, etc... 
        //this will utilize, pathfinding, but also utilize the FMS (finite state machine) system (which the player does not use)

        //this is the finite state machine (brain) of an entity..
        //it controls the states the ai is in and determines its behaviour based on the environment
        protected FSM brain;

        //this is the sight helper for the entity.
        //it tells the brain what it can see, to help it to determine the next behaviour
        // protected EntitySightHelper sightHelper;

        //this is the movement helper for the entity.
        //it allows the entity to move
        protected EntityMovementHelper movementHelper;
        public Coroutine lastAction;


        protected Inventory inventory;

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

        public Entity(string _id, Position _position){
            this.id = _id;


            //set up inventory..
            this.inventory = new Inventory();

           //todo... this.sightHelper = new EntitySightHelper(this);
            this.movementHelper = new EntityMovementHelper(this);
            
            //initialize the fsm (brain) and add callbacks to the game controller
            this.brain = new FSM(this, new FSMStateIdle(this));
            brain.OnInitialized();

        }


        public Inventory Inventory {
            get { return inventory; }
        }

        public EntityMovementHelper MovementHelper {
            get { return movementHelper; }
        }

        public FSM FSM {
            get { return brain; }
        }

    }
}
