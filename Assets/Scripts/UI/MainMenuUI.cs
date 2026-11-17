using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    void SetupStyles()
    {
        label = new GUIStyle(GUI.skin.label) { fontSize = 20, normal = { textColor = Color.white } };
        small = new GUIStyle(label) { fontSize = 15 };
        title = new GUIStyle(label) { fontSize = 39, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        button = new GUIStyle(GUI.skin.button) { fontSize = 20, fontStyle = FontStyle.Bold };
        panel = new GUIStyle(GUI.skin.box) { normal = { background = pixel, textColor = Color.white } };
        task = new GUIStyle(small) { fontSize = 16 };
    }

    void OnGUI()
    {
        if (label == null) SetupStyles();
        float s=Mathf.Min(Screen.width/1280f,Screen.height/720f);
        GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1280*s)/2,(Screen.height-720*s)/2,0),Quaternion.identity,new Vector3(s,s,1));
        RectFill(new Rect(0,0,1280,720),new Color(.09f,.14f,.16f));
        if(mode==Mode.Menu){DrawMenu();return;}
        if(mode==Mode.Settings){DrawSettings();return;}
        DrawWorld();DrawHud();
        if(mode==Mode.Paused||mode==Mode.Won)DrawOverlay();
    }

    void DrawMenu()
    {
        Text(new Rect(100,105,1080,90),"DEAD DROP",title);
        Text(new Rect(365,200,600,45),"Search. Survive. Deliver.",label);
        Text(new Rect(390,254,530,30),"Three missions: Hospital / Police Station / Shop",small);
        if(GUI.Button(new Rect(490,335,300,55),"START GAME",button))StartLevel(1);
        if(GUI.Button(new Rect(490,400,300,55),"SETTINGS",button))mode=Mode.Settings;
        for(int i=1;i<=MaxLevel;i++)
        {
            GUI.enabled=i<=unlocked;
            if(GUI.Button(new Rect(325+(i-1)*215,480,200,48),"LEVEL "+i,button))StartLevel(i);
            GUI.enabled=true;
        }
        Text(new Rect(375,590,650,50),"Arrows: Move  |  Shift: Sprint  |  E: Search  |  Space/Enter: Shoot",small);
    }

    void DrawSettings()
    {
        Text(new Rect(350,145,600,70),"SETTINGS",title);
        Text(new Rect(430,275,450,35),"Master volume: "+Mathf.RoundToInt(volume*100)+"%",label);
        volume=GUI.HorizontalSlider(new Rect(430,325,420,25),volume,0,1);
        AudioListener.volume=volume;PlayerPrefs.SetFloat("TLP_Volume",volume);
        if(GUI.Button(new Rect(480,420,320,60),"SAVE & BACK",button)){PlayerPrefs.Save();mode=Mode.Menu;}
    }

    void DrawOverlay()
    {
        RectFill(new Rect(0,0,1280,720),new Color(0,0,0,.75f));
        Text(new Rect(340,175,600,80),mode==Mode.Won?"MISSION COMPLETE!":"PAUSED",title);
        if(mode==Mode.Won && level<MaxLevel && GUI.Button(new Rect(490,300,300,58),"NEXT LEVEL",button))StartLevel(level+1);
        if(mode==Mode.Paused && GUI.Button(new Rect(490,300,300,58),"RESUME",button))mode=Mode.Playing;
        if(GUI.Button(new Rect(490,380,300,58),"RESTART LEVEL",button))StartLevel(level);
        if(GUI.Button(new Rect(490,460,300,58),"MAIN MENU",button))mode=Mode.Menu;
    }
}
