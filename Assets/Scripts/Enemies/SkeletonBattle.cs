using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattle : AbstractEnemyBattle
{
    public float basicDamageReductionPercent;
    public float lifestealPercent;

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        damageTaken = Mathf.RoundToInt(damageTaken * (1 - basicDamageReductionPercent));
        currentHP = currentHP - damageTaken;
        return "Atacas al Skeleton\n¡El robusto Skeleton reduce la fuerza del ataque \n y recibió " + damageTaken + " puntos de daño!";
    }

    public override string SpecialAttack(int damage)
    {
        int damageDealt = BattleController.Instance.playerStats.TakeDamage(damage);
        currentHP = currentHP + Mathf.RoundToInt(damage*lifestealPercent);
        if (currentHP > maxHP)
            currentHP = maxHP;
        return "Festín: recibes " + damageDealt + " puntos de daño y \n Skeleton recupera "
            + Mathf.RoundToInt(damage * lifestealPercent) + " puntos de daño";
    }

}
