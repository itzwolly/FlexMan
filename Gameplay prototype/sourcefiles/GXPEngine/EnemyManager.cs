using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Collections.ObjectModel;

public class EnemyManager : GameObject
{
    List<Enemy> _listOfEnemies = new List<Enemy>();
    ObservableCollection<Enemy> _dynamicEnemyList;
    Enemy enemy;
    Player _player;
    int[] enemyPosition = new int[] { 0, 0 };

    public EnemyManager(Player pPlayer) {
        _player = pPlayer;
    }

    public void createEnemies() {
        _dynamicEnemyList = new ObservableCollection<Enemy>();

        for (int i = 0; i < 8; i++) {
            enemy = new Enemy("red.png", 8, 1, _player);
            game.AddChild(enemy);
            _listOfEnemies.Add(enemy);
            try {
                _listOfEnemies[0].Type = 1; // type 1 is right
                _listOfEnemies[0].SetState(Fighter.State.WALKING);
                enemyPosition[0] = 1;

                _listOfEnemies[1].Type = 2; // type 2 is left
                _listOfEnemies[1].SetState(Fighter.State.WALKING);
                enemyPosition[1] = 1;
            } catch {
                // empty, dont kill us pls
            }
            enemy.SetXY(Utils.Random(105, 1024 - enemy.width), Utils.Random(400, 600));
        }
    }

    private void GetNextRandomEnemy() {
        try {
            if (enemyPosition[0] == 0) {
                _listOfEnemies[1].Type = 1;
                _listOfEnemies[1].SetState(Fighter.State.WALKING);
                enemyPosition[0] = 1;
            } else if (enemyPosition[1] == 0) {
                _listOfEnemies[1].Type = 2;
                _listOfEnemies[1].SetState(Fighter.State.WALKING);
                enemyPosition[1] = 1;
            }
        } catch {
            // empty
        }
    }

    public Enemy GetEnemyHitByPlayer() { // base for new functionality, doesnt work yet!
        foreach (Enemy enem in _listOfEnemies) {
            if (enem.IsEnemyHitByPlayer) {
                return enem;
            }
        }
        return null;
    }

    void Update() {
        try {
            if (_listOfEnemies[0].IsDestroyed()) { // if enemy dies
                _dynamicEnemyList.Add(_listOfEnemies[0]);
                if (_listOfEnemies[0].Type == 1) { // and is right side
                    enemyPosition[0] = 0; // set right side to empty
                    _listOfEnemies.RemoveAt(0); // remove enemy from list
                    GetNextRandomEnemy(); // get new enemy
                } else if (_listOfEnemies[0].Type == 2) { // and is on left side
                    enemyPosition[1] = 0; // set left side to empty
                    _listOfEnemies.RemoveAt(0); // remove enemy from list
                    GetNextRandomEnemy(); // get new enemy
                } 
                
            } else if (_listOfEnemies[1].IsDestroyed()) {
                _dynamicEnemyList.Add(_listOfEnemies[1]);
                if (_listOfEnemies[1].Type == 1) {
                    enemyPosition[0] = 0;
                    _listOfEnemies.RemoveAt(1);
                    GetNextRandomEnemy();
                } else if (_listOfEnemies[1].Type == 2) {
                    enemyPosition[1] = 0;
                    _listOfEnemies.RemoveAt(1);
                    GetNextRandomEnemy();
                }
            }
        } catch {
            // empty, dont kill us pls
        }
    }

    public Enemy GetEnemy() {
        return enemy;
    }

    public List<Enemy> GetAllEnemies()
    {
        return _listOfEnemies;
    }

    public ObservableCollection<Enemy> GetDeadEnemyList() {
        return _dynamicEnemyList; // dead enemy list
    }
}
