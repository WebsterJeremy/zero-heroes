/* Object.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.world;
using UnityEngine;

namespace Assets.Scripts.world
{
    public class Object : ObjectBase
    {
        public Object(string _id, string _definitionId, Position _position, ObjectData objectData) : base(_id, _definitionId, _position, objectData) {
            //intentionally left empty
        }

        public override void Destroy() {
            GameController.Instance.World.Remove(this);

            if (GameObject != null)
            {
                UnityEngine.GameObject.Destroy(GameObject);
            }
        }

        public override GameObject Spawn() {
            gameObject = GameObject.Instantiate(GameController.Instance.objectTemplate);

            //set sprite
            gameObject.GetComponent<SpriteRenderer>().sprite = ObjectData.sprite; //Definition.Sprite;
            gameObject.GetComponent<SpriteRenderer>().color = ObjectData.color;

            //add animation component if animatable
            if (ObjectData.IsAnimatable) {
                gameObject.AddComponent<SpriteAnimator>().Initialize(ObjectData.sprites);//Definition.Sprites);
            }

            //set other data
            gameObject.transform.position = position.ToVector();
            gameObject.name = id;
            gameObject.transform.SetParent(GameController.Instance.objectContainer, false);

            return gameObject;
        }

        public override void OnInteraction()
        {
            objData.OnInteraction();
            Destroy();
        }
    }
}