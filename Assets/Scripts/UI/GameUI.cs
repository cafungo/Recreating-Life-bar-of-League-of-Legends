using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private static GameUI instance;
    public static GameUI Instance => instance;
    [Header("Content")]
    public Transform hudContent;

    private void Awake()
    {
        instance = this;
        Character.OnCharacterCreatedEvent += OnCharacterCreated;
    }

    private void OnDisable()
    {
        Character.OnCharacterCreatedEvent -= OnCharacterCreated;
    }
    void OnCharacterCreated(Character unit)
    {
        var hud = Instantiate(unit.GetHUD(),hudContent).GetComponent<UnitHUD>();
        hud.Init(unit);
    }
}
