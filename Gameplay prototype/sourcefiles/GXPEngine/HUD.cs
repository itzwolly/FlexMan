using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;


public class HUD : Sprite
{
    Canvas _playerInfo;
    Canvas _enemyInfo;
    Font _font;

    Player _player;
    Enemy _enemy;

    public HUD(Player pPlayer) : base("HUD.png") {
        _player = pPlayer;

        _playerInfo = new Canvas((game.width / 2), 128);
        SetChildIndex(_playerInfo, 1);
        _playerInfo.x = 0;
        _playerInfo.y = 0;

        _font = new Font(FontFamily.GenericSansSerif, 14);
    }

    public HUD(Player pPlayer, Enemy pEnemy) : base("HUD.png") // base for new functionality, doesnt work yet!
    {
        _player = pPlayer;
        _enemy = pEnemy;

        _playerInfo = new Canvas((game.width / 2), 128);
        SetChildIndex(_playerInfo, 1);
        _playerInfo.x = 0;
        _playerInfo.y = 0;

        _enemyInfo = new Canvas((game.width / 2), 128);
        SetChildIndex(_enemyInfo, 1);
        _enemyInfo.x = game.width / 2;
        _enemyInfo.y = 0;

        _font = new Font(FontFamily.GenericSansSerif, 14);
    }

    void Update()
    {
        try
        {
            _playerInfo.graphics.Clear(Color.Transparent);
            _playerInfo.graphics.DrawString(_player.Name, _font, Brushes.White, 225, 50);
            _playerInfo.graphics.DrawString(_player.GetHealth().ToString(), _font, Brushes.White, 225, 80);
            _playerInfo.graphics.DrawString(" HP", _font, (_player.GetHealth() < Mathf.Floor(_player.GetMaxHealth() / 2)) ? Brushes.Red : Brushes.Green, 250, 80);
            _playerInfo.graphics.DrawString(_player.GetScore().ToString(), _font, Brushes.White, 375, 50);
            _playerInfo.graphics.DrawString("PTS", _font, Brushes.Orange, 450, 50);
            _playerInfo.graphics.DrawString(_player.Stamina.ToString(), _font, (_player.HasEnoughStamina() ? Brushes.Green : Brushes.Red), 375, 80);
            _playerInfo.graphics.DrawString("STM", _font, (_player.HasEnoughStamina() ? Brushes.Green : Brushes.Red), 450, 80);

            if (_enemyInfo != null) {
                _enemyInfo.graphics.Clear(Color.Transparent);
                _enemyInfo.graphics.DrawString(_enemy.GetHealth().ToString(), _font, Brushes.White, 225, 40);
                _enemyInfo.graphics.DrawString(" HP", _font, (_enemy.GetHealth() < Mathf.Floor(_enemy.GetMaxHealth() / 2)) ? Brushes.Red : Brushes.Green, 250, 80);
            }
        } catch {

            // empty
        }
    }
}
