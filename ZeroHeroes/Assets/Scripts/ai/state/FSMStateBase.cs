/* FSMStateBase.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 26/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   1/09/2020
 */


using Assets.Scripts.world;

namespace Assets.Scripts.ai.state
{
    public abstract class FSMStateBase
    {
        protected Entity parent;

        protected Constants.FSMActionState actionState;
        protected bool enteredState;

        public virtual void OnInitialized() {
            actionState = Constants.FSMActionState.NONE;
        }

        public virtual bool EnterState() {
            actionState = Constants.FSMActionState.ACTIVE;
            UIController.SetDebugStatistic("Player State", this.ToString()); // Debug
            return true;
        }

        public virtual bool ExitState() {
            actionState = Constants.FSMActionState.COMPLETED;
            return true;
        }

        //every tick of the fsm it will update the current state... 
        public abstract void UpdateState();

        public Constants.FSMActionState ActionState {
            get { return actionState; }
            set { actionState = value; }
        }

        public bool EnteredState {
            get { return enteredState; }
        }
    }
}
