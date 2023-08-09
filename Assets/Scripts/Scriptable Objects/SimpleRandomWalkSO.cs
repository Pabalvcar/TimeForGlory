using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="SimpleRandomWalkParameters_", menuName = "Generador Procedural/DatosSimpleRandomWalk")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterations = 10, walkLenght = 10;
    public bool startIterationOnRandomTile = true;
}
