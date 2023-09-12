using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName="EnemyStats_", menuName = "Enemigos/EstadisticasBatalla")]
public class EnemyStatsSO : ScriptableObject
{
    public int maxHP;
    public int currentHP;
    public int defense;
    public int strenght;
    public int goldDrop;
    public Sprite battleSprite;
}
