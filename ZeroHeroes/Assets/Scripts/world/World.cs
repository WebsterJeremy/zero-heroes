using Assets.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.world
{
    public class World
    {
        private Tilemap tilemap;
        private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
        private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();

        public bool IsTileTraversable(Position _position) {
            Tile tile = GetTileFromPosition(_position);

            if (tile == null) {
                return false;
            }

            return tile.IsTraversable;
        }

        public string GenerateUniqueId() {
            return Guid.NewGuid().ToString();
        }

        public void GenerateWorld(Tilemap _tilemap) {
            this.tilemap = _tilemap;
            //this needs work but can generate random world from our own heightmap (cloud).
            //other aspects can be added based on this too, such as foliage, water/rivers, etc..


            //assemble list of Tiles based on the tiles in the TileMap.

            /*BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

            for (int x = 0; x < bounds.size.x; x++) {
                for (int y = 0; y < bounds.size.y; y++) {
                    TileBase tile = allTiles[x + y * bounds.size.x];

                    bool isTraversable = tile == null;//if null then it is traversable because no collision tile exists..


                    if (!isTraversable) {
                        Debug.Log(new Position(x, y).ToString());
                    }

                    Tile newTile = new Tile(GenerateUniqueId(), new Position(x, y), isTraversable);

                    tiles.Add(newTile.Id, newTile);
                }
            }*/

            foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = tilemap.CellToWorld(localPlace);

                bool isTraversable = !tilemap.HasTile(localPlace);//if it has a tile, it is not traversable

                Tile newTile = new Tile(GenerateUniqueId(), new Position(localPlace.x + 1, localPlace.y + 1), isTraversable);
                tiles.Add(newTile.Id, newTile);
            }

        }

        public void InteractWithPosition(Vector3 mousePos, Position _position) {
            //see below note why thesse is commented... 
            /*Tile tile = GetTileFromPosition(_position);

            if (tile == null) {
                return;
            }

            List<Object> objectsAtPosition = tile.GetChildObjects;
            List<Item> itemsAtPosition = tile.GetChildItems;
            */



            //populate the interaction panel in ui

            UIController.Instance.ClearInteractionPanelItems();


            //add the callbacks
            //todo walk should only be added when there is actually a tile detected above... but this is a test...
            UIController.Instance.AddInteractionPanelItem("Walk Here", () => GameController.Instance.Player.AttemptMoveTo(_position));

            /*todo this was from the code i made but Item was deleted for some reason so its not implemented here..
             * it would work as adding the options for the items on that tile...
            foreach (Item i in itemsAtPosition) {
                string actionString = "";

                if (i.Amount > 1) {
                    actionString = string.Format(Constants.Actions.PICKUP_ITEMS, i.Definition.Name, i.Amount);
                } else {
                    actionString = string.Format(Constants.Actions.PICKUP_ITEM, i.Definition.Name);
                }

                UIController.Instance.AddInteractionPanelItem(actionString, () => GameController.instance.Player.AttemptPickupItem(i, _position));
            }*/

            UIController.Instance.AddInteractionPanelItem("Cancel", () => UIController.Instance.ShowHideInteractionPanel(false));

            //set the position
            UIController.Instance.SetInteractionPanelPosition(mousePos);

            //reveal the panel
            UIController.Instance.ShowHideInteractionPanel(true);
        }


        public Tile GetTileFromPosition(Position _position) {
            foreach (Tile t in tiles.Values) {
                if (t.Position().Equals(_position)) {
                    return t;
                }
            }

            return null;
        }


        public Entity SpawnPlayer(string _id, Position _position) {
            Entity entity = new Entity(GenerateUniqueId(), _position);
            entity.GameObject = GameObject.Instantiate(GameController.Instance.playerPrefab);
            entity.UpdatePosition(_position, true);
            entity.GameObject.transform.SetParent(GameController.Instance.gameplayContainer, false);
            entities.Add(entity.Id, entity);
            return entity;
        }
        public Tile GetTileFromId(string _id) {
            if (!tiles.ContainsKey(_id)) {
                return null;
            }
            return tiles[_id];
        }

        public List<Tile> GetTileNeighbours(Tile _current) {
            List<Tile> neighbours = new List<Tile>();

            if (_current != null) {
                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {
                        //if(x == 0 && y == 0) { this will check all directions (and create diag paths..

                        if ((x == 0 && y == 0) || (x != 0 && y != 0)) {// however this will only create paths of NWSE
                            continue;
                        }

                        Tile neighbouringWorldTile = GetTileFromPosition(new Position(_current.Position().X + x, _current.Position().Y + y));

                        if (neighbouringWorldTile != null) {
                            neighbours.Add(neighbouringWorldTile);
                        }
                    }
                }
            }

            return neighbours;
        }

        public int GetDistance(Tile _current, Tile _target) {

            if (_current == null || _target == null) {
                Debug.LogError("World: Cannot get distance between two tile positions that do not exist!");
                return 0;
            }

            int distX = Mathf.Abs(_current.Position().X - _target.Position().X);
            int distY = Mathf.Abs(_current.Position().Y - _target.Position().Y);


            return distX > distY ? 14 * distY * (distX - distY) : 14 * distX * (distY - distX);
        }



        public int GetDistanceBetweenTwoPositions(Position positionA, Position positionB) {
            /*           if (!IsTileValid(positionA) || !IsTileValid(positionB)) {
                           return 0;
                       }
                       */

            int distX = Mathf.Abs(positionA.X - positionB.X);
            int distY = Mathf.Abs(positionA.Y - positionB.Y);

            return Mathf.RoundToInt(Mathf.Sqrt(distX * distX + distY * distY));
        }


        /// <summary>
        /// max size of the game world 
        /// </summary>
        public int MaxSize {
            get { return Width * Height; }
        }

        public Tilemap TileMap {
            get { return tilemap; }
        }

        /// <summary>
        /// the width of the world
        /// </summary>
        public int Width {
            get { return tilemap.size.x; }
        }
        /// <summary>
        /// the height of the world
        /// </summary>
        public int Height {
            get { return tilemap.size.y; }
        }
    }

}
