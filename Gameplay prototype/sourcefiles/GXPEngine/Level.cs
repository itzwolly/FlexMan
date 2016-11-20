using System;
using System.Drawing;
using GXPEngine;
using System.Timers;


public class Level : GameObject 
{
    MyGame _myGame;
    Player player1;
    Background background;

    

    //initialize game here
    public Level (MyGame pMyGame)
    {
        _myGame = pMyGame;

        player1 = new Player("blue.png", Key.LEFT, Key.RIGHT, Key.UP, Key.DOWN, Key.SPACE, 8, 1);
        AddChildAt(player1, 1);
        player1.SetXY(100, 600);

        background = new Background();
        AddChildAt(background, 0);

        Sound bgmusic = new Sound("battle_theme.mp3", true, true);
        bgmusic.Play();

        
    }
        
    public Player GetPlayer() {
        return player1;
    }

    //update level here
    void Update() {
        SetBoundaries();
        
        //PlayerCamera();
    }

    public void PlayerCamera()
    {
        x = game.width / 2 - player1.x;
        if (x > 0)
        {
            x = 0;
        }
        if (x < -(_myGame.width / 2))
        {
            x = -(_myGame.width / 2);
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

        if (player1.x > _myGame.width - (player1.width / 4)) {
            player1.x = _myGame.width - (player1.width / 4);
        }

        if (player1.x < 0 + (player1.width / 4))
        {
            player1.x = 0 + (player1.width / 4);
        }
    }
}
