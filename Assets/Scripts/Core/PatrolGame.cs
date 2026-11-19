using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{

    enum Mode { Menu, Playing, Paused, Won, Settings }
    Mode mode = Mode.Menu;
    int level = 1, unlocked;
    const int MaxLevel = 3;
    readonly string[] itemNames = MissionCatalog.Names;
    readonly string[] levelNames = MissionCatalog.LevelNames;
    readonly int[][] requirements = MissionCatalog.Required;
    readonly List<Crate> crates = new List<Crate>();
    readonly List<Zombie> zombies = new List<Zombie>();
    readonly List<Rect> obstacles = new List<Rect>();
    readonly int[] collected = new int[16];
    Vector2 player, start = new Vector2(1.5f, 1.5f);
    float stamina = 100, noticeTime, noticeDuration, opening, gunCooldown;
    float spawnClock, nextSpawnDelay;
    const int MaxLivingZombies = 12;
    const float CorpseLifetime = 5f;
    const float ZombieRadius = .27f;
    int selectedCrate = -1;
    string notice = "";
    float volume = 0.7f;
    Texture2D pixel;
    FrameLibrary frames = new FrameLibrary();
    float shootTimer, recoilTimer;
    int ammo = 12, kills;
    Vector2 facing = Vector2.down;
    float walkClock;
    bool isMoving;
    int boxSearchCount;
    Camera cam;
    GUIStyle label, small, title, button, panel, task;
    class Crate { public Vector2 pos; public int item; public bool opened; }
    class Zombie { public Vector2 pos, anchor, facing; public float phase, animClock, attackClock, hurtClock, deathClock; public int hp=2; public bool dead; public float wanderAngle, pathClock; public Vector2 waypoint; }

    void Awake()
    {
        pixel = new Texture2D(1, 1); pixel.SetPixel(0, 0, Color.white); pixel.Apply();
        cam = Camera.main;
        if (cam == null) { var go = new GameObject("Camera"); cam = go.AddComponent<Camera>(); go.tag = "MainCamera"; }
        cam.orthographic = true; cam.orthographicSize = 5.8f; cam.transform.position = new Vector3(8, 5, -10);
        cam.backgroundColor = new Color(.12f,.17f,.18f);
        unlocked = Mathf.Clamp(PlayerPrefs.GetInt("TLP_Unlocked", 1), 1, MaxLevel);
        volume = PlayerPrefs.GetFloat("TLP_Volume", .7f);
        AudioListener.volume = volume;

    }

    bool Key(KeyCode code)
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKey(code);
#elif ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return false;
        switch (code) { case KeyCode.UpArrow: return Keyboard.current.upArrowKey.isPressed; case KeyCode.DownArrow: return Keyboard.current.downArrowKey.isPressed; case KeyCode.LeftArrow: return Keyboard.current.leftArrowKey.isPressed; case KeyCode.RightArrow: return Keyboard.current.rightArrowKey.isPressed; case KeyCode.LeftShift: return Keyboard.current.leftShiftKey.isPressed; default: return false; }
#else
        return false;
#endif
    }

    bool Press(KeyCode code)
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyDown(code);
#elif ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return false;
        switch (code) { case KeyCode.E: return Keyboard.current.eKey.wasPressedThisFrame; case KeyCode.Escape: return Keyboard.current.escapeKey.wasPressedThisFrame; case KeyCode.Space: return Keyboard.current.spaceKey.wasPressedThisFrame; case KeyCode.Return: return Keyboard.current.enterKey.wasPressedThisFrame; default: return false; }
#else
        return false;
#endif
    }

    void Update()
    {
        if(mode==Mode.Playing && Press(KeyCode.Escape)){mode=Mode.Paused;return;}
        if(mode==Mode.Paused && Press(KeyCode.Escape)){mode=Mode.Playing;return;}
        if(mode!=Mode.Playing)return;
        float dt=Mathf.Min(Time.deltaTime,.04f);
        UpdatePlayer(dt);
        UpdateCrates();
        UpdateCombat(dt);
        if (UpdateZombies(dt)) return;
        CheckMissionCompletion();
    }


void Caught()
{
    player = start;
    stamina = 100f;
    ammo = 12;
    kills = 0;
    gunCooldown = 0f;
    shootTimer = 0f;
    recoilTimer = 0f;
    walkClock = 0f;
    isMoving = false;
    facing = Vector2.down;
    opening = 0f;
    selectedCrate = -1;
    boxSearchCount = 0;

    Array.Clear(collected, 0, collected.Length);
    foreach (var c in crates)
        c.opened = false;


    SpawnInitialZombies();

    spawnClock = 0f;
    nextSpawnDelay = UnityEngine.Random.Range(5f, 7f);
    Say("Caught! Zombies reset. Supplies lost. Ammo restored.");
}

    void Say(string s) { notice=s;noticeTime=Time.time;noticeDuration=2.6f; }
}
