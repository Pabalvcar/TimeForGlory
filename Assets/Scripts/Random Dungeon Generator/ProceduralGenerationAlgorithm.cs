using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{

    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLenght)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);

        var currentPosition = startPosition;

        for (int i = 0; i < walkLenght; i++)
        {
            currentPosition = currentPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(currentPosition);
        }

        return path;
    }

    public static List<Vector2Int> CorridorRandomWalk(Vector2Int startPosition, int corridorLenght)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();

        corridor.Add(startPosition);

        var currentPosition = startPosition;

        for (int i = 0; i < corridorLenght; i++)
        {
            currentPosition = currentPosition + direction;
            corridor.Add(currentPosition);
        }

        return corridor;
    }
}

public static class Direction2D
{

    // El orden es importante para la comprobación de las paredes
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),  // arriba
        new Vector2Int(1,0),  // derecha
        new Vector2Int(0,-1), // abajo
        new Vector2Int(-1,0)  // izquierda
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1,1),  // arriba derecha
        new Vector2Int(1,-1), // abajo derecha
        new Vector2Int(-1,-1), // abajo izquierda
        new Vector2Int(-1,1) // arriba izquierda
    };

    public static List<Vector2Int> allDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),  // arriba
        new Vector2Int(1,1),  // arriba derecha
        new Vector2Int(1,0),  // derecha
        new Vector2Int(1,-1), // abajo derecha
        new Vector2Int(0,-1), // abajo
        new Vector2Int(-1,-1), // abajo izquierda
        new Vector2Int(-1,0),  // izquierda
        new Vector2Int(-1,1) // arriba izquierda
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
