using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;

public class Fighter : AnimationSprite
{
    const int HIT_DURATION = 1;
    Sprite hand;
    int hitTimer = 0;
    bool isHitting = false;
    public int score;
    public int hit = 0;
    private State _state;
    private int timer;
    Canvas _collisionHitBox;

    public enum State {
        FIGHTING,
        WALKING,
        WAITING
    }

    public Fighter(string spriteName, int col, int row)
        : base(spriteName, col, row) {
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


    private void CheckHitCollision() {
        foreach (GameObject item in hand.GetCollisions()) {
            //Console.WriteLine(item);
            //Console.WriteLine(item.GetType());
            //Console.WriteLine(this.GetType());
            //Console.WriteLine(item.y);
            //Console.WriteLine(y);
            if (item == this) continue;
            if (item is Fighter && item.y + 100 >= y && item.y - 100 <= y) {
                Fighter fighter = item as Player;
                item.x -= scaleX * 25;             // Player gets knockbacked
                (item as Fighter).hit++;
                score++;
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
            if (timer > 15)
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

    private void EndHit() {
        isHitting = false;
        hand.visible = false;
    }

    protected void Hit() {
        if (GetState() == State.WALKING)
        {
            SetState(State.FIGHTING);
        }
        if (isHitting == false && GetState() == State.FIGHTING) {
            isHitting = true;
            hand.visible = true;
            Sound hitSound = new Sound("hit_sound.wav", false, false);
            hitSound.Play();
            hitTimer = HIT_DURATION;
            //currentFrame = 6;
            //timer++;
            //if (timer > 5)
            //{
            //    NextFrame();
            //    timer = 0;
            //}
            //if (currentFrame == 8)
            //{
            //    currentFrame = 0;
            //}
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
        _collisionHitBox.x -= -5;
        
        return _collisionHitBox;
    }

    public Canvas GetHitBox()
    {
        return _collisionHitBox;
    }
}
