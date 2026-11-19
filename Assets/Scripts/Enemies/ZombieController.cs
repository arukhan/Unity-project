using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    bool UpdateZombies(float dt)
    {
        spawnClock += dt;
        if (spawnClock >= nextSpawnDelay)
        {
            spawnClock = 0f;
            nextSpawnDelay = 10f;
            SpawnZombieAtEdge();
        }
        for (int i = zombies.Count - 1; i >= 0; i--)
        {
            Zombie z = zombies[i];
            if (z.dead)
            {
                z.deathClock += dt;
                if (z.deathClock >= CorpseLifetime) zombies.RemoveAt(i);
                continue;
            }
            z.hurtClock = Mathf.Max(0f, z.hurtClock - dt);
            z.attackClock = Mathf.Max(0f, z.attackClock - dt);
            float dist = Vector2.Distance(player, z.pos);
            bool chasing = dist < 8f;
            z.wanderAngle += dt * (.55f + .1f * (i % 5));
            Vector2 wander = new Vector2(Mathf.Cos(z.wanderAngle + z.phase), Mathf.Sin(z.wanderAngle + z.phase));

            Vector2 desired = chasing ? player : Vector2.Lerp(z.pos + wander * 2f, player, .45f);
            desired.x = Mathf.Clamp(desired.x, 1.1f, 14.9f);
            desired.y = Mathf.Clamp(desired.y, 1.1f, 8.9f);
            z.pathClock -= dt;
            if (z.pathClock <= 0f || Vector2.Distance(z.pos, z.waypoint) < .23f)
            {
                z.waypoint = FindNextWaypoint(z.pos, desired);
                z.pathClock = chasing ? .25f : .55f;
            }
            Vector2 direction = z.waypoint - z.pos;
            if (direction.sqrMagnitude > .015f)
            {
                direction.Normalize();
                float zombieMoveSpeed = chasing ? 1.72f : .72f;
                Vector2 step = direction * zombieMoveSpeed * dt;
                Vector2 candidate = z.pos + new Vector2(step.x, 0f);
                if (!Blocked(candidate, ZombieRadius)) z.pos = candidate;
                candidate = z.pos + new Vector2(0f, step.y);
                if (!Blocked(candidate, ZombieRadius)) z.pos = candidate;
                z.facing = direction;
            }
            z.animClock += dt * (chasing ? 9f : 5f);
            if (Vector2.Distance(player, z.pos) < .52f)
            {
                z.attackClock = .4f;
                Caught();
                return true;
            }
        }
        return false;
    }
}
