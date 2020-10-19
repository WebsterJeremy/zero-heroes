/* ObjectBase.cs
* 
* Author:  Nicholas Ruotsalainen RUOT0003
* Created: 17/08/2020
* 
* Last Edited By: Nicholas Ruotsalainen RUOT0003
* Last Updated:   18/08/2020
*/


using UnityEngine;

//namespace Assets.Scripts.world
namespace Assets.User_Test_Folders.BuildingTestFolder.scripts.WorldGen
{
    public abstract class ObjectBase
    {
        //the unique object id, and definition id
        protected string id, definitionId;
        //the game world representation of the object..
        protected GameObject gameObject;

        //position of the object, we tend not to rely on the gameObject's "gameObject.transform.position" as the gameobject may be null...
        protected Position position;
        protected ObjectData objData;


        public ObjectBase(string _id, string _definitionId, Position _position, ObjectData _objData)
        {
            this.id = _id;
            this.definitionId = _definitionId;
            this.position = _position;
            this.objData = _objData;

            Spawn();
        }

        public string Id
        {
            get { return id; }
        }
        public string DefinitionId
        {
            get { return definitionId; }
        }
        public GameObject GameObject
        {
            get { return gameObject; }
        }
        public Position Position
        {
            get { return position; }
        }

        public ObjectData ObjectData
        {
            get { return objData; }
        }

        public void SetPosition(Position _position, bool _updateGameObject)
        {
            this.position = _position;

            if (_updateGameObject && gameObject != null)
            {
                gameObject.transform.position = this.position.ToVector();
            }
        }

        public abstract GameObject Spawn();
        public abstract void Destroy();
        public abstract void OnInteraction();
    }
}