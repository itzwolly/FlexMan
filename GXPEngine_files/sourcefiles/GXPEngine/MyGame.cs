using System;
using System.Drawing;
using GXPEngine;
using System.Timers;
using System.Collections.Generic;

public class MyGame : Game //MyGame is a Game
{
	Enemy _enemy, _anotherEnemy;
	Player _player;
	HUD _hud;
	int _interval;
	int _cooldownTimer;
	List<Enemy> _enemyList = new List<Enemy>();

	//initialize game here
	public MyGame () : base(1024, 768, false, false)
	{
		targetFps = 60;

		_enemy = new Enemy("Assets\\hit_00.png", 7, 1);
		_anotherEnemy = new Enemy("Assets\\hit_00.png", 7, 1);
		_player = new Player("assets\\hit_00.png", 7, 1);

		_hud = new HUD(_player, _enemy);
		_hud.x = (game.width / 2) - 100;
		_hud.y = game.height / 2;

		_anotherEnemy.x = 300;
		_anotherEnemy.y = 400;

		_enemyList.Add(_enemy);
		_enemyList.Add(_anotherEnemy);

		AddChild(_enemy);
		AddChild(_anotherEnemy);

		AddChild(_player);
		AddChild(_hud);
	}
	
	//update game here
	void Update() {
		Console.WriteLine(currentFps);
		foreach (Enemy singleEnemy in _enemyList) {
			if (singleEnemy.getHitBox().HitTest(_player.getHitBox())) {
				_interval++;
				if (singleEnemy.CanAttack) { // standard true
					if (_interval >= 15) {
						_interval = 0;
						singleEnemy.StartAttackAnimation = true;
					}
					if (singleEnemy.currentFrame == 3) {
						if (singleEnemy.ShouldDealDamage) {
							//_player.CanAttack = false;
							_player.x -= _player.GetVelocity;
							_player.takeDamage();
							singleEnemy.ShouldDealDamage = false;
						}
					}
				} else {
					//_cooldownTimer++;
					singleEnemy.currentFrame = 0;
					singleEnemy.StartAttackAnimation = false;
					//if (_player.currentFrame != 3 && _cooldownTimer >= 505) {
					//    _enemy.CanAttack = true;
					//    _cooldownTimer = 0;
					//}
				}
			} else {
				if (singleEnemy.currentFrame == 0) {
					singleEnemy.StartAttackAnimation = false;
				}
			}

			if (_player.getHitBox().HitTest(singleEnemy.getHitBox())) {
				if (_player.CanAttack) {
					if (_player.StartAttackAnimation) {
						if (_player.CanAttack) {
							_player.DoAttackAnimation();
						}
						if (_player.currentFrame == 3) {
							if (_player.ShouldDealDamage) {
								singleEnemy.CanAttack = false;
								singleEnemy.x += singleEnemy.GetVelocity;
								singleEnemy.currentFrame = 0;
								_player.doDamage(singleEnemy);
								_player.ShouldDealDamage = false;
							}
						}
					} else {
						_player.CanAttack = true;
					}
				} else {
					_player.currentFrame = 0;
				}
			} else {
				if (_player.StartAttackAnimation) {
					_player.DoAttackAnimation();
					if (_player.currentFrame == 6) {
						_player.currentFrame = 0;
						_player.StartAttackAnimation = false;
					}
				}
			}
			if (singleEnemy.currentFrame == 4) {
				singleEnemy.ShouldDealDamage = true;
			}

			if (singleEnemy.StartAttackAnimation) {
				singleEnemy.DoAttackAnimation();
			} else {
				singleEnemy.CanAttack = true;
			}
		}

		if (_player.currentFrame == 4) {
			_player.ShouldDealDamage = true;
		}
		if (_player.currentFrame == 6) {
			_player.currentFrame = 0;
			_player.StartAttackAnimation = false;
		}


		//if (_player.x + 64 < _enemy.x) {
		//    _enemy.x--;
		//}
	}

	//system starts here
	static void Main() 
	{
		new MyGame().Start();
	}
}
