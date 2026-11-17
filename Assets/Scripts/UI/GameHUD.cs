using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    void DrawHud()
    {
        RectFill(new Rect(980,20,285,660),new Color(.12f,.19f,.20f));
        Text(new Rect(995,36,260,35),"LEVEL "+level+" / "+MaxLevel,label);
        Text(new Rect(995,78,260,26),levelNames[level-1],small);
        Text(new Rect(995,130,260,34),"MISSION ITEMS",label);
        int row=0;
        for(int i=0;i<itemNames.Length;i++)if(requirements[level-1][i]>0)
        {
            bool done=collected[i]>=requirements[level-1][i];
            int y=176+row*47;
            Texture2D icon=frames.Item(i);
            if(icon!=null)GUI.DrawTexture(new Rect(995,y,44,44),icon,ScaleMode.ScaleToFit,true);
            Text(new Rect(1044,y+1,216,37),(done?"[OK] ":"")+itemNames[i]+"  "+collected[i]+"/"+requirements[level-1][i],small);
            row++;
        }
        Text(new Rect(995,470,270,35),"STAMINA",label);
        RectFill(new Rect(995,510,245,18),new Color(.12f,.13f,.12f));
        RectFill(new Rect(995,510,245*stamina/100f,18),new Color(.32f,.73f,.46f));
        Text(new Rect(995,541,270,30),"Ammo: "+ammo+"   Kills: "+kills,small);
        Text(new Rect(995,566,270,30),"Arrows - Move",small);
        Text(new Rect(995,590,270,30),"Shift - Sprint",small);
        Text(new Rect(995,614,270,30),"E - Search",small);
        Text(new Rect(995,638,270,30),"Space/Enter - Shoot",small);
        if(Time.time-noticeTime<noticeDuration)
        {RectFill(new Rect(165,605,650,48),new Color(.12f,.16f,.16f,.95f));Text(new Rect(182,612,620,36),notice,label);}
    }
}
