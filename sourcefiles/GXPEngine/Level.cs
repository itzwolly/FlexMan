using System;
using System.Drawing;
using GXPEngine;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;

public class Level : GameObject 
{
    MyGame _myGame;
    Player player1;
    EnemyManager _em;
    Background background;
    Foreground foreground;
    List<Fighter> fighterListOrder = new List<Fighter>();
    Sound bgMusicSound, levelCompleteMusic;
    SoundChannel playMusic;
    bool startedPlaying = true;
    Item healthItem;
    int _randomNumber;


    //initialize game here
    public Level (MyGame pMyGame)
    {
        _myGame = pMyGame;

        player1 = new Player("blue.png", Key.LEFT, Key.RIGHT, Key.UP, Key.DOWN, Key.SPACE, Key.TAB, 8, 1);
        AddChildAt(player1, 3);
        player1.SetXY(100, 600);

        background = new Background();
        foreground = new Foreground();
        foreground.y = -1;
        
        AddChildAt(background, 2);

        bgMusicSound = new Sound("assets\\sfx\\level.wav", true, true);
        levelCompleteMusic = new Sound("assets\\sfx\\level_complete.wav", false, false);
        playMusic = bgMusicSound.Play();

        _em = new EnemyManager(player1);
        _em.createEnemies();

        _em.GetDeadEnemyList().CollectionChanged += Level_CollectionChanged;

        fighterListOrder.Add(player1);
        foreach (Fighter fighter in _em.GetAllEnemies()) {
            fighterListOrder.Add(fighter);
        }
    }

    //update level here
    void Update() {
        PlayerCamera();
        HandleBoundaries();

        StopBackgroundMusic();
        LevelComplete();

        fighterListOrder.Sort((player1, enemy) => player1.y.CompareTo(enemy.y));
        foreach (Fighter obj in fighterListOrder) {
            AddChild(obj);
        }
        game.AddChildAt(foreground, 30); // not sure if this actually creates duplicates

        HandleBGMovement();
    }

    private void HandleBGMovement() {
        if (player1.isGoingLeft) {
            if (player1.x > game.width * 5 && player1.x < background.width / 2) {
                return;
            }
            if (player1.x > game.width / 2) {
                background.MoveMidGround(false);
                background.MoveBackDrop(false);
                foreground.MoveForeGround(false);
            }
        } else if (player1.isGoingRight) {
            if (player1.x > 0 && player1.x < game.width / 2) {
                return;
            }
            if (player1.x < (background.width - (game.width / 2))) {
                background.MoveMidGround(true);
                background.MoveBackDrop(true);
                foreground.MoveForeGround(true);
            }
        }
    }

    private void HandleBoundaries() {
        if (player1.y < 380) { // will change later, just checking where i want it
            player1.y = player1.oldY;
        }
        if (player1.y > background.height) {
            player1.y = player1.oldY;
        }

        if (player1.x - (player1.width / 4) < 0) {
            player1.x = 20; // dont ask
        }
        if (player1.x + (player1.width / 4) > background.width - player1.width) {
            player1.x = background.width - ((player1.width + player1.width / 2) - 20); // the sprite is fucked up can't actually calculate it, just winged it here.
        }
    }

    private void LevelComplete() {
        if (_em._listOfEnemies.Count() == 0) {
            playMusic.Stop();
            if (startedPlaying == true) {
                levelCompleteMusic.Play();
                startedPlaying = false;
            }
        }
    }
    private void StopBackgroundMusic() {
        if (player1.IsDestroyed()) {
            playMusic.Stop();
        }
    }

    public void PlayerCamera()
    {
        x = game.width / 2 - player1.x;

        if (x > 0)
        {
            x = 0;
        }
        if (x < -((_myGame.width * 5) - (_myGame.width / 6))) {
            x = -((_myGame.width * 5) - (_myGame.width / 6));
        }
    }

    //public void HandleCamera() {
    //    bool isEmpty = !_em.GetAllEnemies().Any();
    //    if (player1.x > (game.width * 2) - (Mathf.Round(player1.width / 3)) && isEmpty) {
    //        Pausable.Pause();
    //        // start moving background untill its out of view.
    //        // while making sure another background image slides into view.
    //        // then remove the previous background and load it after the new one.
    //        if (!(background.x == -game.width)) {
    //            background.x -= 8;
    //            player1.visible = false;
    //        } else {
    //            player1.x = player1.width;
    //            player1.visible = true;
    //            Pausable.UnPause();
    //        }
    //    }
    //}

    private void Level_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            healthItem = new Item();
            healthItem.scale = 0.5f;
            healthItem.SetOrigin(healthItem.width / 2, healthItem.height / 2);
            healthItem.x = (e.NewItems[0] as Enemy).x;
            healthItem.y = (e.NewItems[0] as Enemy).y - 200;

            _randomNumber = Utils.Random(0, 8);
            Console.WriteLine(_randomNumber);
            if (_randomNumber == 7)
            {
                AddChildAt(healthItem, 20);
            }
        }
    }

    public Player GetPlayer() {
        return player1;
    }

    public EnemyManager GetEnemyManager() {
        return _em;
    }
}
