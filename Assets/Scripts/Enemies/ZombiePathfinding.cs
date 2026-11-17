using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{

    Vector2 FindNextWaypoint(Vector2 from, Vector2 target)
    {
        const float cell = .5f;
        const int width = 32, height = 20;
        int sx = Mathf.Clamp(Mathf.FloorToInt(from.x / cell), 0, width - 1);
        int sy = Mathf.Clamp(Mathf.FloorToInt(from.y / cell), 0, height - 1);
        int tx = Mathf.Clamp(Mathf.FloorToInt(target.x / cell), 0, width - 1);
        int ty = Mathf.Clamp(Mathf.FloorToInt(target.y / cell), 0, height - 1);
        int source = sy * width + sx;
        int goal = ty * width + tx;
        bool[] passable = new bool[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                passable[y * width + x] = !Blocked(new Vector2((x + .5f) * cell, (y + .5f) * cell), ZombieRadius);
        if (!passable[source])
        {

            float nearest = float.PositiveInfinity;
            for (int i = 0; i < passable.Length; i++)
            {
                if (!passable[i]) continue;
                Vector2 center = new Vector2((i % width + .5f) * cell, (i / width + .5f) * cell);
                float distance = (center - from).sqrMagnitude;
                if (distance < nearest) { nearest = distance; source = i; }
            }
            if (float.IsPositiveInfinity(nearest)) return from;
        }
        if (!passable[goal])
        {
            float best = float.PositiveInfinity;
            for (int i = 0; i < passable.Length; i++)
            {
                if (!passable[i]) continue;
                float d = (new Vector2((i % width + .5f) * cell, (i / width + .5f) * cell) - target).sqrMagnitude;
                if (d < best) { best = d; goal = i; }
            }
        }
        int[] previous = new int[width * height];
        for (int i = 0; i < previous.Length; i++) previous[i] = -1;
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(source);
        previous[source] = source;
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            int x = current % width, y = current / width;
            for (int k = 0; k < 4; k++)
            {
                int nx = x + dx[k], ny = y + dy[k];
                if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;
                int next = ny * width + nx;
                if (!passable[next] || previous[next] >= 0) continue;
                previous[next] = current;
                queue.Enqueue(next);
            }
        }
        if (previous[goal] < 0)
        {
            float closest=float.PositiveInfinity;
            int reachable=source;
            for(int i=0;i<previous.Length;i++)
            {
                if(previous[i]<0)continue;
                Vector2 point=new Vector2((i%width+.5f)*cell,(i/width+.5f)*cell);
                float d=(point-target).sqrMagnitude;
                if(d<closest){closest=d;reachable=i;}
            }
            goal=reachable;
        }
        int step = goal;
        while (previous[step] != source && step != source) step = previous[step];
        if (step == source) return new Vector2((source % width + .5f) * cell, (source / width + .5f) * cell);
        return new Vector2((step % width + .5f) * cell, (step / width + .5f) * cell);
    }
}
