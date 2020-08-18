/* Entity.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.ai;
using Assets.Scripts.util.resource.definition;
using UnityEngine;

namespace Assets.Scripts.world
{
    public class Entity : ObjectBase
    {
        //an entity is a moving or living object... eg npc, etc... 
        //this will utilize, pathfinding, but also utilize the FMS (finite state machine) system (which the player does not use)
       
        //this is the finite state machine (brain) of an entity..
        //it controls the states the ai is in and determines its behaviour based on the environment
        protected FSM brain;
        
        //this is the sight helper for the entity.
        //it tells the brain what it can see, to help it to determine the next behaviour
        protected EntitySightHelper sightHelper;

        //this is the movement helper for the entity.
        //it allows the entity to move
        protected EntityMovementHelper movementHelper;



        public Entity(string _id, string _definitionId, Position _position) : base(_id, _definitionId, _position) {
            this.brain = new FSM();
            this.sightHelper = new EntitySightHelper(this);
            this.movementHelper = new EntityMovementHelper(this);


            //todo initialize the fsm (brain) and add callbacks to the game controller
        }


        public void MoveTo(Position _position) {
            movementHelper.MoveTo(_position);
        
        }

        public EntityDefinition Definition {
            get { return GameController.instance.ResourceManager.EntityResourceManager.GetFromId(definitionId); }
        }

        public override void Destroy() {
            if (gameObject != null) {
                UnityEngine.GameObject.Destroy(gameObject);
            }

            GameController.instance.World.RemoveEntityFromList(id);
        }

        public override void Spawn() {
            gameObject = GameObject.Instantiate(GameController.instance.entityTemplate);

            //set sprite
            gameObject.GetComponent<SpriteRenderer>().sprite = Definition.Sprite;

            //add animation component if animatable
            if (Definition.IsAnimatable) {
                gameObject.AddComponent<SpriteAnimator>().Initialize(Definition.Sprites);
            }

            //set other data
            gameObject.transform.position = position.ToVector();
            gameObject.name = id;
            gameObject.transform.SetParent(GameController.instance.entityContainer, false);
        }
    }
}