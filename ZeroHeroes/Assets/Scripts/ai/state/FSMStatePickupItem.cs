/* FSMStatePickupItem.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 26/08/2020
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
    public class FSMStatePickupItem : FSMStateBase
    {
        private string itemId;
        private Position itemPosition;

        public FSMStatePickupItem(Entity _parent, string _itemId, Position _itemPosition) {
            this.parent = _parent;
            this.itemId = _itemId;
            this.itemPosition = _itemPosition;

            Debug.Log("parent: " + parent.Id + ", item id: " + itemId + ", itempos: " + itemPosition.ToString());
        }

        public override bool EnterState() {
            //entering state
            Debug.Log("Entered Pickup Item State");
            if (base.EnterState()) {
                if(parent == null || string.IsNullOrWhiteSpace(itemId) || itemPosition == null) {
                    //if either of these are null, return false, set as terminated, because it failed, 
                    //then return false.. as it wasnt able to enter state without this data.
                    actionState = Constants.FSMActionState.TERMINATED;
                    enteredState = false;
                    return enteredState;
                }
            }

            enteredState = true;
            //setup path
            parent.MovementHelper.MoveTo(itemPosition);

            return enteredState;
        }

        public override bool ExitState() {
            //set to another state...
            parent.FSM.EnterState(new FSMStateIdle(parent));
            return base.ExitState();
        }

        public override void UpdateState() {
            if (!parent.MovementHelper.IsMoving && actionState == Constants.FSMActionState.COMPLETED) {
                ExitState();
                return;
            }

            if (parent.MovementHelper.GoalPosition != null && parent.MovementHelper.GoalPosition.Equals(parent.Position()) ) {
                //entity is at goal position...  or entity is no longer moving..
                //then exit the state.. completed!

                //detect if item still exists...
                CustomItem i = GameController.Instance.World.GetTileFromPosition(itemPosition).GetChildItem(itemId);

                if(i != null) {
                    //pick it up..
                    i.Pickup(parent);
                }

                actionState = Constants.FSMActionState.COMPLETED;
            }
        }
    }
}
