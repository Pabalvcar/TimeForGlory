using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected SimpleRandomWalkSO smallRoomParameters;

    [SerializeField]
    protected SimpleRandomWalkSO largeRoomParameters;

    [SerializeField]
    protected SimpleRandomWalkSO weirdSmallRoomParameters;

    [SerializeField]
    protected SimpleRandomWalkSO weirdLargeRoomParameters;

    [SerializeField]
    protected SimpleRandomWalkSO megaRoomParameters;

    protected override void RunProceduralGeneration()
    {
        tilemapGenerator.ClearTilemap();
        HashSet<Vector2Int> floorPositions = RunRandomWalk(smallRoomParameters, startingPosition);
        tilemapGenerator.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapGenerator);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int startPosition)
    {
        Vector2Int currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLenght);
            floorPositions.UnionWith(path);
            if (parameters.startIterationOnRandomTile)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }

}