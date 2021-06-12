using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Manager : MonoBehaviour
{
    //Correct Type, Enemy Weakness
    //Default enemy selalu weak untuk potent
    public PowerRuneSO enemyPowerRune;
    public ElementalRuneSO enemyElementRune;
    public TypeRuneSO enemyTypeRune;
    //Mengacu ke inventory equipped player
    public RuneInventory runeInvent;
    public Bar healthBar;
    public Bar manaBar;
    public Bar enemyHealthBar;
    //TODO:Enemy damage
    public Animator anim;

    //Fungsi dipanggil saat button pressed
    public void whenClicked()
    {
        int damage = compareTo();
        enemyHealthBar.reduceBar(-1 * damage);
        if (enemyHealthBar.currentBar == 0)
        {
            anim.Play("Slime_Dead");
            Debug.Log("Duar musuh mati!");
        }
        //TODO: Damage to player sesuai damage enemy
        healthBar.reduceBar(-1 * 10);
        if (healthBar.currentBar == 0)
        {
            Debug.Log("Duar player mati!");
        }
    }

    //Return integer damage 0 no damage, 1 ok damage, 2 full damage with potent rune
    public int compareTo()
    {
        //Digabungkan 2 elemen
        ElementalRuneSO currentPlayerElement = runeInvent.GetElementalRune();
        if (currentPlayerElement != null)
        {
            //Hit
            if (runeInvent.GetTypeRune() == enemyTypeRune && currentPlayerElement == enemyElementRune)
            {
                //Change weakness
                //TODO: Change Weakness sesuai kombinasi di scriptable object enemy type dan enemy

                //Change player mana
                //Mana if dull minus 5, if potent minus 20
                //Note: reduce bar negatif
                if (runeInvent.GetPowerRune() != null)
                {
                    manaBar.reduceBar(-1 * runeInvent.GetPowerRune().manaCost);
                }
                else 
                {
                    manaBar.reduceBar(-15);
                }

                //Formula damage
                if (runeInvent.powerRune == enemyPowerRune)
                {
                    return (2);
                }
                else
                {
                    return (1);
                }
            }
            else
            {
                return (0);
            }
        }
        //Kombinasi elemen salah
        else
        {
            return (0);
        }
    }
}
