using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicBattle : AbstractEnemyBattle
{

    public int goldStolen;
    public float extraDamageForEachRune;

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        currentHP = currentHP - damageTaken;
        BattleController.Instance.playerStats.LoseGold(goldStolen);
        return "Atacas al Mimic\n¡El Mimic recibe " + damageTaken + " puntos de daño!\n Al acercarte, el Mimic te roba " + goldStolen + " monedas de oro";
    }

    public override string SpecialAttack(int damage)
    {
        int runeNumber = BattleController.Instance.playerStats.numberOfUpgrades;
        int damageTaken = BattleController.Instance.playerStats.TakeDamage(damage + Mathf.RoundToInt(runeNumber* extraDamageForEachRune));
        return "Codicia: El Mimic castiga tu codicia \n Al tener " + runeNumber + " runas, recibes " + damageTaken + " puntos de daño adicionales";
    }

}
