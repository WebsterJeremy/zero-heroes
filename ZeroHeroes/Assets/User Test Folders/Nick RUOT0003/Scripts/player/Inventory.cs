/* Inventory.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */ 

using Assets.Scripts.util.resource.definition;
using Assets.Scripts.world;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Inventory {

        private string entityOwnerId;
        private List<InventoryItem> items = new List<InventoryItem>();

        public Inventory(string _entityOwnerId) {
            //this is needed for when we update the save files.
            this.entityOwnerId = _entityOwnerId;
            LoadInventory();
        }
        
        private void LoadInventory() {
            //this is used to load the entities inventory using the entity owner id.
        }

        public void Add(string _inventoryItemId, string _itemReferenceId, int amount) {

            if (string.IsNullOrWhiteSpace(_inventoryItemId)) {
                _inventoryItemId = Constants.GenerateUniqueId();
            }
            //determine if the given item definition (by reference id) is stackable, see if the item ref id is already listed in inventory, add to stack what you can.
            //add to new stack for difference
            ItemDefinition itemDef = GameController.instance.ResourceManager.ItemResourceManager.GetFromId(_itemReferenceId);
            
            if(itemDef == null) {
                //todo log error
                return;
            }

            if (itemDef.IsStackable) {
                //if the item is stackable then add to stack until reach max item stack amount... then add difference until complete

                int itemsLeftToAdd = amount;
                
                while(itemsLeftToAdd > 0) {
                    if(itemsLeftToAdd - Constants.MAX_STACK_SIZE > 0) {
                        //if the amount of items left to add minus the max stack size is greater than 0,  then add the max stack size, 
                        items.Add(new InventoryItem(_inventoryItemId, itemDef.Id, Constants.MAX_STACK_SIZE));
                        //and subtract the amount left
                        itemsLeftToAdd -= Constants.MAX_STACK_SIZE;
                    } else {
                        //if a max stack cant be added, then add whats left
                        if(itemsLeftToAdd > 0) {
                            //add the remaining amount
                            items.Add(new InventoryItem(_inventoryItemId, itemDef.Id, itemsLeftToAdd));
                            //and subtract the amount left
                            itemsLeftToAdd -= itemsLeftToAdd;

                            //from here it should break out of the while loop
                        }
                    }
                }
            } else {
                //if the item is not stackable, add items to inventory until complete or full.
                for (int i = 0; i < amount; i++) {
                    items.Add(new InventoryItem(Constants.GenerateUniqueId(), itemDef.Id));
                }
            }
            
            //todo update inventory to file...
        }

        public InventoryItem GetInventoryItemFromId(string _itemId) {
            foreach(InventoryItem inventoryItem in items) {
                if (inventoryItem.Id.Equals(_itemId)) {
                    return inventoryItem;
                }
            }

            return null;
        }

        public void DropItem(string _itemId, int amount) {
            InventoryItem ii = GetInventoryItemFromId(_itemId);

            if(ii == null) {
                return;
            }

            Position position = GameController.instance.World.GetEntityFromId(entityOwnerId).Position;

            if(ii.ItemDefinition != null && position != null) {
                //spawn the item in the world and remove it from inventory...
                GameController.instance.World.SpawnItem(ii.ItemDefinition, position, amount);

                Remove(ii, amount);
            }
        }

        private void Remove(InventoryItem _inventoryItem, int amount) {
            if (_inventoryItem.ItemDefinition.IsStackable) {
                _inventoryItem.SubtractAmount(amount);

                if (_inventoryItem.Amount <= 0) {
                    items.Remove(_inventoryItem);
                } 
            } else {
                items.Remove(_inventoryItem);
            }

        } 
    }
}
