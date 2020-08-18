/* ResourceManager.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using UnityEngine;

namespace Assets.Scripts.util.resource
{
    public class ResourceManager
    {
        //purpose of the resource manager is to allow sprites to be loaded into memory at runtime and then called on when needed, rather than going through
        //the pain of looking them up and looking for them again...
        //a solution of caching...

        //this also allows us to simply add an item id to a players inventory, 
        //then when needing to know specifics about an item we can just look it up here.
        //rather than duplciating the item many times... (instancing)


        private ObjectResourceManager objectManager = new ObjectResourceManager();  //for object references.. (objects are non-moveable, non item things... such as a rock, or world representation of an item (a dropped item) 
        private TileResourceManager tileManager   = new TileResourceManager();  //for tile references.. (these are tiles within the world...)
        private ItemResourceManager   itemManager   = new ItemResourceManager();       //for item references.... (these are things in inventories, etc... the definitions of many things)
        private EntityResourceManager entityManager = new EntityResourceManager();  //for entity references.. (these are like living things, or moveable things..)

            
        public ResourceManager() {
            //intentionally left empty.
        }

        public void LoadAll() {
            tileManager.Load();
            itemManager.Load();
            objectManager.Load();
            entityManager.Load();

            Debug.Log(string.Format("{0} tiles loaded from resources.", tileManager.Count));
            Debug.Log(string.Format("{0} items loaded from resources.", itemManager.Count));
            Debug.Log(string.Format("{0} objects loaded from resources.", objectManager.Count));
            Debug.Log(string.Format("{0} entities loaded from resources.", entityManager.Count));
        }

        public ItemResourceManager ItemResourceManager {
            get { return itemManager; }
        }

        public ObjectResourceManager ObjectResourceManager {
            get { return objectManager; }
        }

        public EntityResourceManager EntityResourceManager {
            get { return entityManager; }
        }
        public TileResourceManager TileResourceManager {
            get { return tileManager; }
        }
    }
}