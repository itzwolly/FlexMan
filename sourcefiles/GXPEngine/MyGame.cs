using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class MyGame : Game { //MyGame is a Game
    HUD hud;
    StartScreen _startScreen;
    OpeningScreen _openingScreen;
    Level _level;
    LoseScreen _loseScreen;
    WinScreen _winScreen;
    GameState _gameState;
    Player _player;

    public enum GameState {
        STARTSCREEN,
        OPENINGSCREEN,
        PAUSESCREEN,
        LOSESCREEN,
        LEVEL,
        WINSCREEN
    }

    public MyGame() : base(1024, 768, false, false) {
        SetState(GameState.STARTSCREEN);
    }

    public void SetState(GameState pGameState) {
        StopState(_gameState);
        _gameState = pGameState;
        StartState(_gameState);
    }

    void StartState(GameState pGameState) {
        switch (pGameState) {
            case GameState.STARTSCREEN:
                _startScreen = new StartScreen(this);
                AddChild(_startScreen);
                break;
            case GameState.OPENINGSCREEN:
                _openingScreen = new OpeningScreen(this);
                AddChild(_openingScreen);
                break;
            case GameState.LEVEL:
                _level = new Level(this);
                _player = _level.GetPlayer();
                AddChild(_level);
                break;
            case GameState.LOSESCREEN:
                _loseScreen = new LoseScreen(this, _player);
                AddChild(_loseScreen);
                break;
            case GameState.WINSCREEN:
                _winScreen = new WinScreen(this, _player);
                AddChild(_winScreen);
                break;
            default:
                break;
        }
    }

    public GameState GetGameState() {
        return _gameState;
    }

    public void StopState(GameState pGameState) {
        switch (pGameState) {
            case GameState.STARTSCREEN:
                if (_startScreen != null) {
                    _startScreen.Destroy();
                    _startScreen = null;
                }
                break;
            case GameState.OPENINGSCREEN:
                if (_openingScreen != null) {
                    _openingScreen.Destroy();
                    _openingScreen = null;
                }
                break;
            case GameState.LEVEL:
                if (_level != null) {
                    _level.Destroy();
                    _level = null;
                }
                break;
            case GameState.LOSESCREEN:
                if (_loseScreen != null) {
                    _loseScreen.Destroy();
                    _loseScreen = null;
                }
                break;
            case GameState.WINSCREEN:
                if (_winScreen != null) {
                    _winScreen.Destroy();
                    _winScreen = null;
                }
                break;
            default:
                break;
        }
    }

    public Player GetPlayer() {
        return _player;
    }

    public Level GetLevel() {
        return _level;
    }

    //system starts here
    static void Main() {
        new MyGame().Start();
    }
}
