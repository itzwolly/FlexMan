using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Enemy : Fighter
{
    int time = 0;
    Fighter _target;
    int _type;
    float targetPos;            // The position the enemy will take when fighting the player
    //bool _isDead;

    //public bool GetIsDead { get { return _isDead; } }

    public Enemy(int type, string spriteName, int col, int row, Player target)
        : base(spriteName, col, row)
    {
        _target = target;
        _type = type;
        _health = 3;
    }

    void Update()
    {
        base.Update();

        MirrorToPlayer();
        ChooseFightingSide();
        //WaitingStateBehaviour();

        if (x > targetPos + 2 && GetState() == State.WALKING)
        {             // Player is on the left side
            Walk(-4, 0);
            //scaleX = 1.0f;

        }
        if (x < targetPos - 2 && GetState() == State.WALKING)
        {           // Player is on the right side
            Walk(4, 0);
            //scaleX = -1.0f;
        }
        else
        {
            if (y > _target.y && GetState() == State.WALKING)
            {              // Player is above the enemy
                Walk(0, -4);
            }
            if (y < _target.y && GetState() == State.WALKING)
            {              // Player is below the enemy
                Walk(0, 4);
            }
        }
        time++;
        if (_target.DistanceTo(this) < _target.width && GetState() == State.WALKING && _target._invincible == false/* IF PLAYER TARGET IS NOT INVINCIBLE*/ )
        {
            time = 0;
            SetState(State.FIGHTING);
            Hit();
        }
        if (_health == 0)
        {
            //_isDead = true;
            Destroy();
            Sound enemyDeath = new Sound("death.wav", false, false);
            enemyDeath.Play();
        }
    }

    private void ChooseFightingSide()
    {
        targetPos = _target.x + (_type == 1 ? 75 : -75);                // Makes the enemy go either left or right
    }

    private void MirrorToPlayer()
    {
        if (x > _target.x && GetState() == State.WALKING)           //Enemy is on the right
        {
            scaleX = 1.0f;
        }
        if (x < _target.x && GetState() == State.WALKING)           //Enemy is on the left
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
