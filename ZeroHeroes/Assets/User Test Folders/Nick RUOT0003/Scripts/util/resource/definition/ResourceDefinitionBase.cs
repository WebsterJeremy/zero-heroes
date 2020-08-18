/* ResourceDefinitionBase.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */
 
 using UnityEngine;

namespace Assets.Scripts.util.resource.definition
{
    public abstract class ResourceDefinitionBase
    {
        private string id, name, assetName, description;

        private Sprite[] sprites;
        

        public ResourceDefinitionBase(string _id, string _name, string _assetName, string _description, Sprite[] _sprites) {
            this.id = _id;
            this.name = _name;
            this.assetName = _assetName;
            this.description = _description;
            this.sprites = _sprites;
        }

        public string Id {
            get { return id; }
        }
        public string AssetName {
            get { return assetName; }
        }


        public string Description {
            get { return description; }
        }

        /// <summary>
        /// returns true if the object is animatable
        /// </summary>
        public bool IsAnimatable {
            //this is required for the custom sprite animator...
            get { return sprites.Length > 1; }
        }

        public Sprite Sprite {
            get { return sprites[0]; }
        }
        
        /// <summary>
        /// returns the array of sprites
        /// </summary>
        public Sprite[] Sprites {
            get { return sprites; }
        }
    }
}
