using UnityEngine;

public partial class PatrolGame
{
    LocationMap location;
    Texture2D furnitureAtlas;
    readonly Rect[] furnitureUV = new Rect[16];
    bool atlasLoaded;
    GUIStyle roomLabel;

    void LoadFurniture()
    {
        if(atlasLoaded) return;
        atlasLoaded=true;
        furnitureAtlas=Resources.Load<Texture2D>("Maps/FurnitureAtlas");
        if(furnitureAtlas==null) return;
        // Trim transparent cell padding once; keep pixels crisp at game resolution.
        furnitureAtlas.filterMode=FilterMode.Point;
        var pixels=furnitureAtlas.GetPixels32();
        int cw=furnitureAtlas.width/4, ch=furnitureAtlas.height/4;
        for(int i=0;i<16;i++)
        {
            int x0=(i%4)*cw, y0=(3-i/4)*ch;
            int left=x0+cw,right=x0,bottom=y0+ch,top=y0;
            for(int y=y0;y<y0+ch;y++) for(int x=x0;x<x0+cw;x++)
                if(pixels[y*furnitureAtlas.width+x].a>32)
                {left=Mathf.Min(left,x);right=Mathf.Max(right,x);bottom=Mathf.Min(bottom,y);top=Mathf.Max(top,y);}
            furnitureUV[i]=right>=left && top>=bottom
                ? new Rect((float)left/furnitureAtlas.width,(float)bottom/furnitureAtlas.height,
                    (float)(right-left+1)/furnitureAtlas.width,(float)(top-bottom+1)/furnitureAtlas.height)
                : new Rect(i%4*.25f,(3-i/4)*.25f,.25f,.25f);
        }
    }

    void DrawLocation(Rect map)
    {
        if(location==null) location=LocationMap.Create(level);
        LoadFurniture();
        RectFill(map,new Color(.065f,.085f,.09f));
        RectFill(World(0,0,16,10,map),new Color(.10f,.12f,.12f));
        // Small tiles, inset grout, subtle deterministic wear: no random work per frame.
        for(int x=1;x<31;x++) for(int y=1;y<19;y++)
        {
            float shade= .94f + ((x*17+y*29)%7)*.015f;
            Color c=location.floor*shade;
            if(y>=11) c*=.87f;
            RectFill(World(x*.5f+.012f,y*.5f+.012f,.478f,.478f,map),c);
            if((x*7+y*13)%17==0)
                RectFill(World(x*.5f+.13f,y*.5f+.17f,.16f,.025f,map),c*.76f);
        }
        foreach(var room in location.rooms)
        {
            var r=room.bounds;
            RectFill(World(r.x+.05f,r.y+.06f,r.width-.1f,.035f,map),new Color(location.accent.r,location.accent.g,location.accent.b,.30f));
        }
        // Continuous perimeter; all internal wall segments share collision rectangles.
        DrawWall(new Rect(0,0,16,.5f),map);
        DrawWall(new Rect(0,9.5f,16,.5f),map);
        DrawWall(new Rect(0,.5f,.5f,9),map);
        DrawWall(new Rect(15.5f,.5f,.5f,9),map);
        foreach(var wall in location.walls) DrawWall(wall,map);
        // Windows and entrance threshold are visual details on the perimeter.
        for(int i=0;i<4;i++)
        {
            RectFill(World(1+i*3.4f,9.57f,1.8f,.27f,map),new Color(.13f,.23f,.28f));
            RectFill(World(1.05f+i*3.4f,9.69f,1.7f,.035f,map),new Color(.49f,.68f,.72f));
        }
        RectFill(World(.85f,.08f,1.6f,.36f,map),location.accent*.6f);
        for(int i=0;i<5;i++) RectFill(World(.9f+i*.3f,.15f,.14f,.16f,map),location.accent);
        foreach(var p in location.props)
        {
            var r=p.bounds;
            RectFill(World(r.x+.06f,r.y-.08f,r.width,r.height,map),new Color(0,0,0,.28f));
            if(furnitureAtlas!=null)
                GUI.DrawTextureWithTexCoords(World(r.x,r.y,r.width,r.height,map),furnitureAtlas,furnitureUV[p.sprite],true);
            else RectFill(World(r.x,r.y,r.width,r.height,map),location.accent*.65f);
        }
        if(roomLabel==null) roomLabel=new GUIStyle(small){fontSize=12,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter};
        roomLabel.normal.textColor=new Color(.83f,.88f,.85f,.7f);
        foreach(var room in location.rooms)
            Text(World(room.bounds.x,room.bounds.y+room.bounds.height-.45f,room.bounds.width,.28f,map),room.name,roomLabel);
        // Evacuation platform stays at the existing mission destination.
        RectFill(World(13.1f,7.65f,2,1.8f,map),new Color(.10f,.30f,.24f));
        RectFill(World(13.1f,7.65f,2,.055f,map),new Color(.40f,.82f,.59f));
        RectFill(World(13.1f,9.40f,2,.055f,map),new Color(.40f,.82f,.59f));
        for(int i=0;i<6;i++) RectFill(World(13.15f+i*.32f,7.72f,.16f,.13f,map),new Color(.66f,.65f,.35f));
        Text(World(13.25f,8.2f,1.8f,.5f,map),"SAFE",small);
        Text(new Rect(map.x+15,map.y+3,map.width-30,24),location.title,small);
    }

    void DrawWall(Rect r,Rect map)
    {
        RectFill(World(r.x+.035f,r.y-.08f,r.width,r.height,map),new Color(0,0,0,.4f));
        RectFill(World(r.x,r.y,r.width,r.height,map),new Color(.17f,.20f,.21f));
        RectFill(World(r.x+.025f,r.y+r.height*.50f,r.width-.05f,r.height*.45f,map),new Color(.39f,.43f,.43f));
        RectFill(World(r.x+.025f,r.y+r.height*.38f,r.width-.05f,.045f,map),location.accent*.75f);
    }
}
