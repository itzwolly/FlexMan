﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Player : Fighter
{
    int leftKey, rightKey, upKey, downKey, hitKey, pickUpKey;
    public float oldX, oldY;
    public bool isColliding = false;
    public bool hasPickedUp = false;
    

    public Player(string spriteName, int leftKey, int rightKey, int upKey, int downKey, int hitKey, int pickUpKey, int col, int row) : base(spriteName, col, row) {
        this.leftKey = leftKey;
        this.rightKey = rightKey;
        this.downKey = downKey;
        this.upKey = upKey;
        this.hitKey = hitKey;
        this.pickUpKey = pickUpKey;
        _health = 15;
        _maxHealth = _health;
        Name = "Teddy";
       
        //scale = 0.5f;
       // Mirror(false, false);
    }

    void Update()
    {
        base.Update();
        oldX = x;
        oldY = y;

        if (Input.GetKey(leftKey)) {
            SetState(Fighter.State.WALKING);
            if (!hasPickedUp) {
                Walk(-5, 0);
            } else {
                Walk(-1, 0);
            }
            
        }
        else if (Input.GetKey(rightKey)) {
            SetState(Fighter.State.WALKING);
            if (!hasPickedUp) {
                Walk(5, 0);
            } else {
                Walk(1, 0);
            }
            
        }

        oldX = x;
        oldY = y;
        if (Input.GetKey(upKey)) {
            SetState(Fighter.State.WALKING);
            if (!hasPickedUp) {
                Walk(0, -5);
            } else {
                Walk(0, -1);
            }
        }
        else if (Input.GetKey(downKey)) {
            SetState(Fighter.State.WALKING);
            if (!hasPickedUp) {
                Walk(0, 5);
            } else {
                Walk(0, 1);
            }
        }

        if (Input.GetKeyDown(hitKey)) {
            if (!hasPickedUp) {
                Hit();
            }
        }
        if (Input.GetKeyDown(pickUpKey)) {
            if (!hasPickedUp) {
                PickUpObject();
            } else {
                GetPickedUpEnemy().SetState(State.THROWN);
            }
            
        }
    }

    public bool GetInvincible
    {
        get
        {
            return _invincible;
        }
    }
}