using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattle : AbstractEnemyBattle
{

    public float reflectedDamagePercent;
    public int defenseReduction;

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        int damageReflected = Mathf.RoundToInt(damageTaken * reflectedDamagePercent);
        if (damageTaken > currentHP)
        {
            damageReflected = Mathf.RoundToInt(currentHP * reflectedDamagePercent);
        } 
        
        BattleController.Instance.playerStats.TakeDamageIgnoringDefense(damageReflected);
        currentHP = currentHP - damageTaken;
        return "Atacas al Slime\n�El slime recibe " + damageTaken + " puntos de da�o!\n�El gelatinoso Slime te devuelve el impacto \n y recibes " + damageReflected + " puntos de da�o!";
    }

    public override string SpecialAttack(int damage)
    {
        BattleController.Instance.playerStats.defense -= defenseReduction;
        int damageTaken = BattleController.Instance.playerStats.TakeDamage(damage);
        return "�cido: recibes " + damageTaken + " puntos de da�o y \n tu defensa temporalmente baja en "
            + defenseReduction + " puntos";
    }

}
