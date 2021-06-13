using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Weaknesses", menuName = "GameDev/Enemy Weaknesses")]
public class EnemyWeakness : ScriptableObject
{
    //Weakness berupa element dan type
    public ElementalRuneSO enemyElementRune;
    public TypeRuneSO enemyTypeRune;
    public GameObject prefab;
}
