using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheeBattle : AbstractEnemyBattle
{
    public int removedMP;
    public int bonusDamage;
    public float bonusDamageHPThreshold;
    public int baseStrenght;


    public override void Start()
    {
        base.Start();
        baseStrenght = strenght;
    }

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        currentHP = currentHP - damageTaken;
        DamageUpdate();
        return "Atacas a la Banshee\n¡La Banshee recibe " + damageTaken + " puntos de daño!";
    }

    public override string SpecialAttack(int damage)
    {
        int damageTaken = BattleController.Instance.playerStats.TakeDamage(damage);
        BattleController.Instance.playerStats.currentMP -= removedMP;
        return "Susurro: recibes " + damageTaken + " puntos de daño y \n tus MP se reducen en "
            + removedMP + " puntos";
    }

    void DamageUpdate()
    {
        float playerHp = (float) BattleController.Instance.playerStats.currentHP / BattleController.Instance.playerStats.maxHP;

        Debug.Log(playerHp);
        Debug.Log(bonusDamageHPThreshold);

        if (playerHp <= bonusDamageHPThreshold)
        {
            strenght = Mathf.RoundToInt(baseStrenght*1.5f);
        } else
        {
            strenght = baseStrenght;
        }
    }
}
