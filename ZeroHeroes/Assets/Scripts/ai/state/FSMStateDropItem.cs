/* FSMStateDropItem.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 1/09/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   31/08/2020
 */


using Assets.Scripts.Gameplay;
using Assets.Scripts.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ai.state
{
    public class FSMStateDropItem : FSMStateBase
    {
        private string itemId;

        public FSMStateDropItem(Entity _parent, string _itemId) {
            this.parent = _parent;
            this.itemId = _itemId;
        }

        public override bool EnterState() {
            //entering state
//            Debug.Log("Entered Drop Item State");
            if (base.EnterState()) {
                if(parent == null || string.IsNullOrWhiteSpace(itemId)) {
                    //if either of these are null, return false, set as terminated, because it failed, 
                    //then return false.. as it wasnt able to enter state without this data.
                    actionState = Constants.FSMActionState.TERMINATED;
                    enteredState = false;
                    return enteredState;
                }
            }

            enteredState = true;
            return enteredState;
        }

        public override bool ExitState() {
            //set to another state...
            parent.FSM.EnterState(new FSMStateIdle(parent));
            return base.ExitState();
        }

        public override void UpdateState() {
            if (actionState == Constants.FSMActionState.COMPLETED) {
                ExitState();
                return;
            }

            //ensure that the entity has the item in their inventory..
            InventoryItem ii = parent.Inventory.GetInventorytemFromId(itemId);

            if(ii == null) {
                actionState = Constants.FSMActionState.COMPLETED;
                return;
            }

            //destroy it, 
            parent.Inventory.Remove(itemId);

            //then spawn in real world..
            GameController.Instance.World.SpawnItem(ii.AssetName, parent.Position(), ii.Amount);

            //return 
            actionState = Constants.FSMActionState.COMPLETED;
            return;
        }
    }
}
