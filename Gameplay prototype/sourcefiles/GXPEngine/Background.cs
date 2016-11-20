using System;
using System.Drawing;
using GXPEngine;
using System.Timers;

public class Background : Sprite 
{
    Sprite ground;

    public Background() : base("Background.png")
    {
        ground = new Sprite("Ground.png");
        AddChildAt(ground, 0);
        ground.y += height;

    }
}