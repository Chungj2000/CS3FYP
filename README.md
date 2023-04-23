# CS3FYP
Repository for FYP Unity development.

## Purpose
The aim of this project is to create a **turn-based strategy game** that can be used to develop core skills that will be useful in real world scenarios, such as *decision making*, *risk taking*, and *problem solving*. Said game is envisioned to potentially be used as a medium for education and entertainment in such a way that Chess is treated as an extra curriculum activity in some schools.

## Progress
**A *brief* checklist of fundamental components for the game.**
- [x] Add a Main Menu Scene with expected Player interaction functionality.
- [x] Implement a GridSystem containing the relevant logic for visual display and Player interactions.
- [x] Implement logic to handle Player interactions with Units, including:
  - [x] Unit Parameters.
  - [x] Unit Selection.
  - [x] Unit Actions *(E.g. Move, Attack, Summon)*.
  - [x] Unit health logic and visual healthbar.
  - [x] Unit pool of Prefabs and Variants.
- [x] Implement a *Turn-Based System*.
- [x] Implement Multiplayer intergration using [```Photon Engine```](https://www.photonengine.com/pun) framework. This includes:
  - [x] Session handling on the Main Menu.
  - [x] Remote Procedural Calls for Unit interactions.
  - [x] Integrate TurnSystem for Player turn rotation.
- [x] **Add win conditions:**
  - [x] A Player concedes.
  - [x] All Units of a Player faction are dead.
  - [x] Player owned Fortress is destroyed.
- [x] Revamp the combat system for Weapon and Armour type.
- [x] Visual Overhaul for Prefab models, and UIs.
- [ ] Implement Terrain logic.


### Trello Link for Development Log:
The public link for the [Trello Board](https://trello.com/invite/b/4s4Zh5gV/ATTIa21ed244f89fd81309a597bfa92918588C7702CD/final-year-project) of this repository.
Documents tasks and activities designated for this project in greater detail than the commit history.

