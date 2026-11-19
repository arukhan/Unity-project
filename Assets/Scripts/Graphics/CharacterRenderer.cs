using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    readonly Dictionary<Texture2D, Rect> visibleUV = new Dictionary<Texture2D, Rect>();
    Rect VisibleUV(Texture2D sprite)
    {
        Rect cached;
        if(visibleUV.TryGetValue(sprite,out cached)) return cached;


        Rect uv = sprite.width == 96 && sprite.height == 160
            ? new Rect(9f/96f, 10f/160f, 79f/96f, 140f/160f)
            : new Rect(0,0,1,1);
        try
        {
            Color32[] pixels=sprite.GetPixels32();
            int minX=sprite.width,minY=sprite.height,maxX=-1,maxY=-1;
            for(int y=0;y<sprite.height;y++)
                for(int x=0;x<sprite.width;x++)
                    if(pixels[y*sprite.width+x].a>18)
                    {
                        minX=Mathf.Min(minX,x);maxX=Mathf.Max(maxX,x);
                        minY=Mathf.Min(minY,y);maxY=Mathf.Max(maxY,y);
                    }
            if(maxX>=minX && maxY>=minY)
                uv=new Rect((float)minX/sprite.width,(float)minY/sprite.height,
                    (float)(maxX-minX+1)/sprite.width,(float)(maxY-minY+1)/sprite.height);
        }
        catch(System.Exception ex)
        {


            if(!(ex is UnityException) && !(ex is System.ArgumentException)) throw;
        }
        visibleUV[sprite]=uv;
        return uv;
    }


void DrawCharacter(Texture2D sprite, Vector2 position, float maxWidth, float maxHeight, Rect map)
{
    if (sprite == null) return;

    Rect uv = VisibleUV(sprite);

    float w = maxWidth;
    float h = maxHeight;

    Rect target = World(
        position.x - w * 0.5f,
        position.y - 0.15f,
        w,
        h,
        map
    );

    GUI.DrawTextureWithTexCoords(target, sprite, uv, true);
}

    void DrawWorld()
    {
        Rect map=new Rect(18,20,950,660);
        DrawLocation(map);
        foreach(var c in crates)
        {
            Rect r=World(c.pos.x-.43f,c.pos.y-.42f,.86f,.86f,map);
            Texture2D boxTexture=frames.Box(c.opened);
            if(boxTexture!=null)GUI.DrawTexture(r,boxTexture,ScaleMode.ScaleToFit,true);
            else RectFill(r,new Color(.5f,.3f,.15f));
        }
        foreach(var z in zombies)
        {
            string action=z.dead?"death":z.hurtClock>0?"hurt":z.attackClock>0?"attack":Vector2.Distance(player,z.pos)<3.5f?"chase":"walk";
            int index=z.dead?Mathf.Min(3,Mathf.FloorToInt(z.deathClock*9f)):Mathf.FloorToInt(z.animClock)%4;
            Texture2D sprite=frames.Frame("Zombie",action,Direction(z.facing),index);
            DrawCharacter(sprite,z.pos,1.9f,2.1f,map);
        }
        string policeAction=shootTimer>0?"shoot":isMoving?"walk":"idle";
        int policeIndex=shootTimer>0?Mathf.Clamp(Mathf.FloorToInt((.31f-shootTimer)*13f),0,3):isMoving?Mathf.FloorToInt(walkClock*8)%4:Mathf.FloorToInt(Time.time*2)%4;
        Texture2D police=frames.Frame("Police",policeAction,Direction(facing),policeIndex);
        DrawCharacter(police, player, 2.3f, 1.85f, map);
        if(selectedCrate>=0&&mode==Mode.Playing)
        {Rect r=World(crates[selectedCrate].pos.x-.6f,crates[selectedCrate].pos.y+.45f,2.4f,.5f,map);Text(r,"[E] OPEN",small);}
    }

    Rect World(float x,float y,float w,float h,Rect map)
    {float scale=Mathf.Min(map.width/16f,map.height/10f);float ox=map.x+(map.width-16*scale)/2;float oy=map.y+(map.height-10*scale)/2;return new Rect(ox+x*scale,oy+(10-y-h)*scale,w*scale,h*scale);}


    void RectFill(Rect r,Color c) { Color old=GUI.color;GUI.color=c;GUI.DrawTexture(r,pixel);GUI.color=old; }

    void Text(Rect r,string s,GUIStyle style) {GUI.Label(r,s,style);}
}
