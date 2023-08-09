using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapGenerator tilemapGenerator)
    {
        var cardinalDirections = Direction2D.cardinalDirectionsList;
        var diagonalDirections = Direction2D.diagonalDirectionsList;
        var allDirections = Direction2D.allDirectionsList;

        var basicWallPositions = FindWallsInDirection(floorPositions, cardinalDirections);
        var cornerWallPositions = FindWallsInDirection(floorPositions, diagonalDirections);

        CreateBasicWalls(tilemapGenerator, basicWallPositions, floorPositions, cardinalDirections);
        CreateCornerWalls(tilemapGenerator, cornerWallPositions, floorPositions, allDirections);
    }

    private static void CreateBasicWalls(TilemapGenerator tilemapGenerator, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        foreach (var pos in basicWallPositions)
        {
            string binaryIdentifier = GetBinaryIdentifier(pos, floorPositions, directions);
            tilemapGenerator.PaintBasicWallTile(pos, binaryIdentifier);
        }
    }

    private static void CreateCornerWalls(TilemapGenerator tilemapGenerator, HashSet<Vector2Int> diagonalWallPositions, HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        foreach (var pos in diagonalWallPositions)
        {
            string binaryIdentifier = GetBinaryIdentifier(pos, floorPositions, directions);
            tilemapGenerator.PaintCornerWallTile(pos, binaryIdentifier);
        }
    }

    private static string GetBinaryIdentifier(Vector2Int pos, HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        string binaryIdentifier = "";

        foreach (var dir in directions)
        {
            var adjacentPosition = pos + dir;
            if (floorPositions.Contains(adjacentPosition))
            {
                binaryIdentifier += "1";
            }
            else
            {
                binaryIdentifier += "0";
            }
        }

        return binaryIdentifier;
    }

    private static HashSet<Vector2Int> FindWallsInDirection(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var pos in floorPositions)
        {
            foreach (var dir in directionList)
            {
                var adjacentPosition = pos + dir;
                if (!floorPositions.Contains(adjacentPosition))
                {
                    wallPositions.Add(adjacentPosition);
                } 
            }
        }
        return wallPositions;
    }
}
