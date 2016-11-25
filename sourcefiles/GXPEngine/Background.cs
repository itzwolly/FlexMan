using System;
using System.Drawing;
using GXPEngine;
using System.Timers;

public class Background : Sprite 
{
    Sprite _backDrop, _midGround;

    public Background() : base("assets\\bg\\ground.png")
    {
        _backDrop = new Sprite("assets\\bg\\backdrop.png");
        _backDrop.x = 0;
        _backDrop.y = 0;

        _midGround = new Sprite("assets\\bg\\midground.png");
        _midGround.x = 0;
        _midGround.y = 0;

        game.AddChildAt(_midGround, 1);
        game.AddChildAt(_backDrop, 0);
    }

    void Update() {

    }

    public void MoveBackDrop(bool moveToLeft, bool hasPickedUpEnemy) {
        if (moveToLeft) {
            if (hasPickedUpEnemy) {
                _backDrop.x -= (scaleX + 0.075f);
            } else {
                _backDrop.x -= (scaleX + 0.5f);
            }
        } else {
            if (hasPickedUpEnemy) {
                _backDrop.x += (scaleX + 0.075f);
            } else {
                _backDrop.x += (scaleX + 0.5f);
            }
        }
    }

    public void MoveMidGround(bool moveToLeft, bool hasPickedUpEnemy) {
        if (moveToLeft) {
            if (hasPickedUpEnemy) {
                _midGround.x -= (scaleX + 0.075f);
            } else {
                _midGround.x -= (scaleX + 0.5f);
            }
        } else {
            if (hasPickedUpEnemy) {
                _midGround.x += (scaleX + 0.075f); 
            } else {
                _midGround.x += (scaleX + 0.5f); 
            }
            
        }
    }

    public Sprite GetBackDrop() {
        return _backDrop;
    }

    public Sprite GetMidGround() {
        return _midGround;
    }
}