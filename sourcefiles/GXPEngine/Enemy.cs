using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Enemy : Fighter
{
    public const int SCORE_INCREMENT = 100;
    int time = 0;
    Player target;
    int type = 0;
    float targetPos;            // The position the enemy will take when fighting the player
    public bool isPickedUp = false;
    int disabledTimer = 0;
    bool disabledAfterThrown = false;
    Sound enemyDeathOne, enemyDeathTwo, enemyDeathThree;
    public int gotHitAmount = 0;
    int direction = 0;
    Sound throwOne, throwTwo;

    public int Type { get { return type; } set { type = value; } }

    public Enemy(string spriteName, int col, int row, Player target)
        : base(spriteName, col, row)
    {
        this.target = target;
        _health = 999;
        enemyDeathOne = new Sound("assets\\sfx\\death1.wav", false, false);
        enemyDeathTwo = new Sound("assets\\sfx\\death2.wav", false, false);
        enemyDeathThree = new Sound("assets\\sfx\\death3.wav", false, false);

        throwOne = new Sound("assets\\sfx\\throw1.wav", false, false);
        throwTwo = new Sound("assets\\sfx\\throw2.wav", false, false);
    }

    void Update()
    {
        base.Update();

        MirrorToPlayer();
        ChooseFightingSide();
        if (GetState() != State.PICKEDUP && GetState() != State.THROWN) {
            rotation = 0;
        }
        if (GetState() == State.WAITAFTERTHROWN) {
            rotation = 90;
        }
        if (GetState() == State.DISABLED) {
            if (x < oldX) {
                rotation = 270;
            } else {
                rotation = 90;
            }
        }
        
        if (x > targetPos + 2) { // Player is on the left side
            if (GetState() != State.WAITING && GetState() != State.PICKEDUP && GetState() != State.THROWN && GetState() != State.DISABLED && GetState() != State.WAITAFTERTHROWN) {
                SetState(State.WALKING);
            }
            if (GetState() == State.WALKING) {
                Walk(-4, 0);
            }
        } else {
            if (GetState() != State.PICKEDUP && GetState() != State.DISABLED && GetState() != State.THROWN && GetState() != State.WAITAFTERTHROWN) {
                if (target.y + 10 >= y && target.y - 10 <= y) {
                    SetState(State.FIGHTING);
                } else {
                    SetState(State.WALKING);
                }
            }
        }

        if (x < targetPos - 2) { // Player is on the right side
            if (GetState() != State.WAITING && GetState() != State.PICKEDUP && GetState() != State.THROWN && GetState() != State.DISABLED && GetState() != State.WAITAFTERTHROWN) {
                SetState(State.WALKING);
            }
            if (GetState() == State.WALKING) {
                Walk(4, 0);
            }
        } else {
            if (y > target.y) { // Player is above the enemy
                Walk(0, -4);
            }
            if (y < target.y) {  // Player is below the enemy
                Walk(0, 4);
            }
        }

        if (target.DistanceTo(this) < target.width)
        {
            if (GetState() == State.WAITING) {
                return;
            }
            if (GetState() != State.PICKEDUP && GetState() != State.DISABLED && GetState() != State.THROWN) {
                if (GetState() == State.FIGHTING) {
                    EnemyHit();
                }
            }
        }

        if (GetState() != State.WAITING && GetState() != State.PICKEDUP && GetState() != State.THROWN && GetState() != State.FIGHTING && GetState() != State.WAITAFTERTHROWN && GetState() != State.DISABLED) {
            SetState(State.WALKING);
        }

        if (GetState() == State.PICKEDUP) {
            oldX = x;
            oldY = y;
            this.rotation = 90;
            this.x = target.x - width;
            this.y = (target.y - target.height) - height / 4;
        }

        if (GetState() == Fighter.State.THROWN) {
            switch (Utils.Random(0, 2)) {
                case 0:
                    throwOne.Play();
                    break;
                case 1:
                    throwTwo.Play();
                    break;
            }
            
            if (direction == 1) { // right
                x += 20;
                y += 10;
            } else if (direction == 0) { // left
                x -= 20;
                y += 10;
            }
            if (y > target.y - height / 4) {
                _health -= 2;
                SetState(State.WAITAFTERTHROWN);
                disabledAfterThrown = true;
            }
        }

        if (disabledAfterThrown) {
            disabledTimer++;
            if (disabledTimer == 100) {
                disabledTimer = 0;
                y += height / 4;
                SetState(State.WALKING);
                disabledAfterThrown = false;
            }
        }

        if (_health <= 0)
        {
            target.score += SCORE_INCREMENT; // we love magic values yay
            Destroy();
            switch (Utils.Random(0, 3)) {
                case 0:
                    enemyDeathOne.Play();
                    break;
                case 1:
                    enemyDeathTwo.Play();
                    break;
                case 2:
                    enemyDeathThree.Play();
                    break;
            }
        }

        //Console.WriteLine(GetState());
    }

    public override void SetState(State pState) {
        base.SetState(pState);
        if (pState == State.THROWN) {
            direction = target.direction;
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
            targetPos = target.x - 80;
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
}
