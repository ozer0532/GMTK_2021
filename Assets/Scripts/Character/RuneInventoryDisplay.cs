using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneInventoryDisplay : MonoBehaviour, IPointerClickHandler
{
    public int inventoryIndex;
    public RuneInventory inventory;
    public AudioClip clip;
    public Image colorImage;

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

            colorImage.color = rune.color;
        }
        else
        {
            image.enabled = false;
            colorImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Zro.Audio.GameAudioPlayer.PlayOneShot(clip);
        if (inventory) inventory.EquipRune(rune);
    }
}
