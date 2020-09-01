/* FSMStateIdle.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 26/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   1/09/2020
 */


using Assets.Scripts.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ai.state
{
    public class FSMStateIdle : FSMStateBase
    {
 
        public FSMStateIdle(Entity _parent) {
            this.parent = _parent;
        }

        public override bool EnterState() {
//            Debug.Log("Entering Idle State");
            base.EnterState();
            enteredState = true;
            return enteredState;
        }

        public override bool ExitState() {
//            Debug.Log("Exiting Idle State");

            return base.ExitState();
        }

        public override void UpdateState() {
//            Debug.Log("Updating Idle State");
        }
    }
}
