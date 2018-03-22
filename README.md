# Polyrunner

An infinite runner, except there's (usually) no vertical gravity and the main mechanic is creating copies of yourself

Made for [Weekly Game Jam, Week 36 on itch.io](https://itch.io/jam/weekly-game-jam-36) (Twins)

### Mechanics

You can spawn any number of copies of yourself, but they all move simultaneously and you lose the game if any of them touch an obstacle. If there are too many twins on screen for you to handle, you can "detach" some. Detached twins cannot be controlled and experience vertical gravity. However, if a detached twin collects a pellet, it's worth twice the points!

Self cloning makes it easier to get past compound obstacles. These are multiple dots that appear red until you pass them (then they turn green). You must touch all component obstacles before any of them leave the stage or you lose the game.

#### Compound obstacles in more detail

In the game, you have pellets to collect and walls to avoid. However, component obstacles also spawn. These are the same size and shape as pellets but appear red until you pass (run into) them. Then they turn green. Collectively, these component obstacles are referred to as "compound obstacles." These are the most challenging obstacles in the game.

In order to overcome compound obstacles, you must pass all the component obstacles before any leave the stage i.e. go offscreen. Take care not to lose track of your twins while trying to hit the component obstacles, as if they run into an obstacle, you lose the game.

### HUD

The UI shows you your current score, your high score, and the number of component obstacles currently in existence.

### Objective

Avoid the obstalcles and collect as many pellets as possible. Touching the top and bottom walls is allowed. Make copies of yourself to overcome certain obstacles or to collect more pellets.

You earn more points if the pellet is earned by a detached twin.

### Controls

- Press W and S to move up and down. Note that all of your twins move simultaneously.
- Click anywhere to spawn a new twin at the cursor location.
- Press TAB to select the next twin, if present.
- Press SPACE to detach the selected twin. You can only do this is you have at least 2 twins on screen.
- Press ESC to pause/resume or to start a new game if you lose.
- Run into the pellets to collect them.
- Run into the red dots (component obstacles) to turn them green.

Project available under GPLv3.
