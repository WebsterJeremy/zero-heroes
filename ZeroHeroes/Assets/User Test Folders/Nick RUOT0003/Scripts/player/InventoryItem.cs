/* InventoryItem.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.util.resource.definition;

namespace Assets.Scripts
{
    public class InventoryItem
    {

        private string id, itemDefintionId;
        private int amount;

        public InventoryItem(string _id, string _itemDefinitionId, int _amount) {
            this.id = _id;
            this.itemDefintionId = _itemDefinitionId;
            this.amount = _amount;
        }

        public InventoryItem(string _id, string _itemDefinitionId) {
            this.id = _id;
            this.itemDefintionId = _itemDefinitionId;
            this.amount = 1;//if amount isnt listed then the amount must be 1 (since it exists in inventory...
        }

        public ItemDefinition ItemDefinition {
            //this is the item definition from memory, in case we need to spawn this in the real world as an "Item"
            get { return GameController.instance.ResourceManager.ItemResourceManager.GetFromId(itemDefintionId); }
        }

        public string Id {
            get { return id; }
        }
        
        public int Amount {
            get {

                if(ItemDefinition.IsStackable) {
                    //return true amount if stackable, this could be 1 or it could be 10..etc..
                    return amount;
                }

                //if not stackable just return 1 regardless of what it may be listed as.
                return 1;
            }
        }

        public void AddAmount(int _amount) {
            if (ItemDefinition.IsStackable && Amount < Constants.MAX_STACK_SIZE) {
                amount += _amount;

                if (amount > Constants.MAX_STACK_SIZE) {
                    //should probably just attempt to add another item to inventory...
                    amount = Constants.MAX_STACK_SIZE;
                }
            }
        }

        public void SubtractAmount(int _amount) {
            if(ItemDefinition.IsStackable) {
                amount -= _amount;
                
                if(amount < 0) {
                    amount = 0;
                }
            }
        }
    }


}
