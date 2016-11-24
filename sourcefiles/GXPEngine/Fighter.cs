using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;

public class Fighter : Pausable
{
    const int HIT_DURATION = 29;
    const int PICKUP_DURATION = 29;

    Sprite hand;
    int hitTimer = 0;
    int staminaTimer = 0;
    bool isHitting = false;
    bool isPickedUp = false;
    public int score = 0;
    public int _health = 0;
    public int _maxHealth = 0;
    private State _state;
    protected bool _invincible = false;
    Canvas _collisionHitBox;
    string charName;
    Sound hitOne, hitTwo, hitThree, comboHitOne, comboHitTwo, comboHitThree;
    SoundChannel comboHit;
    int enemyHitCountTimer = 0;
    Enemy _pickedUpEnemy;
    int _stamina;
    public int _maxStamina;
    bool attackOnce = false;
    public float oldX, oldY;
    int disabledTimer = 0;
    int direction = 0;
    public int comboAttackCount = 0;
    Sound pickUpOne, pickUpTwo;
    public bool hitAnimCheck = false;
    public bool pickUpCheck = false;
    public bool throwCheck = false;
    int hitAnimTimer = 0;
    int pickUpTimer = 0;
    int throwTimer = 0;
    public bool hasPickedUp = false;
    public bool startThrowAnimation = false;

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

        comboHitOne = new Sound("assets\\sfx\\combohit1.wav", false, false);
        comboHitTwo = new Sound("assets\\sfx\\combohit2.wav", false, false);
        comboHitThree = new Sound("assets\\sfx\\combohit2.wav", false, false);

        pickUpOne = new Sound("assets\\sfx\\pick1.wav", false, false);
        pickUpTwo = new Sound("assets\\sfx\\pick2.wav", false, false);

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

        if (startThrowAnimation) {
            ThrowAnimation();
        } else {
            PickingUpAnimation();
        }

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

    public void ThrowAnimation() {
        if (x == oldX) {
            // 39 - 59
            if (!throwCheck) {
                currentFrame = 39;
            }
            throwCheck = true;

            if (currentFrame > 58) {
                startThrowAnimation = false;
                currentFrame = 0;
                return;
            }

            throwTimer++;
            if (throwTimer > 2) {
                NextFrame();
                throwTimer = 0;
            }
        }
    }

    private void PickingUpAnimation() {
        if (isPickedUp) {
            if (x == oldX) {
                if (!pickUpCheck) {
                    currentFrame = 20;
                }
                pickUpCheck = true;

                if (currentFrame > 27) {
                    return;
                }

                pickUpTimer++;
                if (pickUpTimer > 1) {
                    NextFrame();
                    pickUpTimer = 0;
                }
            }
        }
    }

    private void hittingAnimation() {
        if (isHitting == true) {
            if (x == oldX) {
                if (!hitAnimCheck) {
                    currentFrame = 98;
                }
                hitAnimCheck = true;

                hitAnimTimer++;
                if (hitAnimTimer > 1) {
                    NextFrame();
                    hitAnimTimer = 0;
                }
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
                        switch (Utils.Random(0, 2)) {
                            case 0:
                                pickUpOne.Play();
                                break;
                            case 1:
                                pickUpTwo.Play();
                                break;
                        }
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
                    if (comboAttackCount == 1) {
                        switch (Utils.Random(0, 2)) {
                            case 0:
                                hitOne.Play();
                                comboHit = comboHitOne.Play();
                                break;
                            case 1:
                                hitTwo.Play();
                                comboHit = comboHitTwo.Play();
                                break;
                        }
                    } else if (comboAttackCount == 2) {
                        switch (Utils.Random(0, 2)) {
                            case 0:
                                hitOne.Play();
                                comboHit = comboHitOne.Play();
                                break;
                            case 1:
                                hitTwo.Play();
                                comboHit = comboHitTwo.Play();
                                break;
                        }
                    } else if (comboAttackCount == 3) {
                        hitThree.Play();
                        comboHit = comboHitThree.Play();
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

    public virtual void Walk(float moveX, float moveY) {
        if (isHitting == false && isPickedUp == false && GetState() == State.WALKING) {
            x += moveX;
            y += moveY;
            if (moveX > 0) {
                scaleX = -1.0f; 
            }          // Mirror the sprite the correct way
            if (moveX < 0) {
                scaleX = 1.0f; 
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
        if (!hasPickedUp) {
            currentFrame = 0;
        } 
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
            hitTimer = HIT_DURATION;
        }
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
        }
    }

    protected virtual void PickUpObject() {
        if (GetState() == State.WALKING) {
            SetState(State.FIGHTING);
        }
        if (isPickedUp == false && HasEnoughStamina() && GetState() == State.FIGHTING) {
            isPickedUp = true;
            hitTimer = PICKUP_DURATION;
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
