using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RuneInventory : MonoBehaviour
{
    public PowerRuneSO powerRune;
    public ElementalRuneSO elementRune1;
    public ElementalRuneSO elementRune2;
    public TypeRuneSO typeRune;

    public List<RuneBaseSO> runesList = new List<RuneBaseSO>();

    public List<ElementalRuneSO> elementsList = new List<ElementalRuneSO>();

    public UnityAction onEquipmentChanged;
    public UnityAction onInventoryChanged;

    private void OnEnable()
    {
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        if (onEquipmentChanged != null) onEquipmentChanged();
        if (onInventoryChanged != null) onInventoryChanged();
    }

    /// <summary>
    /// Equips a rune in the inventory into an empty slot
    /// </summary>
    /// <param name="rune">The rune to equip</param>
    /// <returns>True if and only if the rune is equipped successfully</returns>
    public bool EquipRune(RuneBaseSO rune)
    {
        // Find rune in runes list
        if (!runesList.Find(e => e == rune))
        {
            return false;
        }

        // Check if slot is empty & insert if true
        if (rune is PowerRuneSO)
        {
            if (powerRune) return false;
            powerRune = rune as PowerRuneSO;
        }
        else if (rune is TypeRuneSO)
        {
            if (typeRune) return false;
            typeRune = rune as TypeRuneSO;
        }
        else
        {
            if (elementRune1)
            {
                if (elementRune2) return false;
                if (elementRune1 == rune) return false;
                elementRune2 = rune as ElementalRuneSO;
            }
            else
            {
                if (elementRune2 == rune) return false;
                elementRune1 = rune as ElementalRuneSO;
            }
        }

        UpdateInventory();
        return true;
    }

    //Unequip all after turn
    public void unequipRune() 
    {
        powerRune = null;
        elementRune1 = null;
        elementRune2 = null;
        typeRune = null;
        UpdateInventory();
    }

    public int CalculateDamage()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets the combination between two elements
    /// </summary>
    /// <param name="rune1">The first combined element</param>
    /// <param name="rune2">The first combined element</param>
    /// <returns>The resulting element from the combination</returns>
    public ElementalRuneSO GetElementCombination(ElementalRuneSO rune1, ElementalRuneSO rune2)
    {
        if (rune2 == null) return rune1;
        if (rune1 == null) return rune2;

        return (from element in elementsList
               where !element.isBaseElement
               where (rune1 == element.baseElement1 && rune2 == element.baseElement2)
               || (rune1 == element.baseElement2 && rune2 == element.baseElement1)
               select element).FirstOrDefault();
    }

    public TypeRuneSO GetTypeRune() => typeRune;
    public PowerRuneSO GetPowerRune() => powerRune;
    public ElementalRuneSO GetElementalRune() => GetElementCombination(elementRune1, elementRune2);
}
