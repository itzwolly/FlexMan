using System;
using System.Collections.Generic;
using GXPEngine;
using System.Drawing;
using System.Drawing.Text;

public class StartScreen : Sprite {
    Sprite _title;
    Canvas _canvas;
    Font _font;
    PrivateFontCollection _pfc;
    MyGame _myGame;
    int timer = 0;
    Sound _sound;
    SoundChannel _soundChannel;


    public StartScreen(MyGame pMyGame) : base("assets\\menu\\spaceBG.png") {
        _myGame = pMyGame;

        _sound = new Sound("assets\\sfx\\title.wav");
        _soundChannel = _sound.Play();

        _title = new Sprite("assets\\menu\\title.png");
        AddChild(_title);

        _canvas = new Canvas(game.width, game.height);
        AddChild(_canvas);

        _canvas.alpha = 0.9f;

        _pfc = new PrivateFontCollection();
        _pfc.AddFontFile("assets\\font\\zig_____.ttf");
        _font = new Font(_pfc.Families[0], 18);
    }

    private void ChangeOpacity() {
        timer++;
        if (timer == 50) {
            _canvas.alpha = 0.6f;
        } else if (timer == 100) {
            _canvas.alpha = 0.9f;
            timer = 0;
        }
        
    }

    void Update() {
        _canvas.graphics.Clear(Color.Transparent);
        ChangeOpacity();
        _canvas.graphics.DrawString("Press V to continue...", _font, Brushes.White, game.width / 2 - 200, game.height / 2 + _canvas.height / 4);

        if (Input.GetKeyUp(Key.B)) {
            Environment.Exit(0);
        }

        if (Input.GetKeyUp(Key.V)) {
            _soundChannel.Stop();
            _myGame.SetState(MyGame.GameState.OPENINGSCREEN);
        }
    }
}