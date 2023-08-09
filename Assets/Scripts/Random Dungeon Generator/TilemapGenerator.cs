using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    [SerializeField]
    private List<TileBase> floorTiles, wallUpTiles, wallDownTiles, wallLeftTiles, wallRightTiles;

    [SerializeField]
    private TileBase wallFullTile, wallLeftToDownCornerTile, wallRightToDownCornerTile, wallDownToRightCornerTile, wallDownToLeftCornerTile;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTiles);
    }

    public void PaintBasicWallTile(Vector2Int wallPosition, string binaryIdentifier)
    {
        int identifierAsInt = Convert.ToInt32(binaryIdentifier, 2);

        TileBase tile = null;

        if (WallTypesByBits.wallUp.Contains(identifierAsInt))
        {
            tile = wallUpTiles[Random.Range(0, wallUpTiles.Count)];

        } else if (WallTypesByBits.wallDown.Contains(identifierAsInt))
        {
            tile = wallDownTiles[Random.Range(0, wallDownTiles.Count)];

        } else if (WallTypesByBits.wallRight.Contains(identifierAsInt))
        {
            tile = wallRightTiles[Random.Range(0, wallRightTiles.Count)];

        } else if (WallTypesByBits.wallLeft.Contains(identifierAsInt))
        {
            tile = wallLeftTiles[Random.Range(0, wallLeftTiles.Count)];

        } else if (WallTypesByBits.wallFull.Contains(identifierAsInt))
        {
            tile = wallFullTile;

        }

        if (tile != null)
            PaintTile(wallPosition, wallTilemap, tile);
    }

    public void PaintCornerWallTile(Vector2Int wallPosition, string binaryIdentifier)
    {
        int identifierAsInt = Convert.ToInt32(binaryIdentifier, 2);

        TileBase tile = null;

        if (WallTypesByBits.wallLeftToDownCorner.Contains(identifierAsInt))
        {
            tile = wallLeftToDownCornerTile;

        }
        else if (WallTypesByBits.wallRightToDownCorner.Contains(identifierAsInt))
        {
            tile = wallRightToDownCornerTile;

        }
        else if (WallTypesByBits.wallDownToLeftCorner.Contains(identifierAsInt))
        {
            tile = wallDownToLeftCornerTile;

        }
        else if (WallTypesByBits.wallDownToRightCorner.Contains(identifierAsInt))
        {
            tile = wallDownToRightCornerTile;

        }
        else if (WallTypesByBits.wallFullEightDirections.Contains(identifierAsInt))
        {
            tile = wallFullTile;

        }
        else if (WallTypesByBits.wallDownEightDirections.Contains(identifierAsInt))
        {
            tile = wallDownTiles[Random.Range(0, wallDownTiles.Count)];

        }
        else if (WallTypesByBits.wallLeftToUpCorner.Contains(identifierAsInt))
        {
            tile = wallLeftTiles[Random.Range(0, wallLeftTiles.Count)]; ;

        }
        else if (WallTypesByBits.wallRightToUpCorner.Contains(identifierAsInt))
        {
            tile = wallRightTiles[Random.Range(0, wallRightTiles.Count)];

        }


        if (tile != null)
            PaintTile(wallPosition, wallTilemap, tile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, List<TileBase> tiles)
    {
        foreach (var pos in positions)
        {
            TileBase tile = tiles[Random.Range(0, tiles.Count)];
            PaintTile(pos, tilemap, tile);
        }
    }

    private void PaintTile(Vector2Int pos, Tilemap tilemap, TileBase tile)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePosition, tile);
    }

    public void ClearTilemap()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

}
