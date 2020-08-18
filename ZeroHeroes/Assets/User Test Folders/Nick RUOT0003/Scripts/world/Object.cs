/* Object.cs
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

namespace Assets.Scripts.world
{
    public class Object : ObjectBase
    {
        public Object(string _id, string _definitionId, Position _position) : base(_id, _definitionId, _position) {
            //intentionally left empty
        }

        public ObjectDefinition Definition {
            get { return GameController.instance.ResourceManager.ObjectResourceManager.GetFromId(definitionId); }
        }

        public override void Destroy() {
            if (gameObject != null) {
                UnityEngine.GameObject.Destroy(gameObject);
            }

            GameController.instance.World.RemoveItemFromList(id);
        }

        public override void Spawn() {
            gameObject = GameObject.Instantiate(GameController.instance.objectTemplate);

            //set sprite
            gameObject.GetComponent<SpriteRenderer>().sprite = Definition.Sprite;

            //add animation component if animatable
            if (Definition.IsAnimatable) {
                gameObject.AddComponent<SpriteAnimator>().Initialize(Definition.Sprites);
            }

            //set other data
            gameObject.transform.position = position.ToVector();
            gameObject.name = id;
            gameObject.transform.SetParent(GameController.instance.objectContainer, false);
        }
    }
}