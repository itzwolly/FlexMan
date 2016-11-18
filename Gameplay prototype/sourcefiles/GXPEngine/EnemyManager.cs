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
            enemy = new Enemy(i % 2, "red.png", 8, 1, _player);
            //enemy.alpha = 0.5f;
            game.AddChildAt(enemy, 1);
            _listOfEnemies.Add(enemy);
            enemy.SetXY(Utils.Random(_player.width, 1024), Utils.Random(400, game.height - 120));
        }
        //GetTwoRandomEnemies();
    }

    private void GetTwoRandomEnemies() {
        try {
            
                _listOfEnemies[0].SetState(Fighter.State.WALKING);  // index 0 will always go left
                _listOfEnemies[1].SetState(Fighter.State.WALKING);  // index 1 will always go right
            
        }
        catch {
            // empty, dont kill us pls
        }
    }

    void Update() {

        try {
            if (_listOfEnemies[0].IsDestroyed()) {
                _listOfEnemies.RemoveAt(0);
                GetTwoRandomEnemies();
            }
            if (_listOfEnemies[1].IsDestroyed()) {
                _listOfEnemies.RemoveAt(1);
                GetTwoRandomEnemies();
            }
        } catch {
            // empty, dont kill us pls
        }
            

        if (enemy.GetState() == Fighter.State.FIGHTING) {
            enemy.SetState(Fighter.State.WAITING);
        }
        
    }

    public List<Enemy> GetAllEnemies()
    {
        return _listOfEnemies;
    }
}
