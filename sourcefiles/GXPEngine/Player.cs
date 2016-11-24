using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;


public class Player : Fighter
{
    const int HEALTH_INCREMENT = 5;
    int leftKey, rightKey, upKey, downKey, hitKey, pickUpKey;
    //public float oldX, oldY;
    public bool isColliding = false;
    public bool hasPickedUp = false;
    public int playerAttackTimer = 0;
    //bool startAttackTimer = false;
    public int playerAttackAmount = 0;
    bool allowedToHit = true;
    int hitCount = 0;
    int hitDelayTimer = 0;
    public int direction = 1; // 0 = left, 1 = right
    Sound playerDeath;
    public bool isGoingLeft, isGoingRight;

    public Player(string spriteName, int leftKey, int rightKey, int upKey, int downKey, int hitKey, int pickUpKey, int col, int row) : base(spriteName, col, row) {
        this.leftKey = leftKey;
        this.rightKey = rightKey;
        this.downKey = downKey;
        this.upKey = upKey;
        this.hitKey = hitKey;
        this.pickUpKey = pickUpKey;
        Name = "Flexman";
        _health = 30;
        _maxHealth = _health;
        Stamina = 100;
        _maxStamina = Stamina;
        playerDeath = new Sound("assets\\sfx\\player_dead.wav", false, false);
    }

    void Update()
    {
        base.Update();

        oldX = x;
        oldY = y;

        isGoingLeft = false;
        isGoingRight = false;

        if (Input.GetKey(leftKey)) {
            direction = 0;
            SetState(Fighter.State.WALKING);
            isGoingLeft = true;
            isGoingRight = false;
            if (!hasPickedUp) {
                comboAttackCount = 0;
                Walk(-5, 0);
            } else {
                comboAttackCount = 0;
                Walk(-1, 0);
            }
            
        }
        else if (Input.GetKey(rightKey)) {
            direction = 1;
            SetState(Fighter.State.WALKING);
            isGoingLeft = false;
            isGoingRight = true;
            if (!hasPickedUp) {
                comboAttackCount = 0;
                Walk(5, 0);
            } else {
                comboAttackCount = 0;
                Walk(1, 0);
            }
        }

        oldX = x;
        oldY = y;
        if (Input.GetKey(upKey)) {
            SetState(Fighter.State.WALKING);
            if (!hasPickedUp) {
                Walk(0, -5);
            } else {
                Walk(0, -1);
            }
        }
        else if (Input.GetKey(downKey)) {
            SetState(Fighter.State.WALKING);
            if (!hasPickedUp) {
                Walk(0, 5);
            } else {
                Walk(0, 1);
            }
        }

        if (Input.GetKeyDown(hitKey)) {
             //some form of delay
            if (!hasPickedUp && allowedToHit) {
                comboAttackCount++;
                hitCount++;
                if (hitCount == 3) {
                    allowedToHit = false;
                    hitCount = 0;
                }
                Hit();
            }
            //Hit();
        }
         //some form of delay PART 2: the DELAYING
        if (!allowedToHit) {
            hitDelayTimer++;
            if (hitDelayTimer == 35) {
                allowedToHit = true;
                hitDelayTimer = 0;
            }
        }

        if (Input.GetKeyDown(pickUpKey)) {
            if (!hasPickedUp) {
                PickUpObject();
            } else {
                GetPickedUpEnemy().SetState(State.THROWN); // enemy state, delay is inside.
                hasPickedUp = false;
                SetState(Fighter.State.WALKING); // player state
            }
        }
        if (_health <= 0) {
            Destroy();
            playerDeath.Play();
        }

        foreach(GameObject item in GetCollisions())
        {
            if (item is Item)
            {
                PickUpItem((item as Item));
            }
        }
    }

    private void PickUpItem(Item other)
    {
        other.Destroy();
        int difference = GetMaxHealth() - GetHealth();

        if (difference > 0)                      // if there is a difference
        {
            if (difference < HEALTH_INCREMENT)   // and the difference is smaller than the health increment (which is 10)
            {
                _health += difference;           // add the difference to health as increment
            } else
            {
                _health += HEALTH_INCREMENT;
            }
        }
    }
}
