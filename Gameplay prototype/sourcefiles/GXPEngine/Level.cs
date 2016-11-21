using System;
using System.Drawing;
using GXPEngine;
using System.Timers;
using System.Collections.Generic;
using System.Linq;


public class Level : GameObject 
{
    MyGame _myGame;
    Player player1;
    EnemyManager _em;
    Background background, background2;
    List<Fighter> fighterListOrder = new List<Fighter>();
    

    //initialize game here
    public Level (MyGame pMyGame)
    {
        _myGame = pMyGame;

        player1 = new Player("blue.png", Key.LEFT, Key.RIGHT, Key.UP, Key.DOWN, Key.SPACE, Key.TAB, 8, 1);
        AddChildAt(player1, 1);
        player1.SetXY(100, 600);

        background = new Background();
        background2 = new Background();

        AddChildAt(background, 0);
        AddChildAt(background2, 0);

        background2.x = background.width;

        Sound bgmusic = new Sound("battle_theme.mp3", true, true);
        bgmusic.Play();

        _em = new EnemyManager(player1);
        _em.createEnemies();

        fighterListOrder.Add(player1);
        foreach (Fighter fighter in _em.GetAllEnemies()) {
            fighterListOrder.Add(fighter);
        }
    }

    //update level here
    void Update() {
        SetBoundaries();
        PlayerCamera();
        HandleCamera();

        fighterListOrder.Sort((player1, enemy) => player1.y.CompareTo(enemy.y));
        foreach (Fighter obj in fighterListOrder) {
            AddChild(obj);
        }
    }

    public void PlayerCamera()
    {
        x = game.width / 2 - player1.x;

        if (x > 0)
        {
            x = 0;
        }
        if (x < -(_myGame.width - player1.width))
        {
            x = -(_myGame.width);
        }
    }

    public void HandleCamera() {
        bool isEmpty = !_em.GetAllEnemies().Any();
        if (player1.x > (game.width * 2) - (Mathf.Round(player1.width / 3)) && isEmpty) {
            Pausable.Pause();
            // start moving background untill its out of view.
            // while making sure another background image slides into view.
            // then remove the previous background and load it after the new one.
            if (!(background.x == -game.width)) {
                background.x -= 8;
                background2.x -= 8;
                player1.visible = false;
            } else {
                player1.x = player1.width;
                player1.visible = true;
                Pausable.UnPause();
            }
        }
    }

    public void SetBoundaries()
    {
        if (player1.y > _myGame.height - 127)
        {
            player1.y = _myGame.height - 127;
        }

        if (player1.y < background.height + 43)
        {
            player1.y = background.height + 39;
        }

        if (player1.x - (player1.width / 2) < 0) {
            player1.x = player1.width - (player1.width / 2);
        }

        if (player1.x > _myGame.width + (_myGame.width / 4) - (player1.width / 4)) {
            player1.x = _myGame.width + (_myGame.width / 4) - (player1.width / 4);
        }

        if (player1.x < 0 + (player1.width / 4))
        {
            player1.x = 0 + (player1.width / 4);
        }
    }

    public Player GetPlayer() {
        return player1;
    }
}
