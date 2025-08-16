# ip_proj_2.1

# Game objective 

- locate all the items in the house and place them in the right places
- Complete the lava obstacle course
- Confess to shopowner

# Game Hack
- rug is located at the bottom of cabinet in next to the door leading to the living room in the bedroom
- ⁠ouija board is located under the bed
- ⁠candles are located in the cabinet next to the bathroom door in the bedroom at eye level, on the bathtub and next to the bed on the floor
- ⁠teapot is located in the washing machine next to the toilet bowl

# External assets
  Characters
  
    Stan Lee - Download Free 3D model by AsterOmice (@johnalejandro_13) [4f4d885]
    Stylized Ghost model - Download Free 3D model by BrightShot (@_Brightshot) [2eae7ed]
    
  Mixamo: Animatoon of characters
  
  Font: Main Menu and Endscene UI;  Breakdown - https://www.dafont.com/breakdown-pg.font

# Limitations and Bugs of application
- It is set such that to progress out of the ‘dream state of grandma being haunted’ only the teapot needs to be placed down
- you can continue the game by teleporting to the hdb without the teapot, and go to bed without completeing the ‘place teapor on the table’ task
- Can’t get typewriting style for words in cutscene transition hdb 3-4

  # Controls & Gameplay Instructions
| Action             | Key                  |
| ------------------ | -------------------- |
| Move               | WASD                 |
| Jump               | Spacebar             |
| Sprint             | Shift                |
| Crouch             | Ctrl                 |
| Interact           | E                    |
| Pick Up            | R                    |
| Place              | Q                    |
| Throw              | Left click           |


# Recommended Requirements
- Operating System: Windows 10/11 (64-bit) / macOS Monterey 12.0 or later
- Processor: Intel Core i7 or AMD Ryzen 5 and above
- Memory: 8 GB RAM or higher
- Graphics: RAM: 16 GB or higher (for smoother workflow with large projects)
- Storage: 5 GB free storage (SSD strongly recommended)

# FSM Diagrams & AI Implementation Details
1) Grandpa in antique shop
   Patrols -> idles
   - Implementation: After he patrols along waypoints, he will idle on some parts of the store.
2) Cars
   Patrols -> loops
   -Implementation: It patrols along a side of waypoints with 3D Audio added to it whereby when player is near car, it will be loud and will be soft when far from car.
3) Ghost
   Chases
   -Implementation: It follows you to return the teapot to the antique shop and if it touches the player, the player will respawn to the spawn point.
