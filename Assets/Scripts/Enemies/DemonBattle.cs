using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBattle : AbstractEnemyBattle
{
    [SerializeField]
    private int turnsMagicDisabled;

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        if (damageTaken < 0)
            damageTaken = 1;
        currentHP = currentHP - damageTaken;
        BattleManager.Instance.playerStats.canUseMagic = true;
        return "Atacas al Demon\n¡El Demon recibe " + damageTaken + " puntos de daño!";
    }

    public override string SpecialAttack(int damage)
    {
        int damageTaken = BattleManager.Instance.playerStats.TakeDamage(damage);
        BattleManager.Instance.playerStats.canUseMagic = false;
        return "Magia Negra: recibes " + damageTaken + " puntos de daño y no \n " + 
            "puedes usar magia en tu siguiente turno";
    }

}
