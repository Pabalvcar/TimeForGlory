using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicBattle : AbstractEnemyBattle
{

    [SerializeField]
    private int goldStolen;
    [SerializeField]
    private int extraDamageForEachRune;

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        if (damageTaken < 0)
            damageTaken = 1;
        currentHP = currentHP - damageTaken;
        BattleManager.Instance.playerStats.LoseGold(goldStolen);
        return "Atacas al Mimic\n¡El Mimic recibe " + damageTaken + " puntos de daño!\n Al acercarte, el Mimic te roba " + goldStolen + " monedas de oro";
    }

    public override string SpecialAttack(int damage)
    {
        int runeNumber = BattleManager.Instance.playerStats.numberOfUpgrades;
        int damageTaken = BattleManager.Instance.playerStats.TakeDamage(damage + (runeNumber* extraDamageForEachRune));
        return "Codicia: El Mimic castiga tu codicia \n Al tener " + runeNumber + " runas, recibes " + damageTaken + " puntos de daño adicionales";
    }

}
