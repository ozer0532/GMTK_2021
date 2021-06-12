using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneEquipmentDisplay : MonoBehaviour, IPointerClickHandler
{
    public DisplaySlot slot;
    public RuneInventory inventory;

    private Image image;
    private RuneBaseSO rune;

    private void OnEnable()
    {
        image = GetComponent<Image>();
        UpdateDisplay();
        if (inventory) inventory.onEquipmentChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        if (inventory) inventory.onEquipmentChanged -= UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        switch (slot)
        {
            case DisplaySlot.Type:
                rune = inventory.typeRune;
                break;
            case DisplaySlot.Power:
                rune = inventory.powerRune;
                break;
            case DisplaySlot.Element1:
                rune = inventory.elementRune1;
                break;
            case DisplaySlot.Element2:
                rune = inventory.elementRune2;
                break;
            default:
                rune = null;
                break;
        }

        if (rune)
        {
            image.enabled = true;
            image.sprite = rune.sprite;
        }
        else
        {
            image.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        rune = null;

        switch (slot)
        {
            case DisplaySlot.Type:
                inventory.typeRune = null;
                break;
            case DisplaySlot.Power:
                inventory.powerRune = null;
                break;
            case DisplaySlot.Element1:
                inventory.elementRune1 = null;
                break;
            case DisplaySlot.Element2:
                inventory.elementRune2 = null;
                break;
            default:
                break;
        }
        inventory.UpdateInventory();
    }

    public enum DisplaySlot
    {
        Type, Power, Element1, Element2
    }
}
