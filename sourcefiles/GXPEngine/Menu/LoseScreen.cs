using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;
using System.Drawing.Text;

public class LoseScreen : Sprite {
    Sprite _sprite;
    MyGame _myGame;
    int endTimer;
    int finalScore;
    Canvas _canvas;
    Font _font;
    PrivateFontCollection _pfc;
    Player _player;
        

    public LoseScreen(MyGame pMyGame, Player pPlayer) : base("assets\\menu\\spaceBG.png") {
        _myGame = pMyGame;
        _player = pPlayer;

        _sprite = new Sprite("assets\\menu\\deathscreen.png");
        _sprite.x = 0;
        _sprite.y = 0;

        AddChild(_sprite);

        _canvas = new Canvas(game.width, game.height);
        AddChild(_canvas);

        endTimer = _myGame.GetPlayer().finishTimer;
        _myGame.GetPlayer().finishTimer = 0;

        finalScore = (_player.score == 0) ? 0 : _player.Score * 100 - endTimer;

        _pfc = new PrivateFontCollection();
        _pfc.AddFontFile("assets\\font\\zig_____.ttf");
        _font = new Font(_pfc.Families[0], 24);

        _canvas.graphics.DrawString("Final score: " + Mathf.Abs(finalScore), _font, Brushes.White, game.width / 2 - 300, 600);
        
        //_canvas.graphics.DrawString("Final score: " + finalScore, _font, Brushes.White, 0, 0);
    }

    void Update() {
        
        if (Input.GetKeyUp(Key.V)) {
            _myGame.SetState(MyGame.GameState.STARTSCREEN);
        }
    }
}
