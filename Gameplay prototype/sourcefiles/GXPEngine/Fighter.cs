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
    Enemy _pickedUpEnemy;
    int _stamina;
    bool attackOnce = false;
    public float oldX, oldY;
    int disabledTimer = 0;

    public enum State {
        FIGHTING,
        WALKING,
        WAITING,
        DISABLED,
        PICKEDUP,
        THROWN
    }

    public string Name { get { return charName; } set { charName = value; } }
    public bool IsEnemyHitByPlayer { get { return _enemyHitByPlayer; } set { _enemyHitByPlayer = value; } }
    public int Stamina { get { return (_stamina < 100 ? _stamina : 100); } set { _stamina = value; } }

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

        DisableFighter(true, 30);

        if (!isPickedUp) {
            staminaTimer++;
            if (staminaTimer == 150) {
                staminaTimer = 0;
                if (Stamina < 100) {
                    Stamina += 10;
                }
            }
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

    public void DisableFighter(bool isEnemy, int amount) {
        if (GetState() == State.DISABLED) {
            disabledTimer++;
            if (disabledTimer == 35) {
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
            //Console.WriteLine(item); // item is fighter getting hit, so if player hits enemy than item is enemy.
            //Console.WriteLine(item.GetType());
            //Console.WriteLine(this.GetType());
            //Console.WriteLine(item.y);
            //Console.WriteLine(y);
            if (item is Enemy && hand.parent is Enemy) {
                continue;
            }
            if (item == this) continue;
            if (item is Fighter && item.y + 20 >= y && item.y - 20 <= y) { // && fighter.invincible == false
                isHitting = true;
                if (item is Enemy) { 
                    Enemy enem = item as Enemy;
                    enem.IsEnemyHitByPlayer = true; // base for new functionality, doesnt work yet!
                    if (isPickedUp && (hand.parent as Player).hasPickedUp == false && HasEnoughStamina()) {
                        (hand.parent as Player).hasPickedUp = true;
                        Stamina -= 50;
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
                    //(item as Fighter).turnInvurnerable();
                    //hitSound.Play();
                }
                if (item is Enemy) {
                    (item as Enemy).gotHitAmount++;
                    if (isHitting && !isPickedUp && (item as Enemy).gotHitAmount == 3) {
                        (item as Enemy).oldX = item.x;
                        item.x -= scaleX * 150; // Fighter gets knockbacked
                        item.y -= (item as Fighter).height / 3;
                        if (item.x < (item as Fighter).oldX) {
                            item.rotation = 270;
                        } else {
                            item.rotation = 90;
                        }
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
        if (isHitting == false && GetState() == State.WALKING) {
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
        if (GetState() == State.WALKING)
        {
            SetState(State.FIGHTING);
        }
        if (isHitting == false && GetState() == State.FIGHTING) {
            isHitting = true;
            hand.visible = true;
            hitTimer = HIT_DURATION;
            currentFrame = 6;
        }
        SetState(State.WALKING);                    // This enables the enemies in the fighting state to continue moving after hitting
    }

    protected void HandleGenericGuyHit() {
        if (GetState() == State.WALKING) {
            SetState(State.FIGHTING);
        }
        if (isHitting == false && GetState() == State.FIGHTING) {
            isHitting = true;
            hand.visible = true;
            hitTimer = HIT_DURATION;
            currentFrame = 6;
        }
        SetState(State.WALKING);                    // This enables the enemies in the fighting state to continue moving after hitting
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

    public int GetScore() {
        return score;
    }

    public int GetHealth()
    {
        return (_health < 0 ) ? 0 : _health;
    }

    public int GetMaxHealth() {
        return _maxHealth;
    }
}
