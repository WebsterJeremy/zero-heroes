/* FSMStateMoveToPosition.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 31/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   1/9/2020
 */


using Assets.Scripts.Utility;
using Assets.Scripts.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ai.state
{
    public class FSMStateMoveToPosition : FSMStateBase
    {
        private Position targetPosition;

        public FSMStateMoveToPosition(Entity _parent, Position _targetPosition) {
            this.parent = _parent;
            this.targetPosition = _targetPosition;
        }

        public override bool EnterState() {
            //entering state
            Debug.Log("Entered Move To Position State");
            if (base.EnterState()) {
                if(parent == null || targetPosition == null) {
                    //if either of these are null, return false, set as terminated, because it failed, 
                    //then return false.. as it wasnt able to enter state without this data.
                    actionState = Constants.FSMActionState.TERMINATED;
                    enteredState = false;
                    return enteredState;
                }    
            }

            enteredState = true;
            //setup path
            parent.MovementHelper.MoveTo(targetPosition);

            return enteredState;
        }

        public override bool ExitState() {
            Debug.Log("Exited Move To Position State");
            parent.FSM.EnterState(new FSMStateIdle(parent));
            return base.ExitState();
        }

        public override void UpdateState() {
            Debug.Log("Updating Move To Position State");

            if ((parent.MovementHelper.GoalPosition != null && parent.MovementHelper.GoalPosition.Equals(parent.Position()))) {
                //entity is at goal position...  or entity is no longer moving..
                //then exit the state.. completed!
                actionState = Constants.FSMActionState.COMPLETED;
            }

            if (!parent.MovementHelper.IsMoving && actionState == Constants.FSMActionState.COMPLETED) {
                ExitState();
                return;
            }
        }
    }
}
