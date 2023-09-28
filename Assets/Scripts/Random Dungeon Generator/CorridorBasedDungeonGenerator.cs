using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CorridorBasedDungeonGenerator : SimpleRandomWalkDungeonGenerator
{

    [SerializeField]
    private int corridorLenght = 15;

    [SerializeField]
    private int corridorNumber = 5;

    [SerializeField]
    [Range(0.1f,1)]
    private float roomCreationChance = 0.8f;

    [SerializeField]
    [Range(0.1f, 1)]
    private float roomAmountPercent = 0.5f;

    [SerializeField]
    [Range(0.01f, 1)]
    private float weirdRoomChance = 0.15f;

    [SerializeField]
    [Range(0.01f, 1)]
    private float megaRoomChance = 0.01f;

    private HashSet<Vector2Int> invalidSpawnPositions;

    public static CorridorBasedDungeonGenerator Instance { get; private set; }

    public HashSet<Vector2Int> tiles { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    protected override void RunProceduralGeneration()
    {

        invalidSpawnPositions = new HashSet<Vector2Int>();

        float isMegaRoom = Random.value;
        if (isMegaRoom <= megaRoomChance)
        {
            MegaRoomGeneration();
        } else
        {
            CorridorBasedGeneration();
        }

    }

    private void MegaRoomGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(megaRoomParameters, startingPosition);

        int roomAmountFactor = 20;

        SpawnPlayer(floorPositions);
        SpawnStairs(floorPositions);
        SpawnChests(floorPositions);
        SpawnEnemies(floorPositions, roomAmountFactor);

        tiles = floorPositions;

        tilemapGenerator.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapGenerator);
    }

    private void CorridorBasedGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> validRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, validRoomPositions);

        HashSet<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);
        HashSet<Vector2Int> roomPositions = CreateRooms(validRoomPositions, deadEnds);

        SpawnPlayer(roomPositions);
        SpawnStairs(roomPositions);
        SpawnChests(roomPositions);
        SpawnEnemies(roomPositions, validRoomPositions.Count);

        floorPositions.UnionWith(roomPositions);

        tiles = floorPositions;

        tilemapGenerator.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapGenerator);
    }

    private void SpawnPlayer(HashSet<Vector2Int> roomPositions)
    {
        Vector2Int spawnPosition = GetSpawnPosition(roomPositions);
        player.transform.position = new Vector3Int(spawnPosition.x, spawnPosition.y, 0);      
    }

    private void SpawnStairs(HashSet<Vector2Int> roomPositions)
    {
        Vector2Int spawnPosition = GetSpawnPosition(roomPositions);
        stairs.transform.position = new Vector3Int(spawnPosition.x, spawnPosition.y, 0);    
    }

    private void SpawnChests(HashSet<Vector2Int> roomPositions)
    {

        for (int i = 0; i < GameManager.Instance.chestNumber; i++)
        {
            Vector2Int spawnPosition = GetSpawnPosition(roomPositions);
            Instantiate(chestPrefab, new Vector3Int(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
        }

    }

    private void SpawnEnemies(HashSet<Vector2Int> roomPositions, int roomAmount)
    {   
        int enemyAmount = GameManager.Instance.enemyAmountPerRoom * roomAmount;
        int extraEnemies = GameManager.Instance.extraEnemies;

        List<GameObject> enemyPrefabs = new List<GameObject>()
        {
            mimicPrefab,
            demonPrefab,
            bansheePrefab,
            skeletonPrefab,
            slimePrefab,
        };

        List<float> enemyProbabilities = new List<float>()
        {
            GameManager.Instance.mimicSpawnWeight,
            GameManager.Instance.demonSpawnWeight,
            GameManager.Instance.bansheeSpawnWeight,
            GameManager.Instance.skeletonSpawnWeight,
            GameManager.Instance.slimeSpawnWeight,
        };

        GameObject enemy = slimePrefab; //slime es el enemigo default, esto solo debería saltar para el valor aleatorio 1

        for (int i = 0; i < enemyAmount + extraEnemies; i++)
        {
            float randomValue = Random.value;
            float cumulativeProbability = 0;

            for (int j = 0; j < enemyProbabilities.Count; j++)
            {
                cumulativeProbability += enemyProbabilities[j];
                if (randomValue < cumulativeProbability)
                {
                    enemy = enemyPrefabs[j];
                    break;
                }
            }

            Vector2Int chosenPosition = SpawnEnemy(roomPositions, enemy);
        }

    }

    private Vector2Int SpawnEnemy(HashSet<Vector2Int> roomPositions, GameObject enemy)
    {
        bool isFarFromPlayer = false;
        Vector2Int spawnPosition = new Vector2Int();

        while (!isFarFromPlayer)
        {
            spawnPosition = GetSpawnPosition(roomPositions);

            float distanceToPlayer = Vector3.Distance(player.transform.position, (Vector3Int)spawnPosition);
            if (distanceToPlayer > 8f)
            {
                isFarFromPlayer = true;
            }
        }

        Instantiate(enemy, new Vector3Int(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
        return spawnPosition;
    }

    private Vector2Int GetSpawnPosition(HashSet<Vector2Int> roomPositions)
    {
        Vector2Int spawnPosition = new Vector2Int();
        List<Vector2Int> listPositions = new List<Vector2Int>(roomPositions);
        bool isValidSpawn = false;

        while (!isValidSpawn && listPositions.Count != 0)
        {
            int randomIndex = Random.Range(0, listPositions.Count);
            Vector2Int pos = listPositions[randomIndex];

            isValidSpawn = CheckIfSpawnPossible(pos, roomPositions);
            if (isValidSpawn)
                spawnPosition = pos;

            listPositions.RemoveAt(randomIndex);
        }

        invalidSpawnPositions.Add(spawnPosition); //algo se va a generar en esta casilla por lo que la marcamos como inválida para futuros spawns
        return spawnPosition;
    }

    private bool CheckIfSpawnPossible(Vector2Int pos, HashSet<Vector2Int> roomPositions)
    {
        if ((invalidSpawnPositions != null) && invalidSpawnPositions.Contains(pos))
            return false;

        bool spawnPossible = true;

        foreach (var dir in Direction2D.cardinalDirectionsList)
        {
            var adjacentPosition = pos + dir;
            if (!roomPositions.Contains(adjacentPosition))
            {
                spawnPossible = false;
            }
        }

        return spawnPossible;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> validRoomPositions)
    {
        Vector2Int currentPosition = startingPosition;
        validRoomPositions.Add(currentPosition);

        for (int i = 0; i < corridorNumber; i++)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgorithms.CorridorRandomWalk(currentPosition, corridorLenght);
            currentPosition = corridor[corridor.Count -1];
            validRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }

    private HashSet<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {

        HashSet<Vector2Int> deadEnds = new HashSet<Vector2Int>();
        foreach (var pos in floorPositions)
        {
            int neighbours = 0;

            foreach (var dir in Direction2D.cardinalDirectionsList) {
                var neighbourTile = pos + dir;
                if (floorPositions.Contains(neighbourTile))
                {
                    neighbours += 1;
                }
            }

            if (neighbours == 1)
                deadEnds.Add(pos);

        }

        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> validRoomPositions, HashSet<Vector2Int> deadEnds)
    {
        HashSet<Vector2Int> roomPositionsNotDeadEnds = new HashSet<Vector2Int>(); //los callejones sin salida siempre deben generar sala
        HashSet<Vector2Int> roomFloors = new HashSet<Vector2Int>();
        int maxRooms = Mathf.RoundToInt(validRoomPositions.Count * roomAmountPercent) - deadEnds.Count;

        foreach (Vector2Int pos in validRoomPositions)
        {
            float randomNumber = Random.value;
            if (((randomNumber <= roomCreationChance) && (roomPositionsNotDeadEnds.Count < maxRooms)) || deadEnds.Contains(pos))
            {
                if (!deadEnds.Contains(pos))
                {
                    roomPositionsNotDeadEnds.Add(pos);
                }

                HashSet<Vector2Int> roomFloor;

                float isWeird = Random.value;
                if (isWeird <= weirdRoomChance)
                {
                    roomFloor = RunRandomWalk(weirdLargeRoomParameters, pos);
                } else
                {
                    roomFloor = RunRandomWalk(smallRoomParameters, pos);
                }
                
                roomFloors.UnionWith(roomFloor);
            }
        }

        return roomFloors;
    }
}
