using System;
using System.Drawing;
using GXPEngine;
using System.Timers;

public class MyGame : Game //MyGame is a Game
{
    Player player1, player2;
    EnemyManager _em;

    //initialize game here
    public MyGame () : base(1024, 768, false, false)
    {
        player1 = new Player("assets\\green.png", Key.LEFT, Key.RIGHT, Key.UP, Key.DOWN, Key.SPACE, 5, 1);
        AddChild(player1);
        player1.SetXY(800, 600);
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
            }

    //system starts here
    static void Main() 
    {
        new MyGame().Start();
    }
}
