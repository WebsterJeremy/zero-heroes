/* EntityDefinition.cs
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
    public class EntityDefinition : ResourceDefinitionBase
    {
        private Constants.ENTITY_TYPE type;
        public EntityDefinition(string _id, string _name, string _assetName, Sprite[] _sprites, Constants.ENTITY_TYPE _type) : base(_id, _name, _assetName, string.Empty, _sprites) {
            this.type = _type;
            
            //todo other things.. behavioir, stats,etc???

            /*todo need to figure out how we can generate animations on the fly from the definition generation
             *
             *this could be achieved by specifying a folder for the sprite files of 1 animation and specifying the length (index) and duration in seconds in the definiton itself
             * 
             * - RUOT0003
             */
        }
    }
}
