using System;
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
    public int score;
    public int _health = 0;
    private State _state;
    private int timer;
    public bool _invincible = false;
    Canvas _collisionHitBox;

    public enum State {
        FIGHTING,
        WALKING,
        WAITING
    }

    public Fighter(string spriteName, int col, int row) : base(spriteName, col, row) {
            Mirror(true, false);
            score = 0;
            SetOrigin(width / 2, height);
            hand = CreateHand();
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
        if (Input.GetKeyDown(Key.R))
        {
            _collisionHitBox.visible = false;
        }

        if (Input.GetKeyDown(Key.T))
        {
            _collisionHitBox.visible = true;
        }
    }

    private void UpdateHit() {
        
        if (hitTimer > 0) {
            CheckHitCollision();
            hitTimer--;
            if (hitTimer == 0) {
                EndHit();
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
            //Console.WriteLine(item);
            //Console.WriteLine(item.GetType());
            //Console.WriteLine(this.GetType());
            //Console.WriteLine(item.y);
            //Console.WriteLine(y);
            if (item == this) continue;
            if (item is Fighter && item.y + 100 >= y && item.y - 100 <= y && (item as Fighter)._invincible == false) { 
                Fighter fighter = item as Player;
                item.x -= scaleX * 25;             // Player gets knockbacked
                (item as Fighter)._health--;
                (item as Fighter).turnInvurnerable();
                score++;
                Sound hitSound = new Sound("hit_sound.wav", false, false);
                hitSound.Play();
            }
        }
    }

    protected void Walk(float moveX, float moveY) {
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

    protected Canvas CreateHitBox()
    {
        _collisionHitBox = new Canvas(this.width, 20);
        _collisionHitBox.SetOrigin(_collisionHitBox.width / 2, _collisionHitBox.height / 2);
        _collisionHitBox.graphics.FillRectangle(Brushes.Red, this.x, this.y, this.width, 20);
        _collisionHitBox.alpha = 0.33f;
        AddChild(_collisionHitBox);
        _collisionHitBox.y = -23; 
        _collisionHitBox.x -= -15;

        

        
        return _collisionHitBox;
    }

    public Canvas GetHitBox()
    {
        return _collisionHitBox;
    }

    public int GetHealth()
    {
        return _health;
    }
}
