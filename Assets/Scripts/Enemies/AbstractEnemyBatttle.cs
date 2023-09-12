using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;

public abstract class AbstractEnemyBattle : MonoBehaviour
{

    public EnemyStatsSO baseStats;
    public int maxHP;
    public int currentHP;
    public int defense;
    public int strenght;
    public int level;
    public int goldDrop;
    public bool isPoisoned;

    protected Animator animator;
    protected Rigidbody2D rigidBody;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    public virtual void Start()
    {
        goldDrop = baseStats.goldDrop;
        UpdateStats(0);
    }

    public void UpdateStats(int addedLevels)
    {
        level = DifficultyController.Instance.enemyLevel + addedLevels;
        maxHP = baseStats.maxHP + 2*level;
        currentHP = maxHP;
        defense = baseStats.defense + level;
        strenght = baseStats.strenght + level;
    }

    public void updateDropGold(int closeEnemies)
    {
        goldDrop += 5 * closeEnemies;
    }

    public virtual string TakeDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.5f * defense));
        currentHP = currentHP - damageTaken;
        return "El enemigo recibe " + damageTaken.ToString();
    }

    public virtual string TakePoisonDamage()
    {
        int damageTaken = 5 + (5*BattleController.Instance.playerStats.poisonLevel);
        currentHP = currentHP - damageTaken;
        return "¡El veneno inflinge " + damageTaken + " puntos de daño al enemigo!";
    }

    public virtual string TakeMagicDamage(int damage)
    {
        int damageTaken = Mathf.RoundToInt(damage - (0.01f * defense));

        if (BattleController.Instance.playerStats.magicEffect == MagicEffect.LIFESTEAL)
        {
            currentHP = currentHP - damageTaken;
            float recoveryPercentage = 0.45f + (0.05f * BattleController.Instance.playerStats.lifestealLevel);
            int recoveredHP = Mathf.RoundToInt(damageTaken*recoveryPercentage);
            BattleController.Instance.playerStats.RecoverHP(recoveredHP);
            return "¡El ataque mágico inflinge " + damageTaken + " puntos de daño al enemigo y recuperas " + recoveredHP + " puntos de vida!";
        }

        if (BattleController.Instance.playerStats.magicEffect == MagicEffect.WEAKEN)
        {
            currentHP = currentHP - damageTaken;
            float defenseReductionPercent = 0.80f - (0.05f * BattleController.Instance.playerStats.weakenLevel);
            defense = Mathf.RoundToInt(defense * defenseReductionPercent);
            return "¡El ataque mágico inflinge " + damageTaken + " puntos de daño al enemigo y su defensa baja en un " + (1 - defenseReductionPercent)*100 + "%!";
        }

        if (BattleController.Instance.playerStats.magicEffect == MagicEffect.POISON)
        {
            currentHP = currentHP - damageTaken;
            isPoisoned = true;
            return "¡El ataque mágico inflinge " + damageTaken + " puntos de daño al enemigo, y ahora está envenenado!";
        }

        if (BattleController.Instance.playerStats.magicEffect == MagicEffect.EXPLOSION)
        {
            damageTaken = damageTaken * (BattleController.Instance.playerStats.explosionLevel + 1);
            int damageTakenPlayer = Mathf.RoundToInt(BattleController.Instance.playerStats.maxHP / 4);
            BattleController.Instance.playerStats.TakeDamageIgnoringDefense(damageTaken);
            currentHP = currentHP - damageTaken;
            return "¡El ataque mágico inflinge " + damageTaken + " puntos de daño al enemigo! Pero te haces " + damageTakenPlayer + " puntos de daño a tí mismo";
        }

        currentHP = currentHP - damageTaken;
        return "¡El ataque mágico inflinge " + damageTaken + " puntos de daño al enemigo!";
    }

    public virtual string SpecialAttack(int damage)
    {
        return "";
    }

}
