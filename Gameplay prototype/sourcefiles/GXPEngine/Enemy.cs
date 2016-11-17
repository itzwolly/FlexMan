using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Enemy : Fighter {
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

        if (x > targetPos + 2 && GetState() == State.WALKING)
        {             // Player is on the left side
            Walk(-2, 0);

        }
        if (x < targetPos - 2 && GetState() == State.WALKING)
        {           // Player is on the right side
            Walk(2, 0);
        }
        else
        {
            if (y > target.y && GetState() == State.WALKING)
            {              // Player is above the enemy
                Walk(0, -4);
            }
            if (y < target.y && GetState() == State.WALKING)
            {              // Player is below the enemy
                Walk(0, 4);
            }
        }

        time++;
        if (target.DistanceTo(this) < target.width / 2)
        {
            time = 0;
            SetState(State.FIGHTING);
            Hit();
        }


        if (hit > 3)
        {
            //_isDead = true;
            Destroy();
        }
    }

    private void ChooseFightingSide()
    {
        if (Type == 1) { // right
            targetPos = target.x + 75;                // Makes the enemy go either left or right
        }
        if (Type == 2) { // left
            targetPos = target.x - 65;
        }
        // parse in index number of the list of enemies
        // if index is 0, then go left
        // if index is 1, then go right
    }
}
