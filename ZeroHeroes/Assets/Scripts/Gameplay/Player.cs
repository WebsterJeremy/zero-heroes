
namespace Assets.Scripts.world
{
    public class Player
    {
        private string _id;
        private Entity playerEntity;

        public Player(string _id, Position _position) {
            playerEntity = GameController.Instance.World.SpawnPlayer(_id, _position);
        }

        /// <summary>
        /// Attempts to move the player's entity to the given position
        /// </summary>
        public void AttemptMoveTo(Position _position) {
            playerEntity.MoveTo(_position);
        }

        public Entity Entity {
            get { return playerEntity; }
        }
    }
}
