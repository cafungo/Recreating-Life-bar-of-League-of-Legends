using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Clase para controlar el Health HUD de las unidades
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UnitHUD : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Character unit;
    [Header("Data")]
    public Vector2 offSet;
    [Range(0.1f,1.0f)]
    public float fadeTransitionSpeed = 1f;                  //Rapidez en bajar la barra
    Renderer unitRenderer;
    [Header("UI Elements")]
    public TextMeshProUGUI levelLabel;
    public TextMeshProUGUI nameLabel;
    public Slider healthSlider;
    public Slider hitSlider;
    public Slider manaSlider;
    public Slider shieldSlider;
    [Header("Divisor")]
    public RectTransform divisorContent;
    public PoolUtility oneHundredPool = new PoolUtility();
    public PoolUtility oneThousandPool = new PoolUtility();
    public bool IsOpen => canvasGroup.alpha == 1.0f;

    private int maxHealthRecord;

    private float fadeAmount;

    private float divisorWidth;
    private float divisorHalfWidth;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        divisorWidth = divisorContent.rect.width;
        divisorHalfWidth = divisorWidth / 2;
    }

    private void Update()
    {
        if (unit == null)
            return;

        FollowUnit();

        if (unit.IsDead || !IsVisibleOnScreen())
        {
            if (IsOpen)
                Close();
        }else if (!unit.IsDead)
        {
            if (!IsOpen)
                Open();   
        }

        levelLabel.text = unit.level.ToString();
        //Compruebo si se han cambiado los valores de la salud máxima
        bool healthOverload = HealthOverload(out int overValue);
        if (maxHealthRecord!=unit.statSystem.stats.maxHealth && !healthOverload)
        {
            ChangeMaxHealth();
        }else if(healthOverload && overValue != maxHealthRecord)
        {
            ChangeMaxHealth();
        }
        //Update Mask of lines
        if(!healthOverload && (healthSlider.value!=unit.statSystem.currentHealth || 
            shieldSlider.value!=overValue))
        {
            UpdateMask();
        }

        healthSlider.value = unit.statSystem.currentHealth;
        shieldSlider.value = overValue;
        manaSlider.value = unit.statSystem.currentMana;
        //Hit slider effect
        if (unit.statSystem.currentHealth < hitSlider.value)
        {
            fadeAmount += Time.deltaTime * ((hitSlider.maxValue / 5) * fadeTransitionSpeed);
            hitSlider.value -= fadeAmount;
        } else
        {
            hitSlider.value = unit.statSystem.currentHealth;
            fadeAmount = 0;
        }


    }
    public virtual void Init(Character _unit)
    {
        unit = _unit;
        unitRenderer = unit.GetComponentInChildren<Renderer>();
        nameLabel.text = unit.characterName;

        ChangeMaxHealth();
        ChangeMaxMana();
    }
    public void Open()
    {
        canvasGroup.alpha = 1.0f;
    }

    public void Close()
    {
        canvasGroup.alpha = 0.0f;
    }
    void ChangeMaxHealth()
    {
        bool inOverload = HealthOverload(out int overValue);
        if (inOverload)
            healthSlider.maxValue = overValue;
         else
            healthSlider.maxValue = unit.statSystem.stats.maxHealth;
        //Actualizo los valores maximo
        hitSlider.maxValue = unit.statSystem.stats.maxHealth;
        maxHealthRecord = (int)healthSlider.maxValue;
        shieldSlider.maxValue = (inOverload)? overValue : unit.statSystem.stats.maxHealth;
        //Imprimo los divisores
        PrintLifeDivisors();
        //Actualizo la mascara de divisores
        UpdateMask();
    }
    void ChangeMaxMana()
    {
        manaSlider.maxValue = unit.statSystem.stats.maxMana;
    }
    Vector2 GetScreenPosition(Vector3 worldPos)
    {
        return Camera.main.WorldToScreenPoint(worldPos);
    }

    void FollowUnit()
    {
        Vector2 pos = GetScreenPosition(unit.transform.position + new Vector3(0f,2.2f,0f));
        pos += offSet;
        transform.position = pos;
    }

    bool IsVisibleOnScreen()
    {
        return unitRenderer.isVisible;
    }

    public Vector2 WidthToVector(float widthValue)
    {
        return new Vector2(widthValue - divisorHalfWidth, 0);
    }

    public Vector2 ValueToRectPosition(float value, float maxValue, float widthValue, float halfWidthValue)
    {
        float percentage = (value * 100) / maxValue;
        float widthLocation = (widthValue * percentage) / 100;
        Vector2 position = new Vector2(widthLocation - halfWidthValue, 0f);
        return position;
    }

    public void PrintLifeDivisors()
    {
        //Desactivo todos los anteriores
        for(int i = 0; i < divisorContent.childCount; i++)
        {
            divisorContent.GetChild(i).gameObject.SetActive(false);
        }
        
        float amount = 0;
        while (amount < maxHealthRecord)
        {
            amount += 100;
            if (amount > maxHealthRecord)
                continue;

            Vector2 position = ValueToRectPosition(amount, maxHealthRecord, divisorWidth, divisorHalfWidth);
            var obj = (amount%1000==0)? oneThousandPool.GetObject(divisorContent.transform) : oneHundredPool.GetObject(divisorContent.transform);
            obj.transform.localPosition = position;
        }
    }

    public bool HealthOverload(out int value)
    {
        value = unit.statSystem.currentHealth + unit.statSystem.currentShield;
        return value > unit.statSystem.stats.maxHealth;
    }
    void UpdateMask()
    {
        //Coloco el fill amount de la mascara segun el valor del maxHealthRecord
        var mask = divisorContent.GetComponent<Image>();

        float health = unit.statSystem.currentHealth + unit.statSystem.currentShield;
        float maxHealth = unit.statSystem.stats.maxHealth;
        mask.fillAmount = health / maxHealth;
    }

}
