using System;
using System.Collections.Generic;
using GXPEngine;
using System.Drawing;
using System.Timers;

public class Enemy : AnimationSprite {
    private Canvas _canvas;
    private int _increment;
    //private Canvas _healthCanvas;
    //private Font _font;
    private int _health;
    private bool _alive;
    private bool _shouldDealDamage = true;
    private bool _startAttackAnimation;
    private bool _canAttack = true;
    private int _velocity = 10;

    public bool ShouldDealDamage {
        get {
            return _shouldDealDamage;
        }
        set {
            _shouldDealDamage = value;
        }
    }
    public bool StartAttackAnimation {
        get {
            return _startAttackAnimation;
        }
        set {
            _startAttackAnimation = value;
        }
    }
    public bool CanAttack {
        get {
            return _canAttack;
        }
        set {
            _canAttack = value;
        }
    }
    public bool IsAlive {
        get {
            return _alive;
        }
        set {
            _alive = value;
        }
    }

    public int GetVelocity {
        get {
            return _velocity;
        }
    }
    
    public Enemy(string fileName, int cols, int rows) : base(fileName, cols, rows) {
        _alive = true;
        _health = 100;
        scale = 0.35f;
        SetOrigin(width / 2, height / 2);
        currentFrame = 0;
        x = (game.width / 2);
        y = game.height - height;
        DrawHitbox();

        //_healthCanvas = new Canvas(game.width, 150);
        //_healthCanvas.y -= 100;
        //_healthCanvas.x -= 25;
        //_font = new Font(FontFamily.GenericSansSerif, 35);
        //_healthCanvas.graphics.DrawString("HP:" + getHealth(), _font, Brushes.LightBlue, 0, 0);
        //AddChild(_healthCanvas);
    }

    void Update() {
        if (_health == 0) {
            _alive = false;
        }
    }

    //public void UpdateEnemyHealthHUD() {
    //    try {
    //        _healthCanvas.graphics.Clear(Color.Transparent);
    //        _healthCanvas.graphics.DrawString("HP:" + getHealth(), _font, Brushes.Red, 0, 0);
    //    } catch {
    //        // empty
    //    }
    //}

    public void DoAttackAnimation() {
        _increment++;
        if (_increment == 100) {
            _increment = 0;
            NextFrame();
        }
    }

    public void takeDamage() {
        if (_alive) {
            _health--;
        }
    }

    public int getHealth() {
        return _health;
    }

    private void DrawHitbox() {
        _canvas = new Canvas(width, height);
        _canvas.width = 50;
        _canvas.x -= ((width / 2) + _canvas.width);
        _canvas.height += 75;
        _canvas.graphics.FillRectangle(System.Drawing.Brushes.Red, 0, 0, width, 80);
        _canvas.alpha = 0.33f;
        AddChild(_canvas);
    }

    public Canvas getHitBox() {
        return _canvas;
    }
}

