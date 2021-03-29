using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWSAnimEvents : MonoBehaviour
{
    private PlayerWithSword player;
    private PlayerSFXManager mySFXManager;
    void Start()
    {
        player = GetComponentInParent<PlayerWithSword>();
        mySFXManager = PlayerSFXManager.instance;
    }

    void AE_footstep()
    {
        mySFXManager.PlaySFX("Footstep");
    }

    void AE_runStop()
    {
        mySFXManager.PlaySFX("Run Stop");
    }

    void AE_Jump()
    {
        mySFXManager.PlaySFX("Jump");
    }

    void AE_swordAttack()
    {
        mySFXManager.PlaySFX("Sword Slash");
    }

}
