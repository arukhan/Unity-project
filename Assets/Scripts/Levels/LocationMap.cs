using System.Collections.Generic;
using UnityEngine;

// World-space layouts shared by rendering, movement and zombie pathfinding.
public sealed class LocationMap
{
    public struct Prop
    {
        public int sprite;
        public Rect bounds;
        public Prop(int sprite, float x, float y, float w, float h)
        { this.sprite = sprite; bounds = new Rect(x,y,w,h); }
    }
    public struct Room
    {
        public string name;
        public Rect bounds;
        public Room(string name, float x, float y, float w, float h)
        { this.name=name; bounds=new Rect(x,y,w,h); }
    }
    public readonly List<Rect> walls = new List<Rect>();
    public readonly List<Prop> props = new List<Prop>();
    public readonly List<Room> rooms = new List<Room>();
    public readonly List<Rect> collisions = new List<Rect>();
    public string title, subtitle;
    public Color floor, accent;
    void Wall(float x,float y,float w,float h)
    { var r=new Rect(x,y,w,h); walls.Add(r); collisions.Add(r); }
    void Furniture(int sprite,float x,float y,float w,float h)
    { var p=new Prop(sprite,x,y,w,h); props.Add(p); collisions.Add(p.bounds); }

    public static LocationMap Create(int level)
    {
        var m=new LocationMap();
        if(level==1)
        {
            m.title="MERCY HOSPITAL"; m.subtitle="01 / EMERGENCY WING";
            m.floor=new Color(.39f,.47f,.45f); m.accent=new Color(.33f,.66f,.60f);
            m.rooms.Add(new Room("PATIENT WARD",.5f,5.8f,5.1f,3.7f));
            m.rooms.Add(new Room("TREATMENT",6,5.8f,5.2f,3.7f));
            m.rooms.Add(new Room("RECEPTION",.5f,.5f,5.1f,4.7f));
            m.rooms.Add(new Room("PHARMACY",10.5f,.5f,5,4.7f));
            m.Wall(.5f,5.4f,1.5f,.25f); m.Wall(3.5f,5.4f,4,.25f);
            m.Wall(9,5.4f,2.4f,.25f); m.Wall(5.6f,6.8f,.25f,2.7f);
            m.Furniture(0,1.1f,7.1f,1.1f,1.8f); m.Furniture(0,3.6f,7.1f,1.1f,1.8f);
            m.Furniture(14,7.2f,7.1f,1.4f,1.5f); m.Furniture(1,9.5f,8.3f,1.1f,.65f);
            m.Furniture(2,3.1f,2.7f,2,1.1f); m.Furniture(3,1.1f,4.2f,1.8f,.65f);
            m.Furniture(1,11.5f,1.3f,1.2f,.8f); m.Furniture(1,13.6f,3.5f,1.2f,.8f);
            m.Furniture(13,8.8f,1.1f,.65f,.65f);
        }
        else if(level==2)
        {
            m.title="CENTRAL PRECINCT"; m.subtitle="02 / POLICE STATION";
            m.floor=new Color(.35f,.40f,.46f); m.accent=new Color(.49f,.63f,.78f);
            m.rooms.Add(new Room("HOLDING CELLS",.5f,6,5.4f,3.5f));
            m.rooms.Add(new Room("EVIDENCE",6.3f,6,5,3.5f));
            m.rooms.Add(new Room("FRONT DESK",.5f,.5f,5.3f,4.8f));
            m.rooms.Add(new Room("BULLPEN",6.5f,.5f,9,4.8f));
            m.Wall(3.1f,6.7f,.25f,2.8f); m.Wall(6,7,.25f,2.5f);
            m.Wall(6,5.7f,2,.25f); m.Wall(9.5f,5.7f,1.9f,.25f);
            m.Furniture(6,.7f,6,1.1f,.3f); m.Furniture(6,3.4f,6,1.1f,.3f);
            m.Furniture(3,1,8.3f,1.5f,.65f); m.Furniture(3,3.8f,8.3f,1.5f,.65f);
            m.Furniture(7,7.1f,7.7f,1.2f,1.1f); m.Furniture(7,9.6f,7.7f,1.2f,1.1f);
            m.Furniture(2,3,2.8f,2,1.1f); m.Furniture(4,7,2.6f,1.7f,1.2f);
            m.Furniture(4,10.6f,2.6f,1.7f,1.2f); m.Furniture(5,13.8f,4.5f,.9f,1.3f);
            m.Furniture(13,1.1f,4.5f,.65f,.65f);
        }
        else
        {
            m.title="LAST STOP MARKET"; m.subtitle="03 / SUPPLY RUN";
            m.floor=new Color(.47f,.43f,.35f); m.accent=new Color(.79f,.57f,.31f);
            m.rooms.Add(new Room("STOCKROOM",.5f,7.4f,8.6f,2.1f));
            m.rooms.Add(new Room("COLD STORAGE",9.5f,6.9f,3.4f,2.6f));
            m.rooms.Add(new Room("GROCERIES",2.5f,2.8f,8.3f,4.1f));
            m.rooms.Add(new Room("CHECKOUT",10.5f,.5f,5,4.5f));
            m.Wall(.5f,7.1f,2,.25f); m.Wall(4,7.1f,3.1f,.25f); m.Wall(8.6f,7.1f,.8f,.25f);
            m.Furniture(12,1.1f,8.1f,.9f,.9f); m.Furniture(7,5.4f,8.1f,1.2f,1f);
            m.Furniture(11,10,7.6f,1,1.3f); m.Furniture(11,11.6f,7.6f,1,1.3f);
            m.Furniture(8,3,3.3f,1.05f,2.6f); m.Furniture(8,6,3.3f,1.05f,2.6f);
            m.Furniture(8,9,3.3f,1.05f,2.6f); m.Furniture(9,1,4.3f,1.1f,1f);
            m.Furniture(10,12,2.1f,2.2f,1.1f); m.Furniture(13,14.2f,5.2f,.65f,.65f);
        }
        return m;
    }
}
