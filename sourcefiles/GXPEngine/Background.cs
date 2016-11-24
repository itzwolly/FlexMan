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

    public void MoveBackDrop(bool moveToLeft) {
        if (moveToLeft) {
            _backDrop.x -= (scaleX + 0.15f);
        } else {
            _backDrop.x += (scaleX + 0.15f);
        }
    }

    public void MoveMidGround(bool moveToLeft) {
        if (moveToLeft) {
            _midGround.x -= (scaleX + 0.5f);
        } else {
            _midGround.x += (scaleX + 0.5f); 
        }
    }

    public Sprite GetBackDrop() {
        return _backDrop;
    }

    public Sprite GetMidGround() {
        return _midGround;
    }
}