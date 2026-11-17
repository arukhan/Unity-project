using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    void UpdateCombat(float dt)
    {
        gunCooldown=Mathf.Max(0,gunCooldown-dt);
        shootTimer=Mathf.Max(0,shootTimer-dt);
        recoilTimer=Mathf.Max(0,recoilTimer-dt);
        if((Press(KeyCode.Space)||Press(KeyCode.Return)) && gunCooldown<=0)
        {
            if(ammo>0) { ammo--; gunCooldown=.36f; shootTimer=.31f; Fire(); }
            else Say("Out of ammo! Search for ammunition.");
        }
    }

    void Fire()
    {

        Zombie target=null; float best=4.6f;
        foreach(var z in zombies)
        {
            if(z.dead)continue;
            Vector2 offset=z.pos-player;float dist=offset.magnitude;
            if(dist<best && dist>.01f && Vector2.Dot(offset.normalized,facing.normalized)>.90f)
            {target=z;best=dist;}
        }
        if(target==null)return;
        target.hp--;target.hurtClock=.24f;
        if(target.hp<=0){target.dead=true;target.deathClock=0;kills++;Say("Zombie eliminated!");}
    }
}
