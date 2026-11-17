using System.Collections.Generic;
using UnityEngine;
public sealed class FrameLibrary
{
    readonly Dictionary<string,Texture2D[]> cache = new Dictionary<string,Texture2D[]>();
    public Texture2D Frame(string actor, string action, string direction, int index)
    {
        string key = actor + "/" + actor.ToLowerInvariant() + "-" + action + "-" + direction;
        Texture2D[] frames;
        if (!cache.TryGetValue(key,out frames))
        {
            frames = new Texture2D[4];
            for(int i=0;i<4;i++) frames[i]=Resources.Load<Texture2D>(key+"-"+(i+1).ToString("00"));
            cache.Add(key,frames);
        }
        return frames[Mathf.Abs(index)%4];
    }
    public Texture2D Item(int id) => Resources.Load<Texture2D>("Items/"+MissionCatalog.Icons[id]);
    public Texture2D Box(bool opened) => Resources.Load<Texture2D>("Box/box-"+(opened?"opened":"closed"));
}
