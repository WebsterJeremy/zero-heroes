using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class Inventory
    {
        public Dictionary<string, InventoryItem> items = new Dictionary<string, InventoryItem>();
        
        public void Add(string _assetName, int _amount) {
            Debug.Log("adding item to inventory....");
            //right now ill just add each one as  a new item.. but see below todo comment...

            //if stackable...
            InventoryItem ii = new InventoryItem(_assetName, _amount);
            items.Add(ii.Id, ii);

            //else
            //for (int i = 0; i < _amount; i++) {
            // }

            /*todo.. may need some extra logic here if the item is stackable, etc.. this would all be determined from MapTile scriptable class!!! we need a bool check ... eg is farming item stackable.. maybe, but a bucket isnt!
                then if it is stackable.. to what amount? has the max stack been reached? or should we add a new slot? 

                Jeremy -> pls update MapTile.
                Thanks,NR
            */


            //todo ill optimize this another day. it currently iterates every item... not worth it.
            UIController.Instance.UpdateInventory(items.Values.ToList());
        }

        public void Remove(string _id) {
            if (!items.ContainsKey(_id)) {
                return;
            }

            items.Remove(_id);

            //todo ill optimize this another day. it currently iterates every item... not worth it.
            UIController.Instance.UpdateInventory(items.Values.ToList());
        }

        public bool IsInventoryFull() {
            return false;//todo...check...
        }

        public List<InventoryItem> Items() {
            return items.Values.ToList();
        }

        public InventoryItem GetInventorytemFromId(string _id) {
            if (!items.ContainsKey(_id)) {
                return null;
            }

            return items[_id];
        }

       
    }
}
