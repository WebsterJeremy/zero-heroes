/* ItemResourceManager.cs
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
    public class ItemResourceManager
    {
        private Dictionary<string, ItemDefinition> cachedItems = new Dictionary<string, ItemDefinition>();

        public ItemResourceManager() {
            //intentionally left empty
        }

        /// <summary>
        /// loads all item definitions from file into memory.
        /// </summary>
        public bool Load() {
            //read all item definitions from file, find the sprites associated, and dump into memory.

            //get file from resources
            TextAsset ta = Resources.Load(System.IO.Path.Combine(Constants.ResourceNames.ITEMS, Constants.ResourceNames.FILE_DEFINITIONS)) as TextAsset;

            if (ta == null) {
                Debug.LogError("Item Resource Manager: Unable to load items; cannot find definitions file!");
                return false;
            }


            //load contents
            XDocument data = XDocument.Parse(ta.text);


            if (data == null) {
                Debug.LogError("Item Resource Manager: Unable to load items; empty or corrupt definitions file!");
                return false;
            }

            //iterate data and push to list
            foreach (XElement item in data.Element(Constants.XMLHeaders.ITEMS).Elements()) {
                string id, name, assetName, description;
                bool stackable, questItem;

                id = item.Element(Constants.XMLHeaders.ID).Value;
                assetName = item.Element(Constants.XMLHeaders.ASSET_NAME).Value;
                name = item.Element(Constants.XMLHeaders.NAME).Value;
                description = item.Element(Constants.XMLHeaders.DESCRIPTION).Value;
                stackable = (bool)item.Element(Constants.XMLHeaders.STACKABLE).Value.Equals(Constants.XMLHeaders.TRUE) ? true : false;
                questItem = (bool)item.Element(Constants.XMLHeaders.QUEST_ITEM).Value.Equals(Constants.XMLHeaders.TRUE) ? true : false;

                //todo possibly buy and sell value?

                //get the names of the sprites..
                //in directory of project, if there is more than 1 sprite (eg.. for an animation), then,
                //the sprite images would be located under a directory with that asset name 
                Sprite[] sprites = new Sprite[item.Element(Constants.XMLHeaders.SPRITES).Elements().Count()];

                if (sprites.Length > 1) {
                    //then it must animate!
                    int index = 0;

                    foreach (XElement sprite in item.Element(Constants.XMLHeaders.SPRITES).Elements()) {
                        string path = System.IO.Path.Combine(Constants.ResourceNames.ITEMS, assetName, sprite.Value);
                        Sprite foundSpriteAsset = Resources.Load<Sprite>(path) as Sprite;

                        if (foundSpriteAsset == null) {
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
                    string path = System.IO.Path.Combine(Constants.ResourceNames.ITEMS,
                        item.Element(Constants.XMLHeaders.SPRITES).Element(Constants.XMLHeaders.SPRITE).Value);
                    Sprite foundSpriteAsset = Resources.Load<Sprite>(path) as Sprite;

                    if (foundSpriteAsset == null) {
                        //set as missing sprite image
                        sprites[0] = GameController.instance.MISSING_SPRITE;
                    } else {
                        sprites[0] = foundSpriteAsset;
                    }
                }

                //finally add to the cached items
                cachedItems.Add(id, new ItemDefinition(id, name, assetName, description, sprites, stackable, questItem));

            }

            return true;
        }
        /// <summary>
        /// returns the item defintion of the given id, if it exists.
        /// </summary>
        public ItemDefinition GetFromId(string _id) {
            if(!cachedItems.ContainsKey(_id)) {
                return null; 
            }

            return cachedItems[_id];
        }

        /// <summary>
        /// returns the item defintion of the given asset name, if it exists.
        /// </summary>
        public ItemDefinition GetFromAssetName(string _assetName) {
            foreach (ItemDefinition id in cachedItems.Values) {
                if (id.AssetName.Equals(_assetName)) {
                    return id;
                }
            }

            return null;
        }

        public int Count {
            get { return cachedItems.Count; }
        }
    }
}
