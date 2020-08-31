using Assets.Scripts.ai.path;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.world
{
    public class Tile : IHeapItem<Tile>
    {
        private Dictionary<string, CustomItem> spawnedItems = new Dictionary<string, CustomItem>();

        private string id;
        private Position position;
        private int hCost_;
        private int gCost_;
        private string parentId;
        private bool traverable;


        public string Id {
            get { return id; }
        }
        public Tile(string _id, Position _position, bool _traversable) {
            this.id = _id;
            this.position = _position;
            this.traverable = _traversable;
        }


        public bool IsTraversable {
            get { return traverable; }
        }

        #region Pathfinding Waypoint Tile
        
        public Tile Parent {
            get { return GameController.Instance.World.GetTileFromId(parentId); }
            set { parentId = value.id; }
        }

        public Position Position() {
           return position;
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


        public CustomItem SpawnChildItem(string itemTypeId, int amount) {
            CustomItem item = new CustomItem(Constants.GenerateUniqueId(), itemTypeId, id, amount);
            spawnedItems.Add(item.Id, item);
            return item;
        }

        public void DestroyChildItem(CustomItem _item) {
            if (_item.GameObject != null) {
                UnityEngine.GameObject.Destroy(_item.GameObject);
            }

            //remove from list
            spawnedItems.Remove(_item.Id);
        }
        
        public void DestroyChildItem(string _id) {
            if (!spawnedItems.ContainsKey(_id)) {
                return;
            }

            CustomItem _item = spawnedItems[_id];

            if (_item.GameObject != null) {
                UnityEngine.GameObject.Destroy(_item.GameObject);
            }

            //remove from list
            spawnedItems.Remove(_item.Id);
        }
        public List<CustomItem> GetChildItems {
            get { return spawnedItems.Values.ToList(); }
        }

        public CustomItem GetChildItem(string _id) {
            if (!spawnedItems.ContainsKey(_id)) {
                return null;
            }

            return spawnedItems[_id];
        }
    }
}
