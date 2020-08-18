/* LivingEntity.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.util.resource.definition;
using Assets.Scripts.world;
using UnityEngine;

namespace Assets.Scripts
{
    public class LivingEntity : Entity
    {
        //an entity is a moving or living object... eg npc, etc... 
        //this will utilize inventory, pathfinding, like the player, but also utilize the FMS (finite state machine) system (which the player does not use)

        private bool questGiver = false;

        private Inventory inventory;

        public LivingEntity(string _id, string _definitionId, Position _position) : base(_id, _definitionId, _position) {
            inventory = new Inventory(id);
        }

        public EntityDefinition Definition {
            get { return GameController.instance.ResourceManager.EntityResourceManager.GetFromId(definitionId); }
        }


        public Inventory Inventory {
            get { return inventory; }
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