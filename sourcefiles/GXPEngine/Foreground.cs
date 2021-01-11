using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class Foreground : Sprite {
    public Foreground() : base("assets\\bg\\foreground.png") {

    }

    public void MoveForeGround(bool moveToLeft, bool hasPickedUpEnemy) {
        if (moveToLeft) {
            if (hasPickedUpEnemy) {
                x -= scaleX + 2;
            } else {
                x -= scaleX + 10;
            }
            
        } else {
            if (hasPickedUpEnemy) {
                x += scaleX + 2;
            } else {
                x += scaleX + 10;
            }
        }
    }

    public Foreground GetForeGround() {
        return this;
    }
}
