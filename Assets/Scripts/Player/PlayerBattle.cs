using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MagicEffect { BASE, LIFESTEAL, WEAKEN, POISON, EXPLOSION }
public class PlayerBattle : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP = 100;
    public int maxMP = 10;
    public int currentMP = 10;
    public int intelligence = 5;
    public int defense = 5;
    public int strenght = 30;
    public int gold = 0;
    public int magicAttackCostMP = 5;
    public bool canUseMagic = true;
    public int numberOfUpgrades;

    public MagicEffect magicEffect = MagicEffect.BASE;
    public int lifestealLevel = 0;
    public int weakenLevel = 0;
    public int poisonLevel = 0;
    public int explosionLevel = 0;

    public int TakeDamage(int strenght)
    {
        int damageTaken = Mathf.RoundToInt(strenght - (0.5f * defense));
        currentHP = currentHP - damageTaken;
        return damageTaken;
    }

    public void TakeDamageIgnoringDefense(int damageTaken)
    {
        currentHP = currentHP - damageTaken;
        if (currentHP <= 0)
            currentHP = 1;
    }

    public void RecoverHP(int amount)
    {
        currentHP = currentHP + amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void RecoverMP(int amount)
    {
        currentMP = currentMP + amount;
        if (currentMP > maxMP)
            currentMP = maxMP;
    }

    public void GetGold(int amount)
    {
        gold = gold + amount;
    }

    public void LoseGold(int amount)
    {
        gold = gold - amount;
        if (gold < 0)
            gold = 0;
    }

    public void LoseMP(int amount)
    {
        currentMP = currentMP - amount;
        if (currentMP < 0)
            currentMP = 0;
    }

}
