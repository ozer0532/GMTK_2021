using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{
    public Battle_Manager manager;
    public AudioClip hitClip;

    public void AttackEnd()
    {
        Zro.Audio.GameAudioPlayer.PlayOneShot(hitClip);
        manager.AttackEnd();
        Destroy(gameObject);
    }

    public void AttackHit()
    {
        Zro.Audio.GameAudioPlayer.PlayOneShot(hitClip);
        manager.AttackEnd();
    }

    public void ProjectileDestroy()
    {
        Destroy(gameObject);
    }
}
