using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Battle_Manager : MonoBehaviour
{
    //Correct Type, Enemy Weakness
    //Default enemy selalu weak untuk potent
    public List<EnemyDataSO> listData;
    public EnemyDataSO enemyData;
    //Mengacu ke inventory equipped player
    public RuneInventory runeInvent;
    public Bar healthBar;
    public Bar manaBar;
    public Bar enemyHealthBar;
    //TODO:Enemy damage
    public GameObject enemyObject;
    public Animator anim;
    int indeksEnemy;
    int indeksWeakness;

    //Cast Button
    public GameObject winScreen;
    public GameObject loseScreen;
    public UnityEngine.UI.Button castButton;
    public UnityEvent onCast;
    public UnityEvent onHit;

    private bool attacking;

    private void Start()
    {
        Debug.Log("Total enemy:" + listData.Count);
        indeksEnemy = 0;
        enemyData = listData[0];
        initRound();
    }

    private void Update()
    {
        if (runeInvent.GetElementalRune() != null && !attacking)
        {
            castButton.interactable = true;
        }
        else
        {
            castButton.interactable = false;
        }
    }

    public void initRound()
    {
        Debug.Log(indeksEnemy + " Masuk sini");

        if (enemyData is NextStageSO)
        {
            runeInvent.runesList = ((NextStageSO)enemyData).availableRunes;
            runeInvent.UpdateInventory();
            initStage();

            indeksEnemy++;
            enemyData = listData[indeksEnemy];
        }

        enemyHealthBar.maxBar = enemyData.health;
        enemyHealthBar.currentBar = enemyData.health;
        indeksWeakness = Random.Range(0, enemyData.weaknessList.Count);
        EnemyWeakness currentWeakness = enemyData.weaknessList[indeksWeakness];
        enemyData.enemyTypeRune = currentWeakness.enemyTypeRune;
        enemyData.enemyElementRune = currentWeakness.enemyElementRune;

        if (currentWeakness.prefab) changeEnemyPrefab(currentWeakness.prefab);
        else changeEnemyPrefab(enemyData.enemyPrefab);

        runeInvent.unequipRune();
        attacking = false;
    }

    public void changeEnemyPrefab(GameObject prefab)
    {
        Destroy(enemyObject);
        enemyObject = Instantiate(prefab);
        anim = enemyObject.GetComponent<Animator>();
        anim.Play("Alive");
    }

    public void initStage()
    {
        healthBar.currentBar = healthBar.maxBar;    // Full heal
        manaBar.currentBar = manaBar.maxBar;
    }

    //Fungsi dipanggil saat button pressed
    public void whenClicked()
    {
        if (!attacking)
        {
            attacking = true;

            onCast.Invoke();

            Instantiate(runeInvent.GetAttackAnimPrefab()).GetComponent<AttackAnimationController>().manager = this;
        }
    }

    //Return integer damage 0 no damage, 1 ok damage, 2 full damage with potent rune
    public int compareTo()
    {
        //Digabungkan 2 elemen
        ElementalRuneSO currentPlayerElement = runeInvent.GetElementalRune();
        if (currentPlayerElement != null)
        {
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

            //Debug.Log("Weakness:" + enemyData.enemyTypeRune + enemyData.enemyElementRune);

            //Hit
            if (runeInvent.GetTypeRune() == enemyData.enemyTypeRune && currentPlayerElement == enemyData.enemyElementRune)
            {
                //Change weakness
                indeksWeakness = Random.Range(0,enemyData.weaknessList.Count);
                EnemyWeakness currentWeakness = enemyData.weaknessList[indeksWeakness];
                enemyData.enemyTypeRune = currentWeakness.enemyTypeRune;
                enemyData.enemyElementRune = currentWeakness.enemyElementRune;
                Debug.Log("Ganti weakness menjadi" + enemyData.enemyTypeRune + enemyData.enemyElementRune);
                if (currentWeakness.prefab) changeEnemyPrefab(currentWeakness.prefab);

                //Formula damage
                if (runeInvent.powerRune == enemyData.enemyPowerRune)
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
            return (0);
        }
    }

    public void AttackEnd()
    {
        manaBar.reduceBar(10);

        int damage = compareTo();
        enemyHealthBar.reduceBar(-1 * damage);
        if (damage > 0)
        {
            anim.Play("Hit");
        }
        else
        {
            anim.Play("Block");
        }
        if (enemyHealthBar.currentBar == 0)
        {
            //Akses elemen enemy selanjutnya
            if (indeksEnemy != listData.Count - 1)
            {
                indeksEnemy++;
                enemyData = listData[indeksEnemy];
                Invoke(nameof(initRound), 1f);  // Ubah delay mati disini
            }
            else
            {
                Debug.Log("Menang!!");
                winScreen.SetActive(true);
            }
            anim.Play("Dead");
            Debug.Log("Duar musuh mati!");
        }
        else
        {
            Invoke(nameof(EnemyAttack), 1f);
        }
    }

    private void EnemyAttack()
    {
        healthBar.reduceBar(-1 * enemyData.damage);
        onHit.Invoke();
        if (healthBar.currentBar == 0)
        {
            Debug.Log("Duar player mati!");
            loseScreen.SetActive(true);
        }

        runeInvent.unequipRune();
        attacking = false;
    }
}
