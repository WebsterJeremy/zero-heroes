using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class InventoryItem {

        private string id;//unique id is needed so we can do a call back in ui and get the right item..

        private int amount = 1;
        private string assetName;

        public InventoryItem(string _assetName) {
            this.id = Constants.GenerateUniqueId();
            this.assetName = _assetName;
            this.amount = 1;
        }

        public InventoryItem(string _assetName, int _amount) {
            this.id = Constants.GenerateUniqueId();
            this.assetName = _assetName;
            this.amount = _amount;//todo not sure if were doing stackable items.. but here it is.. just in case... assuming we are since we need to collect resources..e tc..
        }

        public string Id {
            get { return id; }
        }


        public string AssetName {
            get { return assetName; }
        }

        public int Amount {
            get { return amount; }
        }

        public string LocalisedName {
            get { return assetName; }//todo as mentioned in CustomItem class, this needs to be set up in MapTile scriptable obj to have the localized name set... eg buckets_3 = Buckets
        }


        public Sprite Sprite() {
            if (string.IsNullOrWhiteSpace(assetName)) {
                return null;
            }

            //if we cant find the sprite resource, then BAIL!... not worth the time. sorry.( ͡° ͜ʖ ͡°) 
            MapTile mt = UnityEngine.Resources.Load<MapTile>(System.IO.Path.Combine("Tiles/Objects", assetName));//todo.. it may not be located here!! we need a defined place to store !!!!ITEMS!!!

            if (mt == null) {
                return null;
            }

            //if reached here then scriptable object, map tile was found... NOW get sprite..

            return mt.sprite;
        }
    }
}
