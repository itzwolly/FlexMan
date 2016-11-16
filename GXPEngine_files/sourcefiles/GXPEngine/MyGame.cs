using System;
using System.Drawing;
using GXPEngine;
using System.Timers;
using System.Collections.Generic;

public class MyGame : Game //MyGame is a Game
{
	Enemy _enemy, _anotherEnemy;
	Player _player, _anotherPlayer;
	HUD _hud;
	int _interval;
	//int _cooldownTimer;
	List<Enemy> _enemyList = new List<Enemy>();
	List<Player> _playerList = new List<Player>();

	//initialize game here
	public MyGame () : base(1024, 768, false, false)
	{
		targetFps = 60;

		_enemy = new Enemy("Assets\\hit_00.png", 7, 1);
		_anotherEnemy = new Enemy("Assets\\hit_00.png", 7, 1);
		_player = new Player("assets\\hit_00.png", 7, 1);
		_anotherPlayer = new Player("assets\\hit_00.png", 7, 1);

		_hud = new HUD(_player, _enemy);
		_hud.x = (game.width / 2) - 100;
		_hud.y = game.height / 2;

		_anotherEnemy.x = 300;
		_anotherEnemy.y = 400;
		_anotherEnemy.Mirror(true, false);

		_anotherPlayer.x = 500;
		_anotherPlayer.y = 400;

		_enemyList.Add(_enemy);
		_enemyList.Add(_anotherEnemy);

		_playerList.Add(_player);
		//_playerList.Add(_anotherPlayer);

		_anotherPlayer.visible = false;

		AddChild(_enemy);
		AddChild(_anotherEnemy);

		AddChild(_player);
		//AddChild(_anotherPlayer);

		AddChild(_hud);
	}
	
	//update game here
	void Update() {
		foreach (Enemy singleEnemy in _enemyList) {
			foreach (Player singlePlayer in _playerList) {

				if (singlePlayer.x < singleEnemy.x) {
					singleEnemy.Direction = 0;
					singleEnemy.Mirror(false, false);
				}
				if (singlePlayer.x > singleEnemy.x) {
					singleEnemy.Direction = 1;
					singleEnemy.Mirror(true, false);
				}

				if (singleEnemy.Direction == 0) { // left
					singleEnemy.getHitBoxRight().visible = false;
					singleEnemy.getHitBoxLeft().visible = true;
					if (singleEnemy.getHitBoxLeft().HitTest(singlePlayer.getHitboxRight())) {
						handleEnemyAttackFacingLeft(singleEnemy);
						handlePlayerAttack(singleEnemy);
					} else {
						handleEnemyAnimation(singleEnemy);
						handlePlayerAnimation(singlePlayer);
					}
				}
				if (singleEnemy.Direction == 1) { // right {
					singleEnemy.getHitBoxRight().visible = true;
					singleEnemy.getHitBoxLeft().visible = false;
					if (singleEnemy.getHitBoxRight().HitTest(singlePlayer.getHitboxRight())) {
						handleEnemyAttackFacingRight(singleEnemy);
						handlePlayerAttack(singleEnemy);
					
					} else {
						handleEnemyAnimation(singleEnemy);
						handlePlayerAnimation(singlePlayer);
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


				if (singlePlayer.currentFrame == 4) {
					singlePlayer.ShouldDealDamage = true;
				}
				if (singlePlayer.currentFrame == 6) {
					singlePlayer.currentFrame = 0;
					singlePlayer.StartAttackAnimation = false;
				}
			}
		}
	}

	private void handleEnemyAttackFacingLeft(Enemy pEnemy) {
		_interval++;
		if (pEnemy.CanAttack) { // standard true
			if (_interval >= 15) {
				_interval = 0;
				pEnemy.StartAttackAnimation = true;
			}
			if (pEnemy.currentFrame == 3) {
				if (pEnemy.ShouldDealDamage) {
					//_player.CanAttack = false;
					_player.x -= _player.GetVelocity;
					_player.takeDamage();
					pEnemy.ShouldDealDamage = false;
				}
			}
		} else {
			//_cooldownTimer++;
			pEnemy.currentFrame = 0;
			pEnemy.StartAttackAnimation = false;
			//if (_player.currentFrame != 3 && _cooldownTimer >= 505) {
			//    _enemy.CanAttack = true;
			//    _cooldownTimer = 0;
			//}
		}
	}

	private void handleEnemyAttackFacingRight(Enemy pEnemy) {
		_interval++;
		if (pEnemy.CanAttack) { // standard true
			if (_interval >= 15) {
				_interval = 0;
				pEnemy.StartAttackAnimation = true;
			}
			if (pEnemy.currentFrame == 3) {
				if (pEnemy.ShouldDealDamage) {
					//_player.CanAttack = false;
					_player.x -= _player.GetVelocity;
					_player.takeDamage();
					pEnemy.ShouldDealDamage = false;
				}
			}
		} else {
			//_cooldownTimer++;
			pEnemy.currentFrame = 0;
			pEnemy.StartAttackAnimation = false;
			//if (_player.currentFrame != 3 && _cooldownTimer >= 505) {
			//    _enemy.CanAttack = true;
			//    _cooldownTimer = 0;
			//}
		}
	}

	private void handlePlayerAttack(Enemy pEnemy) {
		if (_player.CanAttack) {
			if (_player.StartAttackAnimation) {
				if (_player.CanAttack) {
					_player.DoAttackAnimation();
				}
				if (_player.currentFrame == 3) {
					if (_player.ShouldDealDamage) {
						pEnemy.CanAttack = false;
						pEnemy.x += pEnemy.GetVelocity;
						pEnemy.currentFrame = 0;
						_player.doDamage(pEnemy);
						_player.ShouldDealDamage = false;
					}
				}
			} else {
				_player.CanAttack = true;
			}
		} else {
			_player.currentFrame = 0;
		}
	}

	private void handlePlayerAnimation(Player pPlayer) {
		if (pPlayer.StartAttackAnimation) {
			pPlayer.DoAttackAnimation();
			if (pPlayer.currentFrame == 6) {
				pPlayer.currentFrame = 0;
				pPlayer.StartAttackAnimation = false;
			}
		}
	}

	private void handleEnemyAnimation(Enemy pEnemy) {
		if (pEnemy.currentFrame == 0) {
			pEnemy.StartAttackAnimation = false;
		}
	}

	private void handlePlayerControls(Player pPlayer) {
		if (Input.GetKey(Key.RIGHT)) {
			if (!pPlayer.StartAttackAnimation) {
				pPlayer.getHitboxLeft().visible = false;
				pPlayer.getHitboxRight().visible = true;
				pPlayer.Mirror(true, false);
				pPlayer.x += 4f;
			}
		} else if (Input.GetKey(Key.LEFT)) {
			if (!pPlayer.StartAttackAnimation) {
				pPlayer.getHitboxLeft().visible = true;
				pPlayer.getHitboxRight().visible = false;
				pPlayer.Mirror(false, false);
				pPlayer.x -= 4f;
			}
		}

		if (Input.GetKey(Key.UP)) {
			if (!pPlayer.StartAttackAnimation) {
				y -= 4f;
			}
		} else if (Input.GetKey(Key.DOWN)) {
			if (!pPlayer.StartAttackAnimation) {
				y += 4f;
			}
		}

		if (Input.GetKeyDown(Key.SPACE)) {
			pPlayer.StartAttackAnimation = true;
		}
	}

	//system starts here
	static void Main() 
	{
		new MyGame().Start();
	}
}
