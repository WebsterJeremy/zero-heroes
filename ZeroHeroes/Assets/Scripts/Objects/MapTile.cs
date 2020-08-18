using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Tilemaps;

[CreateAssetMenu(fileName = "New Map Tile", menuName = "Tiles/MapTile")]
public class MapTile : Tile
{
    [CreateTileFromPalette]
    public static TileBase CreateMapTile(Sprite sprite)
    {
        var mapTile = ScriptableObject.CreateInstance<MapTile>();
        mapTile.sprite = sprite;
        mapTile.name = sprite.name;
        return mapTile;
    }
}
