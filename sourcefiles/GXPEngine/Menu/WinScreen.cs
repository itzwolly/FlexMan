using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;
using System.Drawing.Text;

public class WinScreen : Sprite {
    MyGame _myGame;
    int endTimer;
    int finalScore;
    Canvas _canvas;
    Font _font;
    PrivateFontCollection _pfc;
    Player _player;

    public WinScreen(MyGame pMyGame, Player pPlayer) : base("assets\\menu\\storyscreen3.png") {
        _myGame = pMyGame;
        _player = pPlayer;

        
        _canvas = new Canvas(1024, 768);
        AddChild(_canvas);

        endTimer = _myGame.GetPlayer().finishTimer;
        _myGame.GetPlayer().finishTimer = 0;

        finalScore = Mathf.Round(Convert.ToSingle(_player.Score * 100) / endTimer * Mathf.Sqrt(3.14f) * Mathf.Pow(4, 5));

        //int conditional = _player.GetMaxHealth() - _player.GetHealth();
        //finalScore = Mathf.Round(_player.score * 100 - (endTimer * 1.5f) - (conditional == 0 ? 0 : conditional * 2.5f));

        _pfc = new PrivateFontCollection();
        _pfc.AddFontFile("assets\\font\\zig_____.ttf");
        _font = new Font(_pfc.Families[0], 24);

        
        _canvas.graphics.DrawString("Final score: " + finalScore, _font, Brushes.White, game.width / 2 - 240, 480);
        
    }

    void Update() {
        
        if (Input.GetKeyUp(Key.V)) {
            _myGame.SetState(MyGame.GameState.STARTSCREEN);
        }
       
    }
}
