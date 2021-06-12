using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneInventoryDisplay : MonoBehaviour, IPointerClickHandler
{
    public int inventoryIndex;
    public RuneInventory inventory;

    private Image image;
    private RuneBaseSO rune;

    private void OnEnable()
    {
        image = GetComponent<Image>();
        UpdateDisplay();
        if (inventory) inventory.onInventoryChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        if (inventory) inventory.onInventoryChanged -= UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        rune = null;
        if (inventory.runesList.Count > inventoryIndex) 
            rune = inventory.runesList[inventoryIndex];

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
        if (inventory) print(inventory.EquipRune(rune));
    }
}
