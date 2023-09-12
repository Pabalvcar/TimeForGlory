using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapGenerator tilemapGenerator = null;

    [SerializeField]
    protected Vector2Int startingPosition = Vector2Int.zero;

    [SerializeField]
    protected GameObject playerPrefab = null;
    protected GameObject player = null;

    [SerializeField]
    protected GameObject stairsPrefab = null;
    protected GameObject stairs = null;

    [SerializeField]
    protected GameObject slimePrefab = null;

    [SerializeField]
    protected GameObject skeletonPrefab = null;

    [SerializeField]
    protected GameObject bansheePrefab = null;

    [SerializeField]
    protected GameObject demonPrefab = null;

    [SerializeField]
    protected GameObject mimicPrefab = null;

    [SerializeField]
    protected GameObject chestPrefab = null;


    public void GenerateDungeon()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<MimicMovement>())
            {
                MimicMovement mimicScript = enemy.GetComponent<MimicMovement>();
                Destroy(mimicScript.chestUIInstance);
            }
            Destroy(enemy);
        }

        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");

        foreach (var chest in chests)
        {
            Chest chestScript = chest.GetComponent<Chest>();
            Destroy(chestScript.chestUIInstance);
            Destroy(chest);
        }

        if (player==null)
            player = Instantiate(playerPrefab, new Vector3Int(0, 0, 0), Quaternion.identity);

        tilemapGenerator.ClearTilemap();

        if (stairs==null)
            stairs = Instantiate(stairsPrefab, new Vector3Int(100, 100, 0), Quaternion.identity);

        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
