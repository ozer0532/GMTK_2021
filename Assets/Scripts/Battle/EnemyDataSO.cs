using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "GameDev/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public int health;
    public int damage;
    public PowerRuneSO enemyPowerRune;
    public ElementalRuneSO enemyElementRune;
    public TypeRuneSO enemyTypeRune;
    //List of weakness
    public List<EnemyWeakness> weaknessList;
}
