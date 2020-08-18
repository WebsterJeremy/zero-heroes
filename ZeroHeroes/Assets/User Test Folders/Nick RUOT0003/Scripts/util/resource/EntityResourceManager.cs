/* EntityResourceManager.cs
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
    public class EntityResourceManager
    {
        private Dictionary<string, EntityDefinition> cachedEntities = new Dictionary<string, EntityDefinition>();


        public EntityResourceManager() {
            //intentionally left empty
        }


        /// <summary>
        /// loads all entity definitions from file into memory.
        /// </summary>
        public bool Load() {
            //read all entity definitions from file, find the sprites associated, and dump into memory.

            //get file from resources
            TextAsset ta = Resources.Load(System.IO.Path.Combine(Constants.ResourceNames.ENTITIES, Constants.ResourceNames.FILE_DEFINITIONS)) as TextAsset;

            if (ta == null) {
                Debug.LogError("Entity Resource Manager: Unable to load entities; cannot find definitions file!");
                return false;
            }


            //load contents
            XDocument data = XDocument.Parse(ta.text);


            if (data == null) {
                Debug.LogError("Entity Resource Manager: Unable to load entities; empty or corrupt definitions file!");
                return false;
            }

            //iterate data and push to list
            foreach (XElement entity in data.Element(Constants.XMLHeaders.ENTITIES).Elements()) {
                string id, name, assetName, description;
                bool questGiver;
                Constants.ENTITY_TYPE type = Constants.ENTITY_TYPE.FRIENDLY;

                id = entity.Element(Constants.XMLHeaders.ID).Value;
                assetName = entity.Element(Constants.XMLHeaders.ASSET_NAME).Value;
                name = entity.Element(Constants.XMLHeaders.NAME).Value;
                questGiver = (bool)entity.Element(Constants.XMLHeaders.QUEST_GIVER).Value.Equals(Constants.XMLHeaders.TRUE) ? true : false;

                //todo.. get enum by string..
//                type = (Constants.ENTITY_TYPE)entity.Element(Constants.XMLHeaders.TYPE).Value;

                //todo possibly other specifics.. behaviour? stats?..etc...

                //get the names of the sprites..
                //in directory of project, if there is more than 1 sprite (eg.. for an animation), then,
                //the sprite images would be located under a directory with that asset name 
                Sprite[] sprites = new Sprite[entity.Element(Constants.XMLHeaders.SPRITES).Elements().Count()];

                if (sprites.Length > 1) {
                    //then it must animate!
                    int index = 0;

                    foreach (XElement sprite in entity.Element(Constants.XMLHeaders.SPRITES).Elements()) {
                        string path = System.IO.Path.Combine(Constants.ResourceNames.ENTITIES, assetName, sprite.Value);
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
                    string path = System.IO.Path.Combine(Constants.ResourceNames.ENTITIES,
                        entity.Element(Constants.XMLHeaders.SPRITES).Element(Constants.XMLHeaders.SPRITE).Value);
                    Sprite foundSpriteAsset = Resources.Load<Sprite>(path) as Sprite;

                    if (foundSpriteAsset == null) {
                        //set as missing sprite image
                        sprites[0] = GameController.instance.MISSING_SPRITE;
                    } else {
                        sprites[0] = foundSpriteAsset;
                    }
                }

                //finally add to the cached items
                //todo differentiate between normal entity and living entity..
                cachedEntities.Add(id, new EntityDefinition(id, name, assetName, sprites, type));

            }

            return true;
        }

        /// <summary>
        /// returns the entity defintion of the given id, if it exists.
        /// </summary>
        public EntityDefinition GetFromId(string _id) {
            if (!cachedEntities.ContainsKey(_id)) {
                return null;
            }

            return cachedEntities[_id];
        }

        /// <summary>
        /// returns the entity defintion of the given asset name, if it exists.
        /// </summary>
        public EntityDefinition GetFromAssetName(string _assetName) {
            foreach (EntityDefinition ed in cachedEntities.Values) {
                if (ed.AssetName.Equals(_assetName)) {
                    return ed;
                }
            }

            return null;
        }

        public int Count {
            get { return cachedEntities.Count; }
        }
    }
}