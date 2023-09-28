using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattle : AbstractEnemyBattle
{

    [SerializeField]
    private float reflectedDamagePercent;
    [SerializeField]
    private int defenseReduction;

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        if (damageTaken < 0)
            damageTaken = 1;
        int damageReflected = Mathf.RoundToInt(damageTaken * reflectedDamagePercent);
        if (damageTaken > currentHP)
        {
            damageReflected = Mathf.RoundToInt(currentHP * reflectedDamagePercent);
        } 
        
        BattleManager.Instance.playerStats.TakeDamageIgnoringDefense(damageReflected);
        currentHP = currentHP - damageTaken;
        return "Atacas al Slime\n¡El slime recibe " + damageTaken + " puntos de daño!\n¡El gelatinoso Slime te devuelve el impacto \n y recibes " + damageReflected + " puntos de daño!";
    }

    public override string SpecialAttack(int damage)
    {
        BattleManager.Instance.playerStats.defense -= defenseReduction;
        int damageTaken = BattleManager.Instance.playerStats.TakeDamage(damage);
        return "Ácido: recibes " + damageTaken + " puntos de daño y \n tu defensa temporalmente baja en "
            + defenseReduction + " puntos";
    }

}
