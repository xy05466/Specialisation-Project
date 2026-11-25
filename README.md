# Specialisation-Project

Land’s Reign is a solo specialisation project by Kon Xin Yi. It's an isometric dungeon crawler with turn-based combat. After a failed attempt to overthrow the king, you're thrown into his dungeon and have to fight your way out.

## How to play

Explore the dungeon, survive random encounters, and **collect every key in the stage**. Once you have them all, approach the locked door to the king’s room and press **E** to open it. Enter the room and defeat the king.

## Controls

| Mode | Key | Action |
| --- | --- | --- |
| Overworld | `W` `A` `S` `D` | Move |
| Overworld | `E` | Interact with doors, keys, and people |
| Overworld | `U` / `O` | Rotate the camera |
| Combat | `U` / `O` | Switch the target group between allies and enemies |
| Combat | `J` / `L` | Cycle through targets in the selected group |
| Combat | `K` | Confirm the selected target and attack |
| Combat | `Esc` | End the battle immediately |

## Combat

Your party’s mana determines how many actions you can take in a turn. You can spend beyond your available mana and carry that mana debt into the next turn, or end the turn early to avoid it.

Skills can damage or heal allies and enemies. Healing a target increases the skill’s mana cost, while hitting a weakness reduces it.

## Dungeon exploration

- **Branching paths and random encounters.** A side path may lead to useful items or a fight with wolves, skeletons, or mages.
- **Auto-mapper.** The map reveals itself as you explore, shows your current location, and distinguishes regular doors from one-way doors.
- **Limited inventory.** Rearrange or discard items to make room, or store extras in chests. Gems are used at healing wells, which fully heal the party and revive fallen allies. Revival potions can also bring back fallen allies.
- **Save and load.** Saves track player status and location, inventory, collected items, and map progress.

## Development and playtesting

I built Land’s Reign as my first complete solo game. I developed the turn-based combat first, then added the dungeon and auto-mapper, inventory and items, and save and load support. After playtesting, I increased the number of random encounters and added item descriptions.
