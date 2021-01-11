using System;
using System.Collections.Generic;
using GXPEngine;
using System.Drawing;


public class PauseMenu : GameObject {
    MyGame          _myGame;
    AnimationButton _btnResume,
                    _btnRestart,
                    _btnReturnToMainMenu,
                    _btnQuit;
    Canvas          _canvas;
    Level           _level;

    public PauseMenu(MyGame pMyGame, Level pMyLevel) {
        _myGame = pMyGame;
        _level = pMyLevel;
        createCanvas();
        
        _btnResume = new AnimationButton("assets\\UI\\resume_button00.png", 2, 1);
        AddChild(_btnResume);
        _btnResume.x = (game.width - _btnResume.width) / 2;
        _btnResume.y = ((game.height / 2) - (_btnResume.height * 2));

        _btnRestart = new AnimationButton("assets\\UI\\restart_button00.png", 2, 1);
        AddChild(_btnRestart);
        _btnRestart.x = (game.width - _btnRestart.width) / 2;
        _btnRestart.y = ((game.height / 2) - (_btnRestart.height / 2));

        _btnReturnToMainMenu = new AnimationButton("assets\\UI\\main_menu_button00.png", 2, 1);
        AddChild(_btnReturnToMainMenu);
        _btnReturnToMainMenu.x = (game.width - _btnReturnToMainMenu.width) / 2;
        _btnReturnToMainMenu.y = ((game.height / 2) + (_btnReturnToMainMenu.height));

        _btnQuit = new AnimationButton("assets\\UI\\quit_button00.png", 2, 1);
        AddChild(_btnQuit);
        _btnQuit.x = (game.width - _btnQuit.width) / 2;
        _btnQuit.y = ((game.height / 2) + ((_btnQuit.height * 2) + (_btnQuit.height / 2)));
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (_btnResume.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnResume.currentFrame = 1;
                _btnResume.y += 4;
            } else if (_btnRestart.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnRestart.currentFrame = 1;
                _btnRestart.y += 4;
            } else if (_btnReturnToMainMenu.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnReturnToMainMenu.currentFrame = 1;
                _btnReturnToMainMenu.y += 4;
            } else if (_btnQuit.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnQuit.currentFrame = 1;
                _btnQuit.y += 4;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (_btnResume.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnResume.currentFrame = 0;
                _btnResume.y -= 4;
                _level.IsPaused = false;
                _myGame.ShowMouse(false);
                Pausable.UnPause();
                this.Destroy();
            } else if (_btnRestart.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnRestart.currentFrame = 0;
                _btnRestart.y -= 4;
                switch (_level.GetCurrentLevel) {
                    case 1:
                        _level.IsPaused = false;
                        Pausable.UnPause();
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.LEVEL1);
                        break;
                    case 2:
                        _level.IsPaused = false;
                        Pausable.UnPause();
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.LEVEL2);
                        break;
                    case 3:
                        _level.IsPaused = false;
                        Pausable.UnPause();
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.LEVEL3);
                        break;
                    default:
                        break;
                }
            } else if (_btnReturnToMainMenu.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnReturnToMainMenu.currentFrame = 0;
                _btnReturnToMainMenu.y -= 4;
                _level.IsPaused = false;
                Pausable.UnPause();
                this.Destroy();
                _myGame.SetState(MyGame.STATE.MAINMENU);
            } else if (_btnQuit.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnQuit.currentFrame = 0;
                _btnQuit.y -= 4;
                Environment.Exit(0);
            }
        }
    }

    private void createCanvas() {
        _canvas = new Canvas(1024, 768);
        _canvas.graphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(0,0, 1024, 768));
        _canvas.alpha = 0.45f;
        AddChild(_canvas);
    }
}

