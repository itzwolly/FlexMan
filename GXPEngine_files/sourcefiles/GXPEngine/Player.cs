using System;
using System.Collections.Generic;
using GXPEngine;

public class Player : AnimationSprite {
    private int _health = 0;
    private Canvas _canvasRight, _canvasLeft;
    private bool _alive;
    private int _increment;
    private bool _shouldDealDamage = true;
    private bool _startAttackAnimation;
    private bool _canAttack = true;
    private int _velocity = 25;
    private int _direction = 0;

    public int Direction {
        get {
            return _direction;
        }
        set {
            _direction = value;
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
        DrawHitboxLeft();
        DrawHitboxRight();
        getHitboxLeft().visible = false;
    }

    void Update() {
        if (Input.GetKey(Key.RIGHT)) {
            if (!StartAttackAnimation) {
                getHitboxLeft().visible = false;
                getHitboxRight().visible = true;
                Mirror(true, false);
                x += 4f;
            }
        } else if (Input.GetKey(Key.LEFT)) {
            if (!StartAttackAnimation) {
                getHitboxLeft().visible = true;
                getHitboxRight().visible = false;
                Mirror(false, false);
                x -= 4f;
            }
        }

        if (Input.GetKey(Key.UP)) {
            if (!StartAttackAnimation) {
                y -= 4f;
            }
        } else if (Input.GetKey(Key.DOWN)) {
            if (!StartAttackAnimation) {
                y += 4f;
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
        if (_increment == 12) {
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

    private void DrawHitboxRight() {
        _canvasRight = new Canvas(105, 40);
        _canvasRight.x -= (width / 2) - width - 25 ;
        _canvasRight.width += (_canvasRight.width - 75);
        _canvasRight.y -= (height / 2) - 25;
        _canvasRight.height *= 8;
        _canvasRight.graphics.FillRectangle(System.Drawing.Brushes.Red, 0, 0, width, 40);
        _canvasRight.alpha = 0.33f;
        AddChild(_canvasRight);
    }

    private void DrawHitboxLeft() {
        _canvasLeft = new Canvas(105, 40);
        _canvasLeft.x -= (width / 2) - width - 75;
        _canvasLeft.width -= (_canvasLeft.width + 150);
        _canvasLeft.y -= (height / 2) - 25;
        _canvasLeft.height *= 8;
        _canvasLeft.graphics.FillRectangle(System.Drawing.Brushes.Red, 0, 0, width, 40);
        _canvasLeft.alpha = 0.33f;
        AddChild(_canvasLeft);
    }

    public Canvas getHitboxRight() {
        return _canvasRight;
    }

    public Canvas getHitboxLeft() {
        return _canvasLeft;
    }
}

