﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Enemy : Fighter
{
    public const int SCORE_INCREMENT = 100;
    int animTimer = 0;
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
    bool walkAnimCheck;
    int walkTimer;
    public int _id;

    public int Type { get { return type; } set { type = value; } }

    public Enemy(string spriteName, int col, int row, Player target, int id)
        : base(spriteName, col, row)
    {
        this.target = target;
        _health = 4;
        _id = id;
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

        oldX = x;
        oldX = y;
        
        if (x > targetPos + 2) { // Player is on the left side
            if (GetState() != State.WAITING && GetState() != State.PICKEDUP && GetState() != State.THROWN && GetState() != State.DISABLED && GetState() != State.WAITAFTERTHROWN) {
                SetState(State.WALKING);
            }
            if (GetState() == State.WALKING) {
                if (_id == 10) {

                    Walk(-4, 0);
                } else {
                    Walk(-2, 0);
                }
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
                if (_id == 10) {
                    Walk(4, 0);
                } else {
                    Walk(2, 0);
                }
                
            }
        } else {
            if (y > target.y) { // Player is above the enemy
                if (_id == 10) {

                    Walk(0, -4);
                } else {
                    Walk(0, -2);
                }
            }
            if (y < target.y) {  // Player is below the enemy
                if (_id == 10) {

                    Walk(0, 4);
                } else {
                    Walk(0, 2);
                }
            }
        }

        if (target.DistanceTo(this) < target.width) {
            
            if (GetState() == State.WAITING) {
                return;
            }
            if (GetState() != State.PICKEDUP && GetState() != State.DISABLED && GetState() != State.THROWN) {
                if (GetState() == State.FIGHTING) {
                    walkAnimCheck = false;
                    //enemyAttack = true;
                    isAttacking = true;
                    EnemyHit();
                }
            }
        }

        if (GetState() != State.WAITING && GetState() != State.PICKEDUP && GetState() != State.THROWN && GetState() != State.FIGHTING && GetState() != State.WAITAFTERTHROWN && GetState() != State.DISABLED) {
            SetState(State.WALKING);
        }

        if (GetState() == State.WALKING) {
            isAttacking = false;
        }

        if (GetState() == State.PICKEDUP) {
            currentFrame = 36; 

            oldX = x;
            oldY = y;
            this.rotation = 90;
            this.x = target.x - target.width;
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

    }

    public override void Walk(float moveX, float moveY) {
        base.Walk(moveX, moveY);

        if (!isPickedUp && GetState() != State.DISABLED && GetState() != State.THROWN && GetState() != State.WAITAFTERTHROWN && GetState() != State.WAITING) {
            if (x > oldX || x < oldX || y > oldY || y < oldY) {
                if (!walkAnimCheck) {
                    currentFrame = 20;
                }
                walkAnimCheck = true;

                if (!isAttacking) {
                    walkTimer++;
                    if (currentFrame == 34) {
                        currentFrame = 20;
                    }

                    if (walkTimer > 2) {
                        NextFrame();
                        walkTimer = 0;
                    }
                }
            }
        }
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

    protected override void EndHit() {
        base.EndHit();
    }
}
