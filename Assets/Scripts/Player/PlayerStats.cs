using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP = 100;
    public int maxMP = 10;
    public int currentMP = 10;
    public int intelligence = 1;
    public int defense = 1;
    public int strenght = 1;
    public int gold = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
