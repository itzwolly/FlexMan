using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Enemy : Fighter
{
    int time = 0;
    Player target;
    int type = 0;
    float targetPos;            // The position the enemy will take when fighting the player

    public int Type { get { return type; } set { type = value; } }


    public Enemy(string spriteName, int col, int row, Player target)
        : base(spriteName, col, row)
    {
        this.target = target; 
        _health = 3;

    }

    void Update()
    {
        base.Update();

        MirrorToPlayer();
        ChooseFightingSide();
        //WaitingStateBehaviour();
        if (GetState() == State.WALKING) {
            if (x > targetPos + 2)
            {             // Player is on the left side
                Walk(-4, 0);
                //scaleX = 1.0f;

            }
            if (x < targetPos - 2)
            {           // Player is on the right side
                Walk(4, 0);
                //scaleX = -1.0f;
            }
            else
            {
                if (y > target.y)
                {              // Player is above the enemy
                    Walk(0, -4);
                }
                if (y < target.y)
                {              // Player is below the enemy
                    Walk(0, 4);
                }
            }
        }
        if (target.DistanceTo(this) < target.width && GetState() == State.WALKING && target.GetInvincible == false/* IF PLAYER TARGET IS NOT INVINCIBLE*/ )
        {
            //SetState(State.FIGHTING);
            //Hit();
        }
        if (_health == 0)
        {
            target.score+=100;
            Destroy();
            Sound enemyDeath = new Sound("death.wav", false, false);
            enemyDeath.Play();
        }
    }

    private void ChooseFightingSide()
    {
      if (Type == 1)
        { // right
            targetPos = target.x + 40;
        }
        if (Type == 2)
        { // left
            targetPos = target.x - 72;
        }
    }

    private void MirrorToPlayer()
    {
        if (x > target.x && GetState() == State.WALKING)           //Enemy is on the right
        {
            scaleX = 1.0f;
        }
        if (x < target.x && GetState() == State.WALKING)           //Enemy is on the left
        {
            scaleX = -1.0f;
        }
    }

    private void WaitingStateBehaviour()                // This will handle how enemies in the WAITING state will walk randomly about, posing no threat
    {
        time++;
        if (GetState() == State.WAITING && time > 500)
        {
            SetState(State.WALKING);
            Walk(Utils.Random(-40, 40), Utils.Random(-40, -40));
            time = 0;
            SetState(State.WAITING);
        }
    }


}
