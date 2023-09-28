using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheeBattle : AbstractEnemyBattle
{
    [SerializeField]
    private int removedMP;
    [SerializeField]
    private float bonusDamage;
    [SerializeField]
    private float bonusDamageHPThreshold;
    private int baseStrenght;


    public override void Start()
    {
        base.Start();
        baseStrenght = strenght;
    }

    public override string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        if (damageTaken < 0)
            damageTaken = 1;
        currentHP = currentHP - damageTaken;
        DamageUpdate();
        return "Atacas a la Banshee\n¡La Banshee recibe " + damageTaken + " puntos de daño!";
    }

    public override string SpecialAttack(int damage)
    {
        int damageTaken = BattleManager.Instance.playerStats.TakeDamage(damage);
        BattleManager.Instance.playerStats.currentMP -= removedMP;
        return "Susurro: recibes " + damageTaken + " puntos de daño y \n tus MP se reducen en "
            + removedMP + " puntos";
    }

    void DamageUpdate()
    {
        float playerHp = (float) BattleManager.Instance.playerStats.currentHP / BattleManager.Instance.playerStats.maxHP;

        Debug.Log(playerHp);
        Debug.Log(bonusDamageHPThreshold);

        if (playerHp <= bonusDamageHPThreshold)
        {
            strenght = Mathf.RoundToInt(baseStrenght * bonusDamage);
        } else
        {
            strenght = baseStrenght;
        }
    }
}
