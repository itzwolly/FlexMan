using System;
using System.Drawing;
using GXPEngine;
using System.Timers;

public class MyGame : Game //MyGame is a Game
{
    Player player1/*, player2*/;
    EnemyManager _em;
    Background background;

    //initialize game here
    public MyGame () : base(1024, 768, false, false)
    {
        player1 = new Player("blue.png", Key.LEFT, Key.RIGHT, Key.UP, Key.DOWN, Key.SPACE, 8, 1);
        AddChildAt(player1, 1);
        player1.SetXY(100, 600);

        background = new Background();
        AddChildAt(background, 0);

        //Sound bgmusic = new Sound("level.mp3", true, true);
        //bgmusic.Play();


        //player2 = new Player("square.png", Key.A, Key.D, Key.W, Key.S, Key.TAB);
        //AddChild(player2);
        //player2.SetXY(224, 600);
        _em = new EnemyManager(player1);
        _em.createEnemies();
    }
    
    //update game here
    void Update() {
        SetBoundaries();
        foreach (GameObject other in player1.GetHitBox().GetCollisions())
        {
            ResolveCollision(other);
        }
    }


    private void ResolveCollision(GameObject other)
    {
        foreach(Enemy enemy in _em.GetAllEnemies())
        {
            if (other is Player)
            {
                if (player1.GetHitBox().HitTest(enemy.GetHitBox()))
                {
                    if (player1.GetHitBox().y + (player1.GetHitBox().height / 2) > enemy.GetHitBox().y - (enemy.GetHitBox().height / 2))
                    {
                        player1.y = (player1.y - 10);
                    }
                }
                if (player1.GetHitBox().y - (player1.GetHitBox().height / 2) < enemy.GetHitBox().y + (enemy.GetHitBox().height / 2))
                {
                    if (player1.GetHitBox().HitTest(enemy.GetHitBox()))
                    {
                        player1.y = (player1.y + 20);
                    }
                }
            }
        }
    }

    //system starts here
    static void Main() 
    {
        new MyGame().Start();
    }

    public void SetBoundaries()
    {
        if (player1.y > height - 127)
        {
            player1.y = height - 127;
        }

        if (player1.y < background.height + 43)
        {
            player1.y = background.height + 39;
        }
    }
}
