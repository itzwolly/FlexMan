using System;
using System.Collections.Generic;
using GXPEngine;

public class Player : AnimationSprite {
    private int _health = 0;
    private Canvas _canvas;
    private bool _alive;
    private int _increment;
    private bool _shouldDealDamage = true;
    private bool _startAttackAnimation;
    private bool _canAttack = true;
    private int _velocity = 100;

    public bool StartAttackAnimation {
        get {
            return _startAttackAnimation;
        }
        set {
            _startAttackAnimation = value;
        }
    }

    public bool ShouldDealDamage {
        get {
            return _shouldDealDamage;
        }
        set {
            _shouldDealDamage = value;
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

    public int GetVelocity {
        get {
            return _velocity;
        }
    }

    public Player(string filename, int cols, int rows) : base(filename, cols, rows) {
        _alive = true;
        _health = 100;
        scale = 0.35f;
        SetOrigin(width / 2, height / 2);
        Mirror(true, false);
        x = ((game.width / 2) - width * 2);
        y = game.height - height;
        DrawHitbox();
    }

    void Update() {
        if (Input.GetKey(Key.RIGHT)) {
            if (!StartAttackAnimation) {
                x += 0.5f;
            }
        }
        if (Input.GetKey(Key.LEFT)) {
            if (!StartAttackAnimation) {
                x -= 0.5f;
            }
        }
        if (Input.GetKeyDown(Key.SPACE)) {
            _startAttackAnimation = true;
        }
        if (_health == 0) {
            _alive = false;
        }
    }

    public void DoAttackAnimation() {
        _increment++;
        if (_increment == 100) {
            _increment = 0;
            NextFrame();
        }
    }

    public void doDamage(Enemy other) {
        other.takeDamage();
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
        _canvas.x -= (width / 2) - width - 25 ;
        _canvas.width += (_canvas.width - 75);
        _canvas.y -= (height / 2) - 25;
        _canvas.height *= 8;
        _canvas.graphics.FillRectangle(System.Drawing.Brushes.Red, 0, 0, width, 40);
        _canvas.alpha = 0.33f;
        AddChild(_canvas);
    }

    public Canvas getHitBox() {
        return _canvas;
    }
}

