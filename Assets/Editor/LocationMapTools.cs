using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// Editor-only review tools. They never unlock levels or modify saved progress.
[InitializeOnLoad]
public static class LocationMapTools
{
    static int pending;
    static LocationMapTools() { EditorApplication.playModeStateChanged += OnPlayMode; }
    static void OnPlayMode(PlayModeStateChange state)
    {
        if(state!=PlayModeStateChange.EnteredPlayMode) return;
        int next=SessionState.GetInt("DeadDrop.MapPreview",0);
        SessionState.SetInt("DeadDrop.MapPreview",0);
        if(next>0) { pending=next; EditorApplication.delayCall+=ApplyPreview; }
    }
    [MenuItem("Dead Drop/Maps/1 Hospital")]
    static void Hospital() { Preview(1); }
    [MenuItem("Dead Drop/Maps/2 Police station")]
    static void Police() { Preview(2); }
    [MenuItem("Dead Drop/Maps/3 Supermarket")]
    static void Shop() { Preview(3); }
    static void Preview(int level)
    {
        var atlas=AssetImporter.GetAtPath("Assets/Resources/Maps/FurnitureAtlas.png") as TextureImporter;
        if(atlas!=null && !atlas.isReadable) { atlas.isReadable=true; atlas.filterMode=FilterMode.Point; atlas.mipmapEnabled=false; atlas.SaveAndReimport(); }
        if(!EditorApplication.isPlaying)
        {
            if(!UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
            SessionState.SetInt("DeadDrop.MapPreview",level);
            EditorApplication.isPlaying=true;
        }
        else { pending=level; ApplyPreview(); }
    }
    static void ApplyPreview()
    {
        var game=UnityEngine.Object.FindFirstObjectByType<PatrolGame>();
        if(game==null) return;
        typeof(PatrolGame).GetMethod("StartLevel",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(game,new object[]{pending});
        EditorApplication.isPaused=true;
        EditorApplication.ExecuteMenuItem("Window/General/Game");
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }

    [MenuItem("Dead Drop/Maps/Validate all layouts")]
    public static void Validate()
    {
        for(int level=1;level<=3;level++)
        {
            var map=LocationMap.Create(level);
            const int width=64,height=40;
            const float cell=.25f, radius=.27f;
            Func<Vector2,bool> blocked=p=>
            {
                if(p.x<.5f+radius || p.x>15.5f-radius || p.y<.5f+radius || p.y>9.5f-radius) return true;
                foreach(var r in map.collisions)
                    if(p.x>r.xMin-radius && p.x<r.xMax+radius && p.y>r.yMin-radius && p.y<r.yMax+radius) return true;
                return false;
            };
            var seen=new HashSet<int>(); var q=new Queue<int>();
            int start=6*width+6; seen.Add(start);q.Enqueue(start);
            int[] dx={1,-1,0,0},dy={0,0,1,-1};
            while(q.Count>0)
            {
                int v=q.Dequeue(),x=v%width,y=v/width;
                for(int d=0;d<4;d++)
                {
                    int nx=x+dx[d],ny=y+dy[d],n=ny*width+nx;
                    if(nx<0||nx>=width||ny<0||ny>=height||seen.Contains(n)||blocked(new Vector2((nx+.5f)*cell,(ny+.5f)*cell)))continue;
                    seen.Add(n);q.Enqueue(n);
                }
            }
            int walkable=0;
            for(int y=0;y<height;y++)for(int x=0;x<width;x++)
                if(!blocked(new Vector2((x+.5f)*cell,(y+.5f)*cell)))walkable++;
            if(seen.Count!=walkable)throw new Exception(map.title+": disconnected walkable areas "+seen.Count+"/"+walkable);
            int safe=Mathf.FloorToInt(8.6f/cell)*width+Mathf.FloorToInt(14.1f/cell);
            if(!seen.Contains(safe))throw new Exception(map.title+": SAFE unreachable");
            Debug.Log("MAP VALIDATION PASS: "+map.title+"; "+walkable+" reachable cells; SAFE reachable.");
        }
        if(EditorApplication.isPlaying) ValidateCrates();
        File.WriteAllText(Path.GetFullPath("../Map-validation.txt"),"PASS: all three layouts have fully connected walkable space and reachable SAFE zones.\n"+(EditorApplication.isPlaying?"PASS: actual crate generation checked on all three levels; every required item is present.\n":""));
    }
    static void ValidateCrates()
    {
        var game=UnityEngine.Object.FindFirstObjectByType<PatrolGame>();
        if(game==null)throw new Exception("No running PatrolGame");
        var t=typeof(PatrolGame); const BindingFlags f=BindingFlags.Instance|BindingFlags.NonPublic;
        int previous=(int)t.GetField("level",f).GetValue(game);
        for(int level=1;level<=3;level++)
        {
            t.GetMethod("StartLevel",f).Invoke(game,new object[]{level});
            int zombieCount=((System.Collections.ICollection)t.GetField("zombies",f).GetValue(game)).Count;
            if(zombieCount!=level+1)throw new Exception("Level "+level+": incorrect initial zombie count");
            int[] found=new int[MissionCatalog.Names.Length]; int count=0;
            foreach(var crate in (System.Collections.IEnumerable)t.GetField("crates",f).GetValue(game))
            {
                int item=(int)crate.GetType().GetField("item").GetValue(crate);
                if(item>=0)found[item]++;count++;
            }
            for(int i=0;i<found.Length;i++) if(found[i]<MissionCatalog.Required[level-1][i])
                throw new Exception("Level "+level+": missing required item "+i);
            Debug.Log("CRATE VALIDATION PASS: level "+level+"; "+count+" crates; all mission supplies present; "+zombieCount+" initial zombies.");
        }
        pending=previous; ApplyPreview();
    }
}

public class MapAtlasImporter : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if(assetPath!="Assets/Resources/Maps/FurnitureAtlas.png")return;
        var importer=(TextureImporter)assetImporter;
        importer.textureType=TextureImporterType.Default;
        importer.isReadable=true; importer.mipmapEnabled=false;
        importer.filterMode=FilterMode.Point;
        importer.textureCompression=TextureImporterCompression.Uncompressed;
        importer.alphaIsTransparency=true; importer.maxTextureSize=2048;
    }
}
