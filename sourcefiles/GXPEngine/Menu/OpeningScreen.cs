using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;
using System.Drawing.Text;

public class OpeningScreen : Sprite {
    Sprite _controls;
    Sprite _storyScreen, _storyScreen2;
    MyGame _myGame;
    Sound _sound;
    SoundChannel _soundChannel;
    Timer timer;
    Canvas _canvas;
    Font _font;
    PrivateFontCollection _pfc;
    bool cutSceneDone = false;

    public OpeningScreen(MyGame pMyGame) : base("assets\\menu\\spaceBG.png") {
        _myGame = pMyGame;

        _sound = new Sound("assets\\sfx\\boss.wav", true, true);
        _soundChannel = _sound.Play();

        _storyScreen = new Sprite("assets\\menu\\StoryScreen1.png");
        _storyScreen.x = 0;
        _storyScreen.y = 0;
        AddChild(_storyScreen);

        _storyScreen2 = new Sprite("assets\\menu\\StoryScreen2.png");
        _storyScreen2.x = 0;
        _storyScreen2.y = 0;

        _controls = new Sprite("assets\\menu\\controlsScreen.png");
        _controls.x = 0;
        _controls.y = 0;

        _pfc = new PrivateFontCollection();
        _pfc.AddFontFile("assets\\font\\zig_____.ttf");
        _font = new Font(_pfc.Families[0], 10);

        _canvas = new Canvas(game.width, game.height);

        _canvas.graphics.DrawString("press v to skip", _font, Brushes.White, game.width - 178, game.height - 22);
        AddChild(_canvas);
    }


    void Update() {
        timer = new Timer(6000, DestroyStoryScreen);
        
        if (_storyScreen.IsDestroyed()) {
            RemoveChild(_canvas);
            _storyScreen2.AddChild(_canvas);
            AddChild(_storyScreen2);
            new Timer(6000, DestroyNextStoryScreen);
        }

        if (_storyScreen2.IsDestroyed()) {
            cutSceneDone = true;
            _storyScreen2.RemoveChild(_canvas);
            _canvas.Destroy();
            AddChild(_controls);
        }

        if (Input.GetKeyUp(Key.V) && cutSceneDone) {
            _soundChannel.Stop();
            _controls.Destroy();
            _myGame.SetState(MyGame.GameState.LEVEL);
        } else {
            if (Input.GetKeyUp(Key.V)) {
                if (_storyScreen != null) {
                    _storyScreen.Destroy();
                    AddChild(_controls);
                }
                if (_storyScreen2 != null) {
                    _storyScreen2.Destroy();
                    AddChild(_controls);
                }
            }
        }

    }

    private void DestroyStoryScreen() {
        _storyScreen.Destroy();
    }

    private void DestroyNextStoryScreen() {
        _storyScreen2.Destroy();
    }
}