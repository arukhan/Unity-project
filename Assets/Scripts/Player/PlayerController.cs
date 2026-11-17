using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    void UpdatePlayer(float dt)
    {
        Vector2 move=new Vector2((Key(KeyCode.RightArrow)?1:0)-(Key(KeyCode.LeftArrow)?1:0),(Key(KeyCode.UpArrow)?1:0)-(Key(KeyCode.DownArrow)?1:0)).normalized;
        isMoving = move.sqrMagnitude > 0;
        if(isMoving){facing=move;walkClock+=dt;}
        bool running=Key(KeyCode.LeftShift)&&stamina>0&&move.sqrMagnitude>0;
        float speed=running?4.5f:2.7f;
        stamina=Mathf.Clamp(stamina+(running?-32:19)*dt,0,100);
        Vector2 dx=new Vector2(move.x*speed*dt,0),dy=new Vector2(0,move.y*speed*dt);
        if(!Blocked(player+dx,.27f))player+=dx;
        if(!Blocked(player+dy,.27f))player+=dy;
    }



    static string Direction(Vector2 v)
    {
        if(Mathf.Abs(v.x)>Mathf.Abs(v.y))return v.x>0?"right":"left";
        return v.y>0?"up":"down";
    }
}
