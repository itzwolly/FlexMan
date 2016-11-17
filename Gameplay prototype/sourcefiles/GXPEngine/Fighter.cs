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
    public int hit = 0;
    private State _state;

    public enum State {
        FIGHTING,
        WALKING,
        WAITING
    }

    public Fighter(string spriteName, int col, int row)
        : base(spriteName, col, row) {
            Mirror(true, false);
            currentFrame = 0;
            score = 0;
            SetOrigin(width / 2, height);
            hand = CreateHand();
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
        hand.x = -48;
        hand.y = -48;
        hand.visible = false;
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
            //Console.WriteLine(item.GetType());
            //Console.WriteLine(this.GetType());
            //Console.WriteLine(item.y);
            //Console.WriteLine(y);
            if (item == this) continue;
            if (item is Fighter && item.y + 10 >= y && item.y - 10 <= y) {
                item.x -= scaleX * 100;             // Player gets knockbacked
                (item as Fighter).hit++;
                score++;
            }
        }
    }

    public void Walk(float moveX, float moveY) {
        if (isHitting == false && GetState() == State.WALKING) {
            x += moveX;
            y += moveY;
            if (moveX > 0) scaleX = -1.0f;          // Mirror the sprite the correct way
            if (moveX < 0) scaleX = 1.0f;
        }
    }

    private void EndHit() {
        isHitting = false;
        hand.visible = false;
    }

    protected void Hit() {
        SetState(State.FIGHTING);
        if (isHitting == false && GetState() == State.FIGHTING) {
            isHitting = true;
            hand.visible = true;
            Sound hitSound = new Sound("hit_sound.wav", false, false);
            hitSound.Play();
            hitTimer = HIT_DURATION;
        }
        SetState(State.WALKING);                    // This enables the enemies in the fighting state to continue moving after hitting
    }
}
