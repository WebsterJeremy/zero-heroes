/* Player.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 18/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.util.resource.definition;
using Assets.Scripts.world;
using UnityEngine;

namespace Assets.Scripts.player
{
    public class Player
    {
        private string _id;
        private LivingEntity playerEntity;

        public Player(string _id, Position _position) {
            playerEntity = GameController.instance.World.SpawnLivingEntity(_id, GameController.instance.ResourceManager.EntityResourceManager.GetFromAssetName("player"), _position);
        }


        /// <summary>
        /// determines possible actions based on a clicked position, eg walk here, pick up item, etc...
        /// </summary>
        public void InteractWithPosition(Vector3  mousePos, Position _position) {
            //determine what options are available

            //populate the interaction panel in ui
            UIController.instance.ClearInteractionPanelItems();


            //add the callbacks
            //todo walk should only be added when there is actually a tile detected above... but this is a test...
            UIController.instance.AddInteractionPanelItem("Walk Here", () => AttemptMoveTo(_position));
            UIController.instance.AddInteractionPanelItem("Cancel", () => UIController.instance.ShowHideInteractionPanel(false));

            //set the position
            UIController.instance.SetInteractionPanelPosition(mousePos);

            //reveal the panel
            UIController.instance.ShowHideInteractionPanel(true);
        }


        /// <summary>
        /// Attempts to move the player's entity to the given position
        /// </summary>
        public void AttemptMoveTo(Position _position) {
            Debug.Log("adf");
            playerEntity.MoveTo(_position);
        }


        /// <summary>
        /// returns the player's entity's inventory
        /// </summary>
        public Inventory Inventory {
            get { return playerEntity.Inventory; }
        }
    }
}
