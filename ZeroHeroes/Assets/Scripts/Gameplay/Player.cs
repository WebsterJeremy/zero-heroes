
using Assets.Scripts.ai.state;
using Assets.Scripts.Gameplay;

namespace Assets.Scripts.world
{
    public class Player
    {
        private string _id;
        private Entity playerEntity;

        public Player(string _id, Position _position) {
            playerEntity = GameController.Instance.World.SpawnPlayer(_id, _position);
            playerEntity.walkSpeed = 10f;
        }

        /// <summary>
        /// Attempts to move the player's entity to the given position
        /// </summary>
        public void AttemptMoveTo(Position _position) {
            playerEntity.FSM.EnterState(new FSMStateMoveToPosition(playerEntity, _position));
        }

        public void AttemptPickupItem(CustomItem i, Position _pos) {
            //todo get item id...
            playerEntity.FSM.EnterState(new FSMStatePickupItem(playerEntity, i.Id, _pos));
        }

        public void AttemptDropInventoryItem(string _inventoryItemId) {
            //todo get item id...
            playerEntity.FSM.EnterState(new FSMStateDropItem(playerEntity, _inventoryItemId));
        }

        public Entity Entity {
            get { return playerEntity; }
        }
    }
}
