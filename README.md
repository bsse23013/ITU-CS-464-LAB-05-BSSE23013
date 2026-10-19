# Roll-a-Ball Collect-a-thon

## How to play

- Move the ball with **WASD** or the **arrow keys**.
- Touch the gold coins to score points.
- Reach **10 points** to win. Press **Restart** on the win panel to play again.

## Bonus round

1. At the start of every game, a random score from **1 to 10** is picked in secret.
2. When your score reaches it, **one red coin** appears (only once per game).
3. Collect the red coin and **50 blue coins** spawn in a circle around you.
4. A hidden counter appears: `BonusCoins: 0/50`.
5. The game will not end until the red coin is collected and the bonus round is finished.

## Setup

1. **Arena:**  Use the colliders Unity adds by default.
2. **Player:** a Sphere tagged `Player`, with a Rigidbody (Linear Damping 1.2) and `PlayerController`.
3. **Coin prefab:** a Cylinder with a Sphere Collider (**Is Trigger** ticked) and `Coin`.
4. **Spawner:** an empty object at (0, 0, 0) with `Spawner`. 
5. **UI** 
   - `ScoreText` (TextMeshPro), anchored top-left
   - `BonusText` (TextMeshPro), anchored top-right, starts hidden
   - `WinPanel` with a "You Win!" label and a Restart button, starts hidden
6. **GameManager:** 


