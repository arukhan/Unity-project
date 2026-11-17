using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{

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
