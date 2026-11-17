# Dead Drop

**Dead Drop** is a 2D top-down zombie survival game developed in Unity 6 using C#.

The player takes the role of a police officer who must explore dangerous locations, collect essential supplies, survive zombie attacks, and deliver the required items to a safe zone.

## Game Overview

- **Genre:** 2D Top-Down Zombie Survival
- **Engine:** Unity 6.6 (6000.6.0f1)
- **Programming Language:** C#
- **Platform:** Windows (PC)
- **Development Status:** Playable Prototype

## Gameplay

Dead Drop consists of three levels, each with different environments, obstacles, and mission objectives.

1. **Hospital** — Explore the hospital and collect required medical and survival supplies.
2. **Police Station** — Search the station for mission items while avoiding zombies.
3. **Store** — Collect essential supplies and survive increasingly dangerous encounters.

Players must search crates to find items, manage ammunition and stamina, fight zombies, and reach the SAFE zone after completing their objectives.

## Controls

| Key | Action |
|---|---|
| Arrow Keys | Move |
| Left Shift | Sprint |
| E | Search / Open Crates |
| Space / Enter | Shoot |
| Escape | Pause / Resume |

## Main Features

### Player Mechanics
- Movement and sprinting
- Stamina management
- Directional shooting
- Limited ammunition
- Character animations

### Enemy AI
- Zombie wandering and player detection
- Player chasing and attacking
- Obstacle avoidance using breadth-first search (BFS) pathfinding
- Periodic zombie spawning
- Damage, death animations, and corpse removal

### Items and Missions
- Searchable supply crates
- Randomized item distribution
- Mission-specific collection requirements
- SAFE zone delivery system

### Progression
- Three playable levels
- Unlockable levels
- Increasing initial zombie population
- Saved level progression using Unity PlayerPrefs

### User Interface
- Main menu
- Mission objectives
- Stamina and ammunition indicators
- Kill counter
- Pause menu
- Volume settings saved between sessions

## How to Run

1. Clone the repository:

   `git clone https://github.com/arukhan/Unity-project.git`

2. Open Unity Hub.
3. Select **Add project from disk**.
4. Choose the cloned `Unity-project` folder.
5. Open the project using Unity **6000.6.0f1**.
6. Open `Assets/Scenes/MainMenu.unity`.
7. Press **Play**.

## Project Structure

| Folder | Purpose |
|---|---|
| `Assets/Scripts/Core` | Main game coordination |
| `Assets/Scripts/Player` | Player movement and combat |
| `Assets/Scripts/Enemies` | Zombie AI, spawning, and pathfinding |
| `Assets/Scripts/Items` | Crate interactions |
| `Assets/Scripts/Levels` | Level setup and progression |
| `Assets/Scripts/UI` | Menus and gameplay interface |
| `Assets/Scripts/Graphics` | Character rendering |
| `Assets/Scripts/Data` | Mission data |
| `Assets/Resources` | Game sprites and item assets |

## Development Team

Developed as a university Unity Game Engine course project.

**GitHub Repository:** https://github.com/arukhan/Unity-project

## Future Improvements

- Additional zombie types
- More detailed level environments
- Sound effects and background music
- Weapon upgrades
- Additional survival mechanics
- Improved visual effects
