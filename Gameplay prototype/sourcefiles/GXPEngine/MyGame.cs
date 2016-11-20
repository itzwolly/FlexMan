using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class MyGame : Game { //MyGame is a Game
    Level _level;
    EnemyManager _em;
    HUD hud;
    Enemy enemyHitByPlayer;

    public MyGame() : base(1024, 768, false, false) {
        _level = new Level(this);
        AddChild(_level);

        _em = new EnemyManager(_level.GetPlayer());
        _em.createEnemies();

        hud = new HUD(_level.GetPlayer());
        //hud = new HUD(_level.GetPlayer(), _em.GetEnemyHitByPlayer()); // base for new functionality, doesnt work yet!
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
