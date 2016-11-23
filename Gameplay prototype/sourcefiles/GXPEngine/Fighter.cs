using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;

public class Fighter : Pausable
{
    const int HIT_DURATION = 25;

    Sprite hand;
    int hitTimer = 0;
    int staminaTimer = 0;
    bool isHitting = false;
    bool isPickedUp = false;
    public int score = 0;
    public int _health = 0;
    public int _maxHealth = 0;
    private State _state;
    private int timer;
    protected bool _invincible = false;
    Canvas _collisionHitBox;
    string charName;
    Sound hitOne, hitTwo, hitThree;
    int enemyHitCountTimer = 0;
    Enemy _pickedUpEnemy;
    int _stamina;
    public int _maxStamina;
    bool attackOnce = false;
    public float oldX, oldY;
    int disabledTimer = 0;
    int direction = 0;
    public int comboAttackCount = 0;

    public enum State {
        WAITING,
        FIGHTING,
        WALKING,
        DISABLED,
        PICKEDUP,
        THROWN,
        WAITAFTERTHROWN
    }

    public string Name { get { return charName; } set { charName = value; } }
    public int Stamina { get { return (_stamina < 100 ? _stamina : 100); } set { _stamina = value; } }
    public int Score {
        get {
            return score;
        }
        set {
            //oldScore = score; // removed cuz doesnt do what i want
            score = value;
        }
    }

    public Fighter(string spriteName, int col, int row) : base(spriteName, col, row) {
        Mirror(true, false);
        scaleX = -1f;
        score = 0;
        SetOrigin(width / 2, height);
        hand = CreateHand();

        hitOne = new Sound("assets\\sfx\\hit1.wav", false, false);
        hitTwo = new Sound("assets\\sfx\\hit2.wav", false, false);
        hitThree = new Sound("assets\\sfx\\hit3.wav", false, false);

        _collisionHitBox = CreateHitBox();
    }

    public virtual void SetState(State pState) {
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

        DisableFighter(true, 100);

        if (!isPickedUp) {
            staminaTimer++;
            if (staminaTimer == 150) {
                staminaTimer = 0;
                if (Stamina < 100) {
                    Stamina += 10;
                }
            }
        }
    }

    public void DisableFighter(bool isEnemy, int amount) {
        if (GetState() == State.DISABLED) {
            disabledTimer++;
            if (disabledTimer == amount) {
                // TODO: if enemy's previous state was waiting makes sure you reset to wait again.
                SetState(State.WALKING);
                if (isEnemy) {
                    y += height / 3;
                }
                disabledTimer = 0;
            }
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
            if (item is Enemy && hand.parent is Enemy) {
                continue;
            }
            if (item == this) continue;
            if (item is Fighter && item.y + 20 >= y && item.y - 20 <= y) { // && fighter.invincible == false
                isHitting = true;
                if (item is Enemy) { 
                    Enemy enem = item as Enemy;
                    if (isPickedUp && (hand.parent as Player).hasPickedUp == false && (hand.parent as Player).HasEnoughStamina()) {
                        (hand.parent as Player).hasPickedUp = true;
                        (hand.parent as Player).Stamina -= 50;
                        enem.SetState(State.PICKEDUP);
                        _pickedUpEnemy = enem;
                    }
                }
                if (attackOnce) {
                    return;
                }
                attackOnce = true;

                if (!isPickedUp) {
                    (item as Fighter)._health--;
                    //hitSound.Play();
                }
                if (item is Enemy) {
                    //(item as Enemy).gotHitAmount++;
                    //if (isHitting && !isPickedUp && (item as Enemy).gotHitAmount == 3) {
                    //    item.x -= scaleX * 150; // Fighter gets knockbacked
                    //    item.y -= (item as Fighter).height / 3;
                    //    (item as Enemy).SetState(Fighter.State.DISABLED);
                    //}

                    if (comboAttackCount == 1) {
                        switch (Utils.Random(0, 2)) {
                            case 0:
                                hitOne.Play();
                                break;
                            case 1:
                                hitTwo.Play();
                                break;
                        }
                    } else if (comboAttackCount == 2) {
                        switch (Utils.Random(0, 2)) {
                            case 0:
                                hitOne.Play();
                                break;
                            case 1:
                                hitTwo.Play();
                                break;
                        }
                    } else if (comboAttackCount == 3) {
                        hitThree.Play();
                    }

                    if ((hand.parent as Player).comboAttackCount == 3) {
                        (hand.parent as Player).comboAttackCount = 0;
                        item.x -= scaleX * 150; // Fighter gets knockbacked
                        item.y -= (item as Fighter).height / 3;
                        (item as Enemy).SetState(Fighter.State.DISABLED);
                    }
                }
            }
        }
    }
    
    public bool HasEnoughStamina() {
        if (Stamina - 50 >= 0) {
            return true;
        } else {
            return false;
        }
    }

    public Enemy GetPickedUpEnemy() {
        return _pickedUpEnemy;
    }

    public void Walk(float moveX, float moveY) {
        if (isHitting == false && isPickedUp == false && GetState() == State.WALKING) {
            x += moveX;
            y += moveY;
            if (moveX > 0) {
                scaleX = -1.0f; 
            }          // Mirror the sprite the correct way
            if (moveX < 0) {
                scaleX = 1.0f; 
            }
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
        attackOnce = false;
    }

    private void EndPickUp() {
        isPickedUp = false;
        hand.visible = false;
    }

    protected void Hit() {
        if (GetState() == State.WALKING) {
            SetState(State.FIGHTING);
        }
        if (GetState() == State.PICKEDUP) {
            return;
        }
        if (isHitting == false && GetState() == State.FIGHTING) {
            isHitting = true;
            hand.visible = true;
            hitTimer = HIT_DURATION;
            currentFrame = 6;
        }
        //SetState(State.WALKING);                    // This enables the enemies in the fighting state to continue moving after hitting
    }

    protected void EnemyHit() {
        if (GetState() == State.PICKEDUP) {
            return;
        }

        enemyHitCountTimer++;
        if (isHitting == false && GetState() == State.FIGHTING && enemyHitCountTimer == 85) {
            enemyHitCountTimer = 0;
            isHitting = true;
            hand.visible = true;
            hitTimer = HIT_DURATION;
            currentFrame = 6;
        }
        //SetState(State.WALKING);                    // This enables the enemies in the fighting state to continue moving after hitting
    }

    protected void PickUpObject() {
        if (GetState() == State.WALKING) {
            SetState(State.FIGHTING);
        }
        if (isPickedUp == false && HasEnoughStamina() && GetState() == State.FIGHTING) {
            isPickedUp = true;
            hand.visible = true;
            hitTimer = HIT_DURATION;
        }
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

    public int GetHealth()
    {
        return (_health < 0 ) ? 0 : _health;
    }

    public int GetMaxHealth() {
        return _maxHealth;
    }

    public int GetStamina() {
        return (_stamina < 100 ? _stamina : 100);
    }

    public int GetMaxStamina() {
        return _maxStamina;
    }
}
