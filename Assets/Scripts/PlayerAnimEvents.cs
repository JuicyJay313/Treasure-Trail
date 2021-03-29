using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player;
    private PlayerSFXManager mySFXManager;
    void Start()
    {
        player = GetComponentInParent<Player>();
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

}
