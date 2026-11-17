using System;
public static class MissionCatalog
{
    public static readonly string[] Names = {"Bandages", "First Aid Kit", "Medicine", "Syringe", "Canned Food", "Water", "Energy Bar", "Jacket", "Revolver", "Ammo", "Radio", "Flashlight", "Key", "Fuel", "Battery", "Crowbar"};
    public static readonly string[] Icons = {"item-bandage", "first-aid", "item-medicine", "item-syringe", "item-canned-food", "item-water-bottle", "item-energy-bar", "item-jacket", "item-revolver", "item-ammo", "item-walkie-talkie", "item-flashlight", "item-key", "item-fuel-can", "item-battery", "item-crowbar"};
    public static readonly string[] LevelNames = {"HOSPITAL", "POLICE STATION", "SHOP"};

    public static readonly int[][] Required = {
        new [] {2,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0},
        new [] {0,0,0,0,0,0,0,0,1,2,1,0,1,0,0,0},
        new [] {0,0,0,0,3,2,1,1,0,0,0,0,0,0,0,0}
    };
}
