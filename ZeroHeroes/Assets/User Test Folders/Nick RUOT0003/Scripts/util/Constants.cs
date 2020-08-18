/* Constants.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using System;

namespace Assets
{
    public class Constants
    {
        public enum FACING { NORTH = 1, WEST = 2, SOUTH = 3, EAST = 4}
        public enum ENTITY_TYPE { FRIENDLY, HOSTILE, PASSIVE }
        public static string MOVE_STATE = "MOVE_{0}";
        public static string IDLE_STATE = "IDLE_{0}";
        public static float ANIMATION_SPEED = 0.5f;
        public static float DEFAULT_WALK_SPEED = 5.0f;

        public static int MAX_STACK_SIZE = 10;

        public static string GenerateUniqueId() {
            return Guid.NewGuid().ToString();
        }

        public class ResourceNames
        {
            public static string ENTITIES = "entities";
            public static string TILES = "tiles";
            public static string OBJECTS = "objects";
            public static string ITEMS = "items";

            public static string FILE_DEFINITIONS = "definitions";
        }

        public class XMLHeaders
        {
            public static string ENTITIES = "Entities";
            public static string TILES = "Tiles";
            public static string OBJECTS = "Objects";
            public static string ITEMS = "Items";
            public static string ENTITY = "Entity";
            public static string TILE = "Tile";
            public static string OBJECT = "Object";
            public static string ITEM = "Item";
            public static string ID = "Id";
            public static string NAME = "Name";
            public static string ASSET_NAME = "AssetName";//asset name is a bit confusing. to sum this up, this is so we can safely reference asset resources.. eg, an asset may be called Magic Sword, so the asset name is magic_sword
            public static string DESCRIPTION = "Description";
            public static string TRAVERSABLE = "Traversable";
            public static string STACKABLE = "Stackable";
            public static string QUEST_ITEM = "QuestItem";
            public static string QUEST_GIVER = "QuestGiver";
            public static string SPRITES = "Sprites";
            public static string SPRITE = "Sprite";
            public static string TYPE = "Type";
            
            public static string TRUE = "true";
            public static string FALSE = "false";


        }
    }
}
