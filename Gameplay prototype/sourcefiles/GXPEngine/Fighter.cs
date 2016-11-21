﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;

public class Fighter : AnimationSprite
{
    const int HIT_DURATION = 25;
    Sprite hand;
    int hitTimer = 0;
    bool isHitting = false;
    bool isPickingUp = false;
    public int score;
    public int _health = 0;
    public int _maxHealth = 0;
    private State _state;
    private int timer;
    protected bool _invincible = false;
    Canvas _collisionHitBox;
    string charName;
    Sound hitSound;
    bool _enemyHitByPlayer = false; // base for new functionality, doesnt work yet!
    int enemyHitTimer = 0; // base for new functionality, doesnt work yet!

    public enum State {
        FIGHTING,
        WALKING,
        WAITING,
        PICKEDUP
    }

    public string Name { get { return charName; } set { charName = value; } }
    public bool IsEnemyHitByPlayer { get { return _enemyHitByPlayer; } set { _enemyHitByPlayer = value; } }

    public Fighter(string spriteName, int col, int row) : base(spriteName, col, row) {
        Mirror(true, false);
        score = 0;
        SetOrigin(width / 2, height);
        hand = CreateHand();
        hitSound = new Sound("hit_sound.wav", false, false);
        _collisionHitBox = CreateHitBox();
    }

    public void SetState(State pState) {
        _state = pState;
    }

    public State GetState() {
        return _state;
    }

    protected Sprite CreateHand() {
        Sprite hand = new Sprite("colors.png");
        hand.SetOrigin(32, 32);
        hand.width = 32;
        hand.height = 32;
        AddChild(hand);
        hand.x = -50;
        hand.y = -100;
        hand.visible = false;
        hand.alpha = 0.33f;
        return hand;
    }

    protected void Update() {
        UpdateHit();
        hittingAnimation();

        enemyHitTimer++; // base for new functionality, doesnt work yet!
        if (enemyHitTimer == 6000) { // base for new functionality, doesnt work yet!
            enemyHitTimer = 0;
            IsEnemyHitByPlayer = false;
        }

        if (Input.GetKeyDown(Key.R))
        {
            _collisionHitBox.visible = true;
        }

        if (Input.GetKeyDown(Key.T))
        {
            _collisionHitBox.visible = false;
        }
    }

    private void UpdateHit() {
        if (hitTimer > 0) {
            CheckHitCollision();
            hitTimer--;
            if (hitTimer == 0) {
                EndHit();
                EndPickUp();
            }
        }
    }

    private void hittingAnimation()
    {
        if (isHitting == true)
        {
            timer++;
            if (timer > 10)
            {
                NextFrame();
                timer = 0;
            }
            if (currentFrame == 8)
            {
                currentFrame = 0;
            }
        }
    }
    private void CheckHitCollision() {
        foreach (GameObject item in hand.GetCollisions()) {
            //Console.WriteLine(item); // item is fighter getting hit, so if player hits enemy than item is enemy.
            //Console.WriteLine(item.GetType());
            //Console.WriteLine(this.GetType());
            //Console.WriteLine(item.y);
            //Console.WriteLine(y);
            if (item is Enemy && hand.parent is Enemy) {
                continue;
            }
            if (item == this) continue;
            if (item is Fighter && item.y + 100 >= y && item.y - 100 <= y && (item as Fighter)._invincible == false) {
                isHitting = true;
                if (isHitting && !isPickingUp) {
                    item.x -= scaleX * 40; // Fighter gets knockbacked
                }
                if (item is Enemy) { // base for new functionality, doesnt work yet!
                    Enemy enem = item as Enemy;
                    enem.IsEnemyHitByPlayer = true;
                    enem.isPickingUp = true;
                    enem.SetState(Fighter.State.PICKEDUP);
                }
                Console.WriteLine(isPickingUp);
                (item as Fighter)._health--;
                (item as Fighter).turnInvurnerable();
                hitSound.Play();
            }
        }
    }

    public void Walk(float moveX, float moveY) {
        if (isHitting == false && GetState() == State.WALKING) {
            x += moveX;
            y += moveY;
            if (moveX > 0) scaleX = -1.0f;          // Mirror the sprite the correct way
            if (moveX < 0) scaleX = 1.0f;
            timer++;
            if (timer > 8)
            {
                NextFrame();
                timer = 0;
            }
            if (currentFrame == 5)
            {
                currentFrame = 0;
            }
        }
    }

    private void turnInvurnerable()
    {
        _invincible = true;
        alpha = 0.3f;
        new Timer(1600, turnVurnerable); // Needs to be around 800 milliseconds, or else the player can pass the flame
    }
    private void turnVurnerable()
    {
        _invincible = false;
        alpha = 1f;
    }

    private void EndHit() {
        isHitting = false;
        hand.visible = false;
    }

    private void EndPickUp() {
        isPickingUp = false;
        hand.visible = false;
    }

    protected void Hit() {
        if (GetState() == State.WALKING)
        {
            SetState(State.FIGHTING);
        }
        if (isHitting == false && GetState() == State.FIGHTING /*IF THE OTHER GUY IS NOT INVINCIBLE*/) {
            isHitting = true;
            hand.visible = true;
            hitTimer = HIT_DURATION;
            currentFrame = 6;
        }
        SetState(State.WALKING);                    // This enables the enemies in the fighting state to continue moving after hitting
    }

    protected void PickUp() {
        if (GetState() == State.WALKING) {
            SetState(State.FIGHTING);
        }
        if (isPickingUp == false && GetState() == State.FIGHTING) {
            isPickingUp = true;
            hand.visible = true;
            hitTimer = HIT_DURATION;
            // set frame
        }
        // do something
    }

    protected Canvas CreateHitBox()
    {
        _collisionHitBox = new Canvas(this.width, 20);
        _collisionHitBox.SetOrigin(_collisionHitBox.width / 2, _collisionHitBox.height / 2);
        _collisionHitBox.graphics.FillRectangle(Brushes.Red, this.x, this.y, this.width, 20);
        _collisionHitBox.alpha = 0.33f;
        AddChild(_collisionHitBox);
        _collisionHitBox.y = -23; 
        _collisionHitBox.x -= -15;

        _collisionHitBox.visible = false;

        
        return _collisionHitBox;
    }

    public Canvas GetHitBox()
    {
        return _collisionHitBox;
    }

    public int GetScore() {
        return score;
    }

    public int GetHealth()
    {
        return _health;
    }

    public int GetMaxHealth() {
        return _maxHealth;
    }
}
