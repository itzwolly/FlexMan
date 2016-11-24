using System;
using System.Collections.Generic;

namespace GXPEngine {
    public class MyGame : Game {
        private MainMenu _mm;
        private LevelSelectMenu _lsm;
        private Level _level;
        private PauseMenu _pm;
        private WinScreen _ws;
        private STATE _state;
        private bool _levelOneCompleted, _levelTwoCompleted, _levelThreeCompleted, _isLevel;
        private int _currentLevel;
        private int _amountOfMovesByPlayer;

        public enum STATE {
            MAINMENU,
            LEVELSELECTMENU,
            PAUSEMENU,
            LEVEL1,
            LEVEL2,
            LEVEL3,
            WINSCREEN
        }

        public bool LevelOneCompleted {
            get {
                return _levelOneCompleted;
            }
            set {
                _levelOneCompleted = value;
            }
        }

        public bool LevelTwoCompleted {
            get {
                return _levelTwoCompleted;
            }
            set {
                _levelTwoCompleted = value;
            }
        }

        public bool LevelThreeCompleted {
            get {
                return _levelThreeCompleted;
            }
            set {
                _levelThreeCompleted = value;
            }
        }

        public bool IsLevel {
            get {
                return _isLevel;
            }
        }

        public int GetCompletedLevel {
            get {
                return _currentLevel;
            }
        }

        public int AmountOfMovesByPlayer {
            get {
                return _amountOfMovesByPlayer;
            }
            set {
                _amountOfMovesByPlayer = value;
            }
        }

        // Initializes game and sets the level
        public MyGame () : base (1024, 768, false) {
            SetState(STATE.MAINMENU);
            targetFps = 60; // cap fps so the game runs the same on all machines.
        }

        public void SetState(STATE pGameState) {
            StopState(_state);
            _state = pGameState;
            StartState(_state);
        }

        void StartState(STATE pGameState) {
            switch (pGameState) {
                case STATE.MAINMENU:
                    _mm = new MainMenu(this);
                    _isLevel = false;
                    AddChild(_mm);
                    ShowMouse(true);
                    break;
                case STATE.LEVELSELECTMENU:
                    _lsm = new LevelSelectMenu(this);
                    _isLevel = false;
                    AddChild(_lsm);
                    ShowMouse(true);
                    break;
                case STATE.PAUSEMENU:
                    _pm = new PauseMenu(this, _level);
                    _isLevel = false;
                    AddChild(_pm);
                    ShowMouse(true);
                    break;
                case STATE.LEVEL1:
                    _level = new Level(this, 1);
                    _currentLevel = 1;
                    _isLevel = true;
                    AddChild(_level);
                    ShowMouse(false);
                    break;
                case STATE.LEVEL2:
                    _level = new Level(this, 2);
                    _currentLevel = 2;
                    _isLevel = true;
                    AddChild(_level);
                    ShowMouse(false);
                    break;
                case STATE.LEVEL3:
                    _level = new Level(this, 3);
                    _currentLevel = 3;
                    _isLevel = true;
                    AddChild(_level);
                    ShowMouse(false);
                    break;
                case STATE.WINSCREEN:
                    _ws = new WinScreen(this, AmountOfMovesByPlayer);
                    _isLevel = false;
                    AddChild(_ws);
                    ShowMouse(true);
                    break;
                default:
                    break;
            }
        }

        public STATE GetState() {
            return _state;
        }

        public void StopState(STATE pGameState) {
            switch (pGameState) {
                case STATE.MAINMENU:
                    if (_mm != null) {
                        _mm.Destroy();
                        _mm = null;
                    }
                    break;
                case STATE.LEVELSELECTMENU:
                    if (_lsm != null) {
                        _lsm.Destroy();
                        _lsm = null;
                    }
                    break;
                case STATE.PAUSEMENU:
                    if (_pm != null) {
                        _pm.Destroy();
                        _pm = null;
                    }
                    break;
                case STATE.LEVEL1:
                case STATE.LEVEL2:
                case STATE.LEVEL3:
                    if (_level != null) {
                        _level.Destroy();
                        _level = null;
                    }
                    break;
                default:
                    break;
            }
        }

        static void Main() {
            new MyGame().Start();
        }
    }
}
