using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="SimpleRandomWalkParameters_", menuName = "Generador Procedural/DatosSimpleRandomWalk")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterations;
    public int walkLenght;
    public bool startIterationOnRandomTile = true;
}
