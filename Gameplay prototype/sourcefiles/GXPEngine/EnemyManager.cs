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
            enemy = new Enemy(i % 2, "assets\\green.png", 5, 1, _player);
            enemy.alpha = 0.5f;
            game.AddChild(enemy);
            _listOfEnemies.Add(enemy);
            enemy.SetXY(Utils.Random(0, 800), Utils.Random(0, 200));
        }
        getTwoRandomEnemies();
    }

    private void getTwoRandomEnemies() {
        try {
            _listOfEnemies[0].SetState(Fighter.State.WALKING);  // index 0 will always go left
            _listOfEnemies[1].SetState(Fighter.State.WALKING);  // index 1 will always go right

            //TODO: // Make it randomly selected, although not the same enemy should be selected twice
        }
        catch {
            // empty, dont kill us pls
        }
    }

    void Update() {
        try {
            if (_listOfEnemies[0].IsDestroyed()) {
                _listOfEnemies.RemoveAt(0);
                getTwoRandomEnemies();
            }
            if (_listOfEnemies[1].IsDestroyed()) {
                _listOfEnemies.RemoveAt(1);
                getTwoRandomEnemies();
            }
        } catch {
            // empty, dont kill us pls
        }
            

        if (enemy.GetState() == Fighter.State.FIGHTING) {
            enemy.SetState(Fighter.State.WAITING);
        }
        Console.WriteLine(enemy.GetState());
    }
}
