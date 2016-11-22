using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class MyGame : Game { //MyGame is a Game
    Level _level;
    HUD hud;

    public MyGame() : base(1024, 768, false, false) {
        _level = new Level(this);
        AddChild(_level);

        hud = new HUD(_level.GetPlayer());
        hud.y = game.height - hud.height;
        AddChildAt(hud, 1);
    }

    void Update() {
        
    }

    //system starts here
    static void Main() {
        new MyGame().Start();
    }
}
