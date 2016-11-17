using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Enemy : Fighter
{
    int time = 0;
    Sprite target;
    int type = 0;
    float targetPos;            // The position the enemy will take when fighting the player

    public int Type { get { return type; } set { type = value; } }

    public Enemy(string spriteName, int col, int row, Player target)
        : base(spriteName, col, row) {
            this.target = target;
    }

    void Update()
    {
        base.Update();

        ChooseFightingSide();
        if (GetState() == State.WALKING) {
            if (x > targetPos + 2) {             // Player is on the left side
                Walk(-2, 0);
            }
            if (x < targetPos - 2) {           // Player is on the right side
                Walk(2, 0);
            } else {
                if (y > target.y) {              // Player is above the enemy
                    Walk(0, -2);
                }
                if (y < target.y) {              // Player is below the enemy
                    Walk(0, 2);
                }
            }
        }
        

        if (target.DistanceTo(this) < target.width / 2 && GetState() == State.WALKING)
        {
            SetState(State.FIGHTING);
            Hit();
        }
        if (hit > 3)
        {
            Destroy();
        }
    }

    private void ChooseFightingSide()
    {
        if (Type == 1) { // right
            targetPos = target.x + 135;                // Makes the enemy go either left or right
        }
        if (Type == 2) { // left
            targetPos = target.x - 125;
        }
        // parse in index number of the list of enemies
        // if index is 0, then go left
        // if index is 1, then go right
    }
}
