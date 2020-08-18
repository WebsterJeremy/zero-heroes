/* TileResourceManager.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.util.resource.definition;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts.util.resource
{
    public class TileResourceManager
    {
        private Dictionary<string, TileDefinition> cachedTiles = new Dictionary<string, TileDefinition>();

        public TileResourceManager() {
            //intentionally left empty
        }

        /// <summary>
        /// loads all tile definitions from file into memory.
        /// </summary>
        public bool Load() {
            //read all tile definitions from file, find the sprites associated, and dump into memory.

            //get file from resources
            TextAsset ta = Resources.Load(System.IO.Path.Combine(Constants.ResourceNames.TILES, Constants.ResourceNames.FILE_DEFINITIONS)) as TextAsset;

            if(ta == null) {
                Debug.LogError("Tile Resource Manager: Unable to load tiles; cannot find definitions file!");
                return false;
            }


            //load contents
            XDocument data = XDocument.Parse(ta.text);


            if(data == null) {
                Debug.LogError("Tile Resource Manager: Unable to load tiles; empty or corrupt definitions file!");
                return false;
            }

            //iterate data and push to list
            foreach(XElement tile in data.Element(Constants.XMLHeaders.TILES).Elements()) {
                string id, name, assetName, description;
                bool traversable;

                id = tile.Element(Constants.XMLHeaders.ID).Value;
                assetName = tile.Element(Constants.XMLHeaders.ASSET_NAME).Value;
                name = tile.Element(Constants.XMLHeaders.NAME).Value;
                description = tile.Element(Constants.XMLHeaders.DESCRIPTION).Value;
                traversable = (bool) tile.Element(Constants.XMLHeaders.TRAVERSABLE).Value.Equals(Constants.XMLHeaders.TRUE) ? true : false;

                //get the names of the sprites..
                //in directory of project, if there is more than 1 sprite (eg.. for an animation), then,
                //the sprite images would be located under a directory with that asset name 
                Sprite[] sprites = new Sprite[tile.Element(Constants.XMLHeaders.SPRITES).Elements().Count()];

                if (sprites.Length > 1) {
                    //then it must animate!
                    int index = 0;
                    
                    foreach (XElement sprite in tile.Element(Constants.XMLHeaders.SPRITES).Elements()) {
                        string path = System.IO.Path.Combine(Constants.ResourceNames.TILES, assetName, sprite.Value);

                        Debug.Log(path);
                        Sprite foundSpriteAsset = Resources.Load<Sprite>(path) as Sprite;

                        if(foundSpriteAsset == null) {
                            Debug.Log("sprite is missing");

                            //set as missing sprite image
                            sprites[index] = GameController.instance.MISSING_SPRITE;
                        } else {
                            sprites[index] = foundSpriteAsset;
                        }

                        index++;
                    }
                } else {
                    //single sprite image... not animated..
                    
                    //reason this needs to be in an else is that if its a single sprite and not an animated one it wont be in a folder (sub dir). see the difference in paths.
                    string path = System.IO.Path.Combine(Constants.ResourceNames.TILES,
                        tile.Element(Constants.XMLHeaders.SPRITES).Element(Constants.XMLHeaders.SPRITE).Value);


                    Sprite foundSpriteAsset = Resources.Load<Sprite>(path) as Sprite;

                    if(foundSpriteAsset == null) {
                        //set as missing sprite image
                        sprites[0] = GameController.instance.MISSING_SPRITE;
                    } else {
                        sprites[0] = foundSpriteAsset;
                    }
                }

                //finally add to the cached tiles
                cachedTiles.Add(id, new TileDefinition(id, name, assetName, description, sprites,  traversable));
            }

            return true;
        }

        /// <summary>
        /// returns the tile defintion of the given id, if it exists.
        /// </summary>
        public TileDefinition GetFromId(string _id) {
            if (!cachedTiles.ContainsKey(_id)) {
                return null;
            }

            return cachedTiles[_id];
        }

        /// <summary>
        /// returns the tile defintion of the given asset name, if it exists.
        /// </summary>
        public TileDefinition GetFromAssetName(string _assetName) {
            foreach(TileDefinition td in cachedTiles.Values) {
                if (td.AssetName.Equals(_assetName)) {
                    return td;
                }
            }

            return null;
        }


        public int Count {
            get { return cachedTiles.Count; }
        }
    }
}
