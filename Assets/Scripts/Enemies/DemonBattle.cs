using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBattle : AbstractEnemyBattle
{

    public float strenghtIncreasePercent;
    public int turnsMagicDisabled;

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        currentHP = currentHP - damageTaken;
        BattleController.Instance.playerStats.canUseMagic = true;
        return "Atacas al Demon\n¡El Demon recibe " + damageTaken + " puntos de daño!";
    }

    public override string SpecialAttack(int damage)
    {
        int damageTaken = BattleController.Instance.playerStats.TakeDamage(Mathf.RoundToInt(damage/2f));
        BattleController.Instance.playerStats.canUseMagic = false;
        return "Magia Negra: recibes " + damageTaken + " puntos de daño y no \n " + 
            "puedes usar magia en tu siguiente turno";
    }

}
