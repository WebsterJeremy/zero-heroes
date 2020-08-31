using Assets.Scripts.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class CustomItem
    {
        //the unique object id, and definition id
        private string id, definition;
        //the game world representation of the object..
        private GameObject gameObject;
        private string parentId;

        //position of the object, we tend not to rely on the gameObject's "gameObject.transform.position" as the gameobject may be null...
        private Position position;
        private int amount;

        public CustomItem(string _id, string _definition, string _parentTileId, int _amount) {
            this.id = _id;
            this.definition = _definition;
            this.parentId = _parentTileId;
            this.amount = _amount;
            Spawn();//finally spawn it...
        }
        public CustomItem(string _id, string _definitionId, string _parentTileId) {
            this.id = _id;
            this.definition = _definitionId;
            this.parentId = _parentTileId;
            this.amount = 1;
            Spawn();//finally spawn it...
        }

        //todo this definition needs to be the scriptable object so we can get reference to the sprite!
        // public ItemDefinition Definition {
        //   get { return GameController.instance.ResourceManager.ItemResourceManager.GetFromId(definitionId); }
        // }

        public Tile ParentTile {
            get { return GameController.Instance.World.GetTileFromId(parentId); }
        }

        public int Amount {
            get { return amount; }
        }

        public void Pickup(Entity _entity) {
            if (_entity.Inventory == null) {
                return; //entity may not have an inventory...
            }

            if (!_entity.Inventory.IsInventoryFull()) {
                //todo add to inventory here...

                //destroy world item..
                Destroy();
            }
        }

        public void Destroy() {
            ParentTile.DestroyChildItem(id);
        }

        protected void Spawn() {
            gameObject = GameObject.Instantiate(GameController.Instance.itemPrefabTemplate);

            //set sprite
            //todo set sprite!
            //if we cant find the sprite resource, then BAIL!... not worth the time. sorry.( ͡° ͜ʖ ͡°) 
            MapTile mt = UnityEngine.Resources.Load<MapTile>(System.IO.Path.Combine("Tiles/Objects", definition));//todo.. it may not be located here!! we need a defined place to store !!!!ITEMS!!!

            if(mt == null) {
                Destroy();
                return;
            }

            //if reached here then scriptable object, map tile was found... NOW get sprite..
            if (mt.sprite == null) {
                Destroy();
                return;
            }

            gameObject.GetComponent<SpriteRenderer>().sprite = mt.sprite;

            //set other data
            gameObject.transform.position = Position().ToVector();
            gameObject.name = id;
            gameObject.transform.SetParent(GameController.Instance.ItemContainer, false);
        }


        #region Getters
        public string Id {
            get { return id; }
        }

        public string ParentId {
            get { return parentId; }
        }

        public string LocalisedName {
            get { return definition; }//TODO currently just returning the definition id...  eg buckets_3 rather than Bucket... because we need Map Tile scriptable object to contain this var!!!!! speak with nick when ready.
        }

        public string Definition {
            get { return definition; }
        }
        public GameObject GameObject {
            get { return gameObject; }
        }

        public virtual Position Position() {
            if (!string.IsNullOrWhiteSpace(parentId)) {
                //get parent position
                Tile p = GameController.Instance.World.GetTileFromId(parentId);
                if (p != null) {
                    return p.Position();
                }
            } else {
                return position;

            }

            return new Position(0, 0);
        }

        public void UpdatePosition(Position _position, bool _updateGameObject) {
            this.position = _position;

            if (_updateGameObject && gameObject != null) {
                gameObject.transform.position = this.position.ToVector();
            }
        }

        #endregion
    }
}
