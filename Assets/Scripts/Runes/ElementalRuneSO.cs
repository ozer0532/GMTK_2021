using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element Rune", menuName = "GameDev/Runes/Element")]
public class ElementalRuneSO : RuneBaseSO
{
    public bool isBaseElement = true;
    public ElementalRuneSO baseElement1;
    public ElementalRuneSO baseElement2;
    public ElementalRuneSO[] effective;
    public ElementalRuneSO[] ineffective;
}
