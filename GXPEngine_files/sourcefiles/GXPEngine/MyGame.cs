using System;
using System.Drawing;
using GXPEngine;
using System.Timers;

public class MyGame : Game //MyGame is a Game
{
	Enemy _enemy;
	Player _player;
	HUD _hud;
	int _interval;

	//initialize game here
	public MyGame () : base(1024, 768, false)
	{
		//targetFps = 30;

		_enemy = new Enemy("Assets\\hit_00.png", 7, 1);
		_player = new Player("assets\\hit_00.png", 7, 1);

		_hud = new HUD(_player, _enemy);
		_hud.x = (game.width / 2) - 100;
		_hud.y = game.height / 2;

		AddChild(_enemy);
		AddChild(_player);
		AddChild(_hud);
	}
	
	//update game here
	void Update() {
		if (_enemy.getHitBox().HitTest(_player.getHitBox())) {
			_interval++;
			if (_enemy.CanAttack) { // standard true
				if (_interval >= 125) {
					_interval = 0;
					_enemy.StartAttackAnimation = true;
				}
				if (_enemy.currentFrame == 3) {
					if (_enemy.ShouldDealDamage) {
						//_player.CanAttack = false;
						_player.x -= _player.GetVelocity;
						_player.takeDamage();
						_enemy.ShouldDealDamage = false;
					}
				}
			} else {
				_enemy.currentFrame = 0;
			}
		} else {
			if (_enemy.currentFrame == 0) {
				_enemy.StartAttackAnimation = false;
			}
		}
		if (_enemy.currentFrame == 4) {
			_enemy.ShouldDealDamage = true;
		}

		if (_enemy.StartAttackAnimation) {
			_enemy.DoAttackAnimation();
		} else {
			_enemy.CanAttack = true;
		}

		if (_player.getHitBox().HitTest(_enemy.getHitBox())) {
			if (_player.CanAttack) {
				if (_player.StartAttackAnimation) {
					if (_player.CanAttack) {
						_player.DoAttackAnimation();
					}
					if (_player.currentFrame == 3) {
						if (_player.ShouldDealDamage) {
							_enemy.CanAttack = false;
							_enemy.x += _enemy.GetVelocity;
							_enemy.currentFrame = 0;
							_player.doDamage(_enemy);
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
