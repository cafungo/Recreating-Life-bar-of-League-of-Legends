using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public enum PortalType
    {
        Damage,
        Health,
        Shield,
        MaxHealth,
        Level,
        ManaRandom
    }

    public PortalType portalType;
    [Range(0.1f,1.0f)]
    public float percentageValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var unit = other.GetComponent<Character>();
            int value = (int)(unit.statSystem.stats.maxHealth * percentageValue);
            switch (portalType)
            {
                case PortalType.Damage:
                    unit.statSystem.TakeDamage(value);
                    break;
                case PortalType.Health:
                    unit.statSystem.AddHealing(value);
                    break;
                case PortalType.Shield:
                    unit.statSystem.AddShield(value);
                    break;
                case PortalType.MaxHealth:
                    unit.statSystem.stats.maxHealth += value;
                    break;
                case PortalType.Level:
                    unit.level++;
                    break;
                case PortalType.ManaRandom:
                    unit.statSystem.AlterMana(Random.Range(-50,50));
                    break;
            }
        }
    }
}
