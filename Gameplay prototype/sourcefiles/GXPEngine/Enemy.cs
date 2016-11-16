using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Enemy : Fighter {
    int time = 0;
    Sprite target;
    int type;
    //bool _isDead;

    //public bool GetIsDead { get { return _isDead; } }

    public Enemy(int type, string spriteName, int col, int row, Player target)
        : base(spriteName, col, row) {
            this.target = target;
            this.type = type;
    }

    void Update()
    {
        base.Update();

        float targetPos = target.x + (type == 1 ? 75 : -65);

        if (x > targetPos + 2 && GetState() == State.WALKING) {
            Walk(-2, 0);
        } else if (x < targetPos - 2 && GetState() == State.WALKING) {
            Walk(2, 0);
        }
        else { 
            if (y > target.y && GetState() == State.WALKING) {
                Walk(0, -4);
            }
            if (y < target.y && GetState() == State.WALKING) {
                Walk(0, 4);
            }
        }
        time++;
        if (target.DistanceTo(this) < target.width / 2) {
            time=0;
            // SetState(State.FIGHTING);
            //Hit();
        }

        if (hit > 3) {
            //_isDead = true;
            Destroy();
        }


    }
}
