/* World.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts.ai.path;
using Assets.Scripts.util.resource.definition;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.world
{
    public class World {
        private int width, height;

        private Dictionary<string, Item> spawnedItems = new Dictionary<string, Item>();
        private Dictionary<string, Tile> spawnedTiles = new Dictionary<string, Tile>();
        private Dictionary<string, Assets.Scripts.world.ObjectBase> spawnedObjects = new Dictionary<string, Assets.Scripts.world.ObjectBase>();
        private Dictionary<string, Entity> spawnedEntities = new Dictionary<string, Entity>();

        #region World Load and Generation
        public void GenerateWorld(int _width, int _height) {
            //this needs work but can generate random world from our own heightmap (cloud).
            //other aspects can be added based on this too, such as foliage, water/rivers, etc..
            this.width = _width;
            this.height = _height;


            TileDefinition testDef = GameController.instance.ResourceManager.TileResourceManager.GetFromAssetName("grass");
            TileDefinition testDef2 = GameController.instance.ResourceManager.TileResourceManager.GetFromAssetName("water");

            //generate here..
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    //todo for now just generate grass world... we will look into generation later..
                    SpawnTile(testDef, new Position(x, y));
                }
            }

            //todo update some tiles as water.. this is a test...
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    //todo for now just generate grass world... we will look into generation later..
                    SpawnTile(testDef2, new Position(x, y));
                }
            }

            ItemDefinition itemdef = GameController.instance.ResourceManager.ItemResourceManager.GetFromAssetName("stick");
            SpawnItem(itemdef, new Position(6, 6), 2);

            //write to world file..
        }

public int TileCount {
            get { return spawnedTiles.Count; }
        }

        public void LoadWorld(string _worldName) {
            //this would be stored data...
        }
        #endregion

        #region Spawn
        public Tile SpawnTile(TileDefinition tileDefinition, Position position) {
            Tile tile = new Tile(Constants.GenerateUniqueId(), tileDefinition.Id, position);

            //check if tile already exists..
            Tile existingTile = GetTileFromPosition(position.X, position.Y);

            if (existingTile != null) {
                //then update that tile
                spawnedTiles[existingTile.Id].Destroy();
                spawnedTiles.Remove(existingTile.Id);
            }

            spawnedTiles.Add(tile.Id, tile);
            return tile;
        }


        public Entity SpawnEntity(EntityDefinition entityDefinition, Position position) {
            Entity entity = new Entity(Constants.GenerateUniqueId(), entityDefinition.Id, position);
            spawnedEntities.Add(entity.Id, entity);
            return entity;
        }
        public LivingEntity SpawnLivingEntity(EntityDefinition entityDefinition, Position position) {
            LivingEntity entity = new LivingEntity(Constants.GenerateUniqueId(), entityDefinition.Id, position);
            spawnedEntities.Add(entity.Id, entity);
            return entity;
        }
        public LivingEntity SpawnLivingEntity(string _id, EntityDefinition entityDefinition, Position position) {
            LivingEntity entity = new LivingEntity(_id, entityDefinition.Id, position);
            spawnedEntities.Add(entity.Id, entity);
            return entity;
        }

        public Object SpawnObject(Object objectDefinition, Position position) {
            Object obj = new Object(Constants.GenerateUniqueId(), objectDefinition.Id, position);
            spawnedObjects.Add(obj.Id, obj);
            return obj;
        }

        public Item SpawnItem(ItemDefinition itemDefinition, Position position, int amount) {
            Item item = new Item(Constants.GenerateUniqueId(), itemDefinition.Id, position);
            spawnedItems.Add(item.Id, item);
            return item;
        }
        #endregion

        #region Remove From List
        public void RemoveTileFromList(string _id) {
            spawnedTiles.Remove(_id);
        }


        public void RemoveObjectFromList(string _id) {
            spawnedObjects.Remove(_id);
        }


        public void RemoveItemFromList(string _id) {
            spawnedItems.Remove(_id);
        }

        public void RemoveEntityFromList(string _id) {
            spawnedEntities.Remove(_id);
        }
        #endregion

        #region World 
        public Entity GetEntityFromId(string _id) {
            foreach (Entity entity in spawnedEntities.Values) {
                if (entity.Id.Equals(_id)) {
                    return entity;
                }
            }

            return null;
        }

        public Tile GetTileFromId(string _id) {
            foreach (Tile tile in spawnedTiles.Values) {
                if (tile.Id.Equals(_id)) {
                    return tile;
                }
            }

            return null;
        }

        public Tile GetTileFromPosition(int _x, int _y) {
            foreach (Tile tile in spawnedTiles.Values) {
                if (tile.Position.Equals(_x, _y)) {
                    return tile;
                }
            }

            return null;
        }

        public Tile GetTileFromPosition(Position _position) {
            return GetTileFromPosition(_position.X, _position.Y);
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

                        Tile neighbouringWorldTile = GetTileFromPosition(_current.Position.X + x, _current.Position.Y + y);

                        if (neighbouringWorldTile != null) {
                            neighbours.Add(neighbouringWorldTile);
                        }
                    }
                }
            }

            return neighbours;
        }

/*        public bool IsTileValid(Position _position) {
            if (!_position.IsWithinWorldBounds()) {
                return false;
            }


            Tile tile = GetTileFromPosition(_position);

            if(tile == null) {
                return false;
            }
            

            //if reached here then position is valid position and tile exists in the world
            return tile.Definition.IsTraversable;
        }
        */

        public bool IsTileTraversable(Position _position) {
            Tile tile = GetTileFromPosition(_position);

            if (tile == null) {
                return false;
            }

            return tile.Definition.IsTraversable;
        }

        public int GetDistance(Tile _current, Tile _target) {

            if (_current == null || _target == null) {
                Debug.LogError("World: Cannot get distance between two tile positions that do not exist!");
                return 0;
            }


/*            if (!IsTileValid(tileA.Position) || !IsTileValid(tileB.Position)) {
                Debug.LogError("World: Cannot get distance between two tile positions. One or both are not valid tiles!");
                return 0;
            }
            */

            int distX = Mathf.Abs(_current.Position.X - _target.Position.X);
            int distY = Mathf.Abs(_current.Position.Y - _target.Position.Y);


            return distX > distY ? 14 * distY * (distX - distY) : 14 * distX * (distY - distX);
        }


        //todo 0 refs to may not be required anymore
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
            get { return width * height; }
        }

        /// <summary>
        /// the width of the world
        /// </summary>
        public int Width {
            get { return width; }
        }
        /// <summary>
        /// the height of the world
        /// </summary>
        public int Height {
            get { return height; }
        }
        #endregion

    }
}