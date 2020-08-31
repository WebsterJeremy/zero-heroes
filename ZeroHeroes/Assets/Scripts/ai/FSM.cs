/* FSM.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 18/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.ai.state;
using Assets.Scripts.world;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ai
{
    public class FSM
    {
        Entity parent;
        FSMStateBase startState;
        FSMStateBase previousState;
        FSMStateBase currentState;

        private bool canUpdate;
        IEnumerator coroutine;

        public FSM(Entity _parent, FSMStateBase _startState) {
            this.parent = _parent;
            this.startState = _startState;
        }

        public void OnInitialized() {
            if (startState != null) {
                EnterState(startState);
            }
        }

        #region FSM Management
        public void EnterState(FSMStateBase nextState) {
            if (nextState == null) {
                return;
            }

            this.canUpdate = false;

            //stop the current state update routine
            if (coroutine != null) {
                GameController.Instance.StopCoroutine(coroutine);
            }

            //set the previous state to the current state...
            previousState = currentState;
            //set current state to the next state..
            currentState = nextState;

            //restart the update ticks
            this.canUpdate = true;

            //enter the new current state... and begin updating...
            if (currentState.EnterState()) {
                coroutine = TickCurrentState();
                GameController.Instance.StartCoroutine(coroutine);
            }
        }

        private IEnumerator TickCurrentState() {
            //this is the update method of the FSM.. where the state is ticked over.. (updated)
            while (currentState != null && CanUpdate) {
                currentState.UpdateState();

                yield return new WaitForEndOfFrame();//wait until the end of the frame so we dont overload...
            }
        }
        #endregion

        #region Getters
        public bool CanUpdate {
            get { return canUpdate && GameController.Instance.CurrentGameState == GameController.GameState.PLAYING; }
        }

        public FSMStateBase CurrentState {
            get { return currentState; }
        }

        public Entity ParentEntity {
            get { return parent; }
        }
        #endregion
    }
}
