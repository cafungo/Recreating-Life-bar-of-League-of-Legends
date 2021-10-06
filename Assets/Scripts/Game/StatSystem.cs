using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Estadisticas básicas de las unidades
/// </summary>
[System.Serializable]
public class StatSystem
{
    public Action OnMaxHealthChangedEvt;
    public Action OnMaxManaChangedEvt;

    [System.Serializable]
    public class Stats
    {
        public int maxHealth;
        public int maxMana;
    }

    public int currentHealth;
    public int currentMana;
    public int currentShield;
    public Stats stats;
    public bool IsDeath => currentHealth <= 0;

    float shieldTimer;
    public void Init()
    {
        currentHealth = stats.maxHealth;
        currentMana = stats.maxMana;
    }
    public void UpdateTick()
    {
        if (shieldTimer > 0.0f)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
                currentShield = 0;
        }
        
        
    }
    public void TakeDamage(int damageValue)
    {
        if (IsDeath)
            return;

        damageValue = Mathf.Abs(damageValue);

        if (currentShield > 0)
        {
            RemoveShield(ref damageValue);

            if (damageValue > 0)
                AlterHealth(-damageValue);
        }else
        {
            AlterHealth(-damageValue);
        }
    }
    public void AddHealing(int healthValue)
    {
        if (IsDeath)
            return;
        AlterHealth(healthValue);
    }
    public void AlterHealth(int value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, stats.maxHealth);
        if (currentHealth <= 0)
            Dead();

    }
    public void AlterMana(int value)
    {
        currentMana = Mathf.Clamp(currentMana + value, 0, stats.maxMana);
    }
    public void AddShield(int shieldValue)
    {
        currentShield += shieldValue;
        shieldTimer = 10.0f;
    }
    public void RemoveShield(ref int damage)
    {
        currentShield -= damage;
        if (currentShield >= 0)
        {
            damage = 0;
        } else
        {
            damage = Mathf.Abs(currentShield);
            currentShield = 0;
        }
    }
    public void Dead()
    {

    }


}
