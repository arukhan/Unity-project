using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    void SpawnInitialZombies()
    {
        zombies.Clear();
        // Choose free positions from the actual layout, including after being caught.
        for(float y=4;y<=8.5f && zombies.Count<level+1;y+=1.5f)
            for(float x=5;x<=13.5f && zombies.Count<level+1;x+=2.5f)
            {
                var p=new Vector2(x,y);
                if(Blocked(p,ZombieRadius)||Vector2.Distance(p,start)<3.5f)continue;
                int i=zombies.Count;
                zombies.Add(new Zombie {pos=p,anchor=p,waypoint=p,phase=i*1.7f,
                    facing=Vector2.down,wanderAngle=i*1.8f});
            }
    }

    void SpawnZombieAtEdge()
    {
        int alive = 0;
        foreach (Zombie z in zombies) if (!z.dead) alive++;
        if (alive >= MaxLivingZombies) return;
        for (int attempt = 0; attempt < 60; attempt++)
        {
            int edge = UnityEngine.Random.Range(0, 4);
            float t = UnityEngine.Random.Range(1.3f, 8.7f);
            Vector2 pos = edge == 0 ? new Vector2(1.25f, t) :
                          edge == 1 ? new Vector2(14.75f, t) :
                          edge == 2 ? new Vector2(UnityEngine.Random.Range(1.3f, 14.7f), 1.25f) :
                                      new Vector2(UnityEngine.Random.Range(1.3f, 14.7f), 8.75f);
            if (Blocked(pos, ZombieRadius) || Vector2.Distance(pos, player) < 3.5f) continue;
            zombies.Add(new Zombie { pos = pos, anchor = pos, facing = Vector2.down,
                phase = UnityEngine.Random.Range(0f, 6.28f), wanderAngle = UnityEngine.Random.Range(0f, 6.28f) });
            return;
        }
    }
}
