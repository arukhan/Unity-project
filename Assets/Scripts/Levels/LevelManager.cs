using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    void StartLevel(int next)
    {
        level = next; mode = Mode.Playing; player = start; stamina = 100; opening = 0; selectedCrate = -1; ammo = 12; kills=0; shootTimer=0; spawnClock=0; nextSpawnDelay = UnityEngine.Random.Range(5f, 7f);; facing=Vector2.down; isMoving=false;
        Array.Clear(collected, 0, collected.Length); crates.Clear(); zombies.Clear(); obstacles.Clear();

        location = LocationMap.Create(level);
        obstacles.AddRange(location.collisions);
        boxSearchCount = 0;
        var rng = new System.Random(level * 911 + 7);
        var pool = new List<int>();
        for (int i = 0; i < itemNames.Length; i++) for (int j = 0; j < requirements[level-1][i]; j++) pool.Add(i);
        for (int i = pool.Count; i < 17; i++) pool.Add(i%3==0 ? (level==1?4:level==2?2:0) : -1);
        for (int i = pool.Count-1; i > 0; i--) { int j = rng.Next(i+1); int temp=pool[i];pool[i]=pool[j];pool[j]=temp; }

        var candidates = new List<Vector2>();
        for (float y=1.15f; y<=8.85f; y+=.85f)
            for (float x=1.15f; x<=14.85f; x+=.85f)
            {
                Vector2 p = new Vector2(x,y);
                if (Blocked(p,.55f) || Vector2.Distance(p,start)<1.25f ||
                    Vector2.Distance(p,new Vector2(14.1f,8.6f))<1.5f) continue;
                candidates.Add(p);
            }
        for (int i=candidates.Count-1;i>0;i--)
        {
            int j=rng.Next(i+1);
            Vector2 tmp=candidates[i]; candidates[i]=candidates[j]; candidates[j]=tmp;
        }

        var regions = new List<Vector2>[4];
        for(int i=0;i<4;i++) regions[i]=new List<Vector2>();
        foreach(Vector2 p in candidates)
            regions[(p.x>=8f?1:0)+(p.y>=5f?2:0)].Add(p);
        int placed=0;
        while(placed<pool.Count)
        {
            bool any=false;
            for(int region=0;region<4 && placed<pool.Count;region++)
            {
                var options=regions[region];
                for(int j=options.Count-1;j>=0;j--)
                {
                    Vector2 pos=options[j];
                    bool near=false;
                    foreach(Crate c in crates)
                        if(Vector2.Distance(c.pos,pos)<1.45f){near=true;break;}
                    if(near)options.RemoveAt(j);
                }
                if(options.Count==0)continue;
                Vector2 chosen=options[options.Count-1];
                options.RemoveAt(options.Count-1);
                crates.Add(new Crate {pos=chosen,item=pool[placed++]});
                any=true;
            }
            if(!any)break;
        }
        SpawnInitialZombies();
        Say("Search crates, avoid zombies, deliver supplies to SAFE!");
    }

    bool Blocked(Vector2 p,float radius)
    {
        if(p.x<.5f+radius||p.x>15.5f-radius||p.y<.5f+radius||p.y>9.5f-radius)return true;
        foreach(var r in obstacles) if(p.x>r.xMin-radius&&p.x<r.xMax+radius&&p.y>r.yMin-radius&&p.y<r.yMax+radius)return true;
        return false;
    }

    void CheckMissionCompletion()
    {
        if(Vector2.Distance(player,new Vector2(14.1f,8.6f))<1.1f)
        {
            bool done=true;for(int i=0;i<itemNames.Length;i++)if(collected[i]<requirements[level-1][i])done=false;
            if(done){mode=Mode.Won;unlocked=Mathf.Max(unlocked,Mathf.Min(level+1,MaxLevel));PlayerPrefs.SetInt("TLP_Unlocked",unlocked);PlayerPrefs.Save();}
            else if(Time.time-noticeTime>2.5f)Say("Find all required supplies first!");
        }
    }
}
