/* ObjectResourceManager.cs
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
    public class ObjectResourceManager
    {
        private Dictionary<string, ObjectDefinition> cachedObjects = new Dictionary<string, ObjectDefinition>();

        public ObjectResourceManager() {
            //intentionally left empty
        }

        /// <summary>
        /// loads all objects definitions from file into memory.
        /// </summary>
        public bool Load() {
            //read all objects definitions from file, find the sprites associated, and dump into memory.

            //get file from resources
            TextAsset ta = Resources.Load(System.IO.Path.Combine(Constants.ResourceNames.OBJECTS, Constants.ResourceNames.FILE_DEFINITIONS)) as TextAsset;

            if (ta == null) {
                Debug.LogError("Object Resource Manager: Unable to load objects; cannot find definitions file!");
                return false;
            }


            //load contents
            XDocument data = XDocument.Parse(ta.text);


            if (data == null) {
                Debug.LogError("Object Resource Manager: Unable to load objects; empty or corrupt definitions file!");
                return false;
            }

            //iterate data and push to list
            foreach (XElement obj in data.Element(Constants.XMLHeaders.OBJECTS).Elements()) {
                string id, name, assetName, description;

                id = obj.Element(Constants.XMLHeaders.ID).Value;
                assetName = obj.Element(Constants.XMLHeaders.ASSET_NAME).Value;
                name = obj.Element(Constants.XMLHeaders.NAME).Value;
                description = obj.Element(Constants.XMLHeaders.DESCRIPTION).Value;

                //todo bounding box generation! this will prevent player from moving through the object... for now it is ignored...

                //get the names of the sprites..
                //in directory of project, if there is more than 1 sprite (eg.. for an animation), then,
                //the sprite images would be located under a directory with that asset name 
                Sprite[] sprites = new Sprite[obj.Element(Constants.XMLHeaders.SPRITES).Elements().Count()];

                if (sprites.Length > 1) {
                    //then it must animate!
                    int index = 0;

                    foreach (XElement sprite in obj.Element(Constants.XMLHeaders.SPRITES).Elements()) {
                        string path = System.IO.Path.Combine(Constants.ResourceNames.OBJECTS, assetName, sprite.Value);
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
                    string path = System.IO.Path.Combine(Constants.ResourceNames.OBJECTS,
                        obj.Element(Constants.XMLHeaders.SPRITES).Element(Constants.XMLHeaders.SPRITE).Value);
                    Sprite foundSpriteAsset = Resources.Load<Sprite>(path) as Sprite;

                    if (foundSpriteAsset == null) {
                        //set as missing sprite image
                        sprites[0] = GameController.instance.MISSING_SPRITE;
                    } else {
                        sprites[0] = foundSpriteAsset;
                    }
                }

                //finally add to the cached objects
                cachedObjects.Add(id, new ObjectDefinition(id, name, assetName, description, sprites));
            }

            return true;
        }

        /// <summary>
        /// returns the object defintion of the given id, if it exists.
        /// </summary>
        public ObjectDefinition GetFromId(string _id) {
            if (!cachedObjects.ContainsKey(_id)) {
                return null;
            }

            return cachedObjects[_id];
        }

        /// <summary>
        /// returns the object defintion of the given asset name, if it exists.
        /// </summary>
        public ObjectDefinition GetFromAssetName(string _assetName) {
            foreach (ObjectDefinition od in cachedObjects.Values) {
                if (od.AssetName.Equals(_assetName)) {
                    return od;
                }
            }

            return null;
        }

        public int Count {
            get { return cachedObjects.Count; }
        }
    }
}