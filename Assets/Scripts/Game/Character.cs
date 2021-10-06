using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Clase principal de unidades, se puede heredar para Minions, Players, Dragons, etc.
/// </summary>
public class Character : MonoBehaviour
{
    public static Action<Character> OnCharacterCreatedEvent;

    public enum ObjetiveType
    {
        Self,
        Team,
        Enemy,
    }

    public bool IsDead => statSystem.IsDeath;

    [Header("Rpg Stats")]
    public string characterName;
    [Range(1,18)]
    public int level = 1;
    public StatSystem statSystem;
    [Header("Team")]
    public ObjetiveType objetive;
    [Header("HUD")]
    public GameObject teamHUD;
    public GameObject enemyHUD;

    private void Start()
    {
        OnCharacterCreatedEvent?.Invoke(this);

        statSystem.Init();
    }
    private void Update()
    {
        statSystem.UpdateTick();
    }
    public GameObject GetHUD()
    {
        if (objetive == ObjetiveType.Team)
            return teamHUD;
        else
            return enemyHUD;
    }

}
