using System;
using System.Collections.Generic;
using GXPEngine;
using System.Drawing;


public class WinScreen : GameObject {
    private MyGame          _myGame;
    private Canvas          _canvas;
    private AnimationButton _btnNextLevel,
                            _btnRedoPreviousLevel,
                            _btnReturnToMainMenu;
    private int             _amountOfMoves;

    public WinScreen(MyGame pMyGame, int pAmountOfMovesAsScore) {
        _myGame = pMyGame;
        _amountOfMoves = pAmountOfMovesAsScore;
        createCanvas();

        switch (_myGame.GetCompletedLevel) {
            case 1:
                _btnNextLevel = new AnimationButton("assets\\UI\\next_level_button00.png", 2, 1);
                break;
            case 2:
                _btnNextLevel = new AnimationButton("assets\\UI\\next_level_button00.png", 2, 1);
                break;
            case 3:
                _btnNextLevel = null; // dont need this button because there is no next level at this point.
                break;
            default:
                break;
        }

        if (_btnNextLevel != null) {
            AddChild(_btnNextLevel);
            _btnNextLevel.x = (game.width - _btnNextLevel.width) / 2;
            _btnNextLevel.y = ((game.height / 2) - (_btnNextLevel.height * 2));
        }

        _btnRedoPreviousLevel = new AnimationButton("assets\\UI\\retry_button_00.png", 2, 1);
        AddChild(_btnRedoPreviousLevel);
        _btnRedoPreviousLevel.x = (game.width - _btnRedoPreviousLevel.width) / 2;
        _btnRedoPreviousLevel.y = ((game.height / 2) - (_btnRedoPreviousLevel.height / 2));

        _btnReturnToMainMenu = new AnimationButton("assets\\UI\\main_menu_button00.png", 2, 1);
        AddChild(_btnReturnToMainMenu);
        _btnReturnToMainMenu.x = (game.width - _btnReturnToMainMenu.width) / 2;
        _btnReturnToMainMenu.y = ((game.height / 2) + (_btnReturnToMainMenu.height));
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (_btnNextLevel != null && _btnNextLevel.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnNextLevel.currentFrame = 1;
                _btnNextLevel.y += 4;
            } else if (_btnRedoPreviousLevel.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnRedoPreviousLevel.currentFrame = 1;
                _btnRedoPreviousLevel.y += 4;
            } else if (_btnReturnToMainMenu.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnReturnToMainMenu.currentFrame = 1;
                _btnReturnToMainMenu.y += 4;
            } 
        }

        if (Input.GetMouseButtonUp(0)) {
            if (_btnNextLevel != null && _btnNextLevel.HitTestPoint(Input.mouseX, Input.mouseY) && _btnNextLevel != null) {
                _btnNextLevel.currentFrame = 0;
                _btnNextLevel.y -= 4;

                switch (_myGame.GetCompletedLevel) {
                    case 1:
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.LEVEL2);
                        break;
                    case 2:
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.LEVEL3);
                        break;
                    case 3:
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.MAINMENU);
                        break;
                    default:
                        break;
                }
            } else if (_btnRedoPreviousLevel.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnRedoPreviousLevel.currentFrame = 0;
                _btnRedoPreviousLevel.y -= 4;

                switch (_myGame.GetCompletedLevel) {
                    case 1:
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.LEVEL1);
                        break;
                    case 2:
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.LEVEL2);
                        break;
                    case 3:
                        this.Destroy();
                        _myGame.SetState(MyGame.STATE.LEVEL3);
                        break;
                    default:
                        break;
                }
            } else if (_btnReturnToMainMenu.HitTestPoint(Input.mouseX, Input.mouseY)) {
                _btnReturnToMainMenu.currentFrame = 0;
                _btnReturnToMainMenu.y -= 4;
                this.Destroy();
                _myGame.SetState(MyGame.STATE.MAINMENU);
            }
        }
    }

    private void createCanvas() {
        _canvas = new Canvas(_myGame.width, _myGame.height);
        Font _font = new Font(SystemFonts.DefaultFont.FontFamily, 12, FontStyle.Regular);

        try {
            _canvas.graphics.Clear(Color.Transparent);
            _canvas.graphics.DrawString("Congratulations! \n\nYou have successfully solved level: " + _myGame.GetCompletedLevel + "\nwithin " + _amountOfMoves + " moves.", _font, Brushes.White, ((_myGame.width / 2) - (_canvas.graphics.DpiX)), (_myGame.height / 4) - (_canvas.graphics.DpiY / 2));
        } catch {
            // empty
        }
        AddChild(_canvas);
    }
}

