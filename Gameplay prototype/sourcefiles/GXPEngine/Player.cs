using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Player : Fighter
{
    int leftKey, rightKey, upKey, downKey, hitKey;

    public Player(string spriteName, int leftKey, int rightKey, int upKey, int downKey, int hitKey, int col, int row)
        : base(spriteName, col, row) {
            this.leftKey = leftKey;
            this.rightKey = rightKey;
            this.downKey = downKey;
            this.upKey = upKey;
            this.hitKey = hitKey;
        //scale = 0.5f;
    }

    void Update()
    {
        base.Update();

        if (Input.GetKey(leftKey)) {
            SetState(Fighter.State.WALKING);
            Walk(-10, 0);
        }
        if (Input.GetKey(rightKey)) {
            SetState(Fighter.State.WALKING);
            Walk(10, 0);
        }
        if (Input.GetKey(upKey)) {
            SetState(Fighter.State.WALKING);
            Walk(0, -10);
        }
        if (Input.GetKey(downKey)) {
            SetState(Fighter.State.WALKING);
            Walk(0, 10);
        }
        if (Input.GetKeyDown(hitKey)) {
            Hit();
        }
    }
}
