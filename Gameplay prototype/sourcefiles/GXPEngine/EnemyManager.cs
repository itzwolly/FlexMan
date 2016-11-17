using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class EnemyManager : GameObject
{
    List<Enemy> _listOfEnemies = new List<Enemy>();
    Enemy enemy;
    Player _player;

    public EnemyManager(Player pPlayer) {
        _player = pPlayer;
    }

    public void createEnemies() {
        for (int i = 0; i < 5; i++) {
            enemy = new Enemy("assets\\green.png", 5, 1, _player);
            enemy.alpha = 0.5f;
            game.AddChild(enemy);
            _listOfEnemies.Add(enemy);
            try {
                _listOfEnemies[0].Type = 1; // type 1 is right
                _listOfEnemies[1].Type = 2; // type 2 is left
            } catch {

            }
            enemy.SetXY(Utils.Random(0, 800), Utils.Random(0, 200));
        }
        getTwoRandomEnemies();
    }

    private void getTwoRandomEnemies() {
        try {
            _listOfEnemies[0].SetState(Fighter.State.WALKING);  // index 0 will always go left
            _listOfEnemies[1].SetState(Fighter.State.WALKING);  // index 1 will always go right
        } catch {
            // empty, dont kill us pls
        }
    }

    void Update() {
        try {
            if (_listOfEnemies[0].IsDestroyed()) {
                _listOfEnemies.RemoveAt(0);
                _listOfEnemies[0].Type = 1; // type 1 is right
                getTwoRandomEnemies();
            }
            if (_listOfEnemies[1].IsDestroyed()) {
                _listOfEnemies.RemoveAt(1);
                _listOfEnemies[1].Type = 2; // type 2 is left
                getTwoRandomEnemies();
            }
        } catch {
            // empty, dont kill us pls
        }

        if (enemy.GetState() == Fighter.State.FIGHTING) {
            enemy.SetState(Fighter.State.WAITING);
        }
    }
}
