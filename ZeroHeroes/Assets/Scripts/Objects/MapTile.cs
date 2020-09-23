using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor.Tilemaps;
#endif

[CreateAssetMenu(fileName = "New Map Tile", menuName = "Tiles/MapTile")]
public class MapTile : Tile
{
#if UNITY_EDITOR
    [CreateTileFromPalette]
#endif
    public static TileBase CreateMapTile(Sprite sprite)
    {
        var mapTile = ScriptableObject.CreateInstance<MapTile>();
        //todo MapTile needs a name var!! once added, please update CustomItem LocalisedName to return this!... eg Resoources.Load... and then get teh name for the return. 
        mapTile.sprite = sprite;
        mapTile.name = sprite.name;
        return mapTile;
    }
}
