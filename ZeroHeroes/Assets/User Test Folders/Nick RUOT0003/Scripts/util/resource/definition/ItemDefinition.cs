/* ItemDefinition.cs
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
    public class ItemDefinition : ResourceDefinitionBase
    {
        private bool stackable;
        private bool questItem;
        
        public ItemDefinition(string _id, string _name, string _assetName, string _description, Sprite[] _sprites, bool _stackable, bool _questItem) : base(_id, _name, _assetName, _description, _sprites) {
            this.stackable = _stackable;
            this.questItem = _questItem;

            //todo other tags, value, etc?? type?

            /*todo need to figure out how we can generate animations on the fly from the definition generation
             *
             *this could be achieved by specifying a folder for the sprite files of 1 animation and specifying the length (index) and duration in seconds in the definiton itself
             * 
             * - RUOT0003
             */
        }


        public bool IsQuestItem {
            get { return questItem; }
        }
       
        public bool IsStackable {
            get { return stackable; }
        }
    }
}
