using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{
    public Battle_Manager manager;
    public void AttackEnd()
    {
        manager.AttackEnd();
        Destroy(gameObject);
    }

    public void AttackHit()
    {
        manager.AttackEnd();
    }

    public void ProjectileDestroy()
    {
        Destroy(gameObject);
    }
}
