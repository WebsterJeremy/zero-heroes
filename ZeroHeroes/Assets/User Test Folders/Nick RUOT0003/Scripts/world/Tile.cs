/* Tile.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.ai.path;
using Assets.Scripts.util.resource.definition;
using UnityEngine;

namespace Assets.Scripts.world
{
    public class Tile : ObjectBase, IHeapItem<Tile>
    {
        private int hCost_;
        private int gCost_;
        private string parentId;

        public Tile(string _id, string _definitionId, Position _position) : base(_id, _definitionId, _position) {
            //intentionally left empty
        }

        public TileDefinition Definition {
            get { return GameController.instance.ResourceManager.TileResourceManager.GetFromId(definitionId); }
        }

        public override void Destroy() {
            if (gameObject != null) {
                UnityEngine.GameObject.Destroy(gameObject);
            }

            GameController.instance.World.RemoveTileFromList(id);
        }

        public override void Spawn() {
            gameObject = GameObject.Instantiate(GameController.instance.tileTemplate);

            //set sprite
            gameObject.GetComponent<SpriteRenderer>().sprite = Definition.Sprite;

            //add animation component if animatable
            if (Definition.IsAnimatable) {
                gameObject.AddComponent<SpriteAnimator>().Initialize(Definition.Sprites);
            }

            //set other data
            gameObject.transform.position = position.ToVector();
            gameObject.name = id;
            gameObject.transform.SetParent(GameController.instance.tileContainer, false);
        }


        #region Pathfinding Waypoint Tile

        public Tile Parent {
            get { return GameController.instance.World.GetTileFromId(parentId); }
            set { parentId = value.id; }
        }

        public int fCost {
            get { return gCost_ + hCost_; }
        }

        public int hCost {
            get { return hCost_; }
            set { hCost_ = value; }
        }

        public int gCost {
            get { return gCost_; }
            set { gCost_ = value; }
        }

        int heapIndex;

        public int HeapIndex {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(Tile tile) {
            int compare = fCost.CompareTo(tile.fCost);
            if (compare == 0) {
                compare = hCost.CompareTo(tile.hCost);
            }

            return -compare;
        }
        #endregion
    }
}
