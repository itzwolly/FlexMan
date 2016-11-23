using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;
using System.Collections.Specialized;


public class HUD : Sprite
{
    const int HEALTH_BAR_WIDTH = 365;
    const int HEALTH_BAR_OUTLINE_WIDTH = HEALTH_BAR_WIDTH + 1;
    const int STAMINA_BAR_WIDTH = 365;
    const int STAMINA_BAR_OUTLINE_WIDTH = STAMINA_BAR_WIDTH + 1;


    Canvas _playerInfo;
    Canvas _pointsToAddInfo;

    Font _font;
    Player _player;
    Level _level;
    Brush _healthBrushColor;
    int _healthBarWidth;
    int _staminaBarWidth;


    public HUD(Level pLevel, Player pPlayer) : base("HUD-final.png") {
        _player = pPlayer;
        _level = pLevel;

        _level.GetEnemyManager().GetDeadEnemyList().CollectionChanged += Enemy_CollectionChanged;
        
        _playerInfo = new Canvas(game.width, 128);
        SetChildIndex(_playerInfo, 1);
        _playerInfo.x = 0;
        _playerInfo.y = 0;

        _pointsToAddInfo = new Canvas(game.width, 128);
        SetChildIndex(_pointsToAddInfo, 1);
        _pointsToAddInfo.x = 0;
        _pointsToAddInfo.y = 0;

        _font = new Font(FontFamily.GenericSansSerif, 24);
    }

    private void ClearPointsToAddHUD() {
        _pointsToAddInfo.graphics.Clear(Color.Transparent);
    }

    private void Enemy_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        // The moment an enemy dies, he gets added to a list
        if (e.Action == NotifyCollectionChangedAction.Add) {
            _pointsToAddInfo.graphics.DrawString("+" + Enemy.SCORE_INCREMENT, _font, Brushes.Green, 861, -2);
            new Timer(1000, ClearPointsToAddHUD);
        }
        return;
    }

    private void SetFlexManImage() {
        if (_healthBrushColor == Brushes.Green) {
            _playerInfo.graphics.DrawImage(Image.FromFile("assets\\faces\\face1.png"), 15, 15, 100, 100);
        } else if (_healthBrushColor == Brushes.OrangeRed) {
            _playerInfo.graphics.DrawImage(Image.FromFile("assets\\faces\\face2.png"), 15, 15, 100, 100);
        } else if (_healthBrushColor == Brushes.Red) {
            _playerInfo.graphics.DrawImage(Image.FromFile("assets\\faces\\face3.png"), 15, 15, 100, 100);
        }
    }

    private Brush GetHealthBrushColor() {
        // more than 75% and lower or equal than 100%;
        if (_player.GetHealth() > (Mathf.Ceiling(Convert.ToSingle(_player.GetMaxHealth()) / 100 * 75)) && _player.GetHealth() <= _player.GetMaxHealth()) { 
            _healthBrushColor = Brushes.Green;
            

            // more or equal than 25% and less than 75%
        } else if (_player.GetHealth() > (Mathf.Ceiling(Convert.ToSingle(_player.GetMaxHealth()) / 100 * 25)) && _player.GetHealth() <= (Mathf.Floor(Convert.ToSingle(_player.GetMaxHealth()) / 100 * 75))) { 
            _healthBrushColor = Brushes.OrangeRed;
            _playerInfo.graphics.DrawImage(Image.FromFile("assets\\faces\\face2.png"), 15, 15, 100, 100);

            // more or equal than 0% and less than 25%
        } else if (_player.GetHealth() >= (Convert.ToSingle(_player.GetMaxHealth()) - _player.GetMaxHealth()) && _player.GetHealth() <= (Mathf.Floor(Convert.ToSingle(_player.GetMaxHealth()) / 100 * 25))) {
            _healthBrushColor = Brushes.Red;
            _playerInfo.graphics.DrawImage(Image.FromFile("assets\\faces\\face3.png"), 15, 15, 100, 100);

        }
        return _healthBrushColor;
    }

    private int GetHealthBarWidth() {
        if (_player.GetMaxHealth() - _player.GetHealth() == 0) { // full hp
            _healthBarWidth = HEALTH_BAR_WIDTH; // full size of the bar
        } else if (_player.GetMaxHealth() - _player.GetHealth() == _player.GetMaxHealth()) { // dead
            _healthBarWidth = 0;
        } else {
            // what is the percentage of your current hp, compared to your max hp
            float _singlePoint = Mathf.Round((float)_player.GetHealth() / _player.GetMaxHealth() * 100);
            // get new width
            _healthBarWidth = Mathf.Round((float)HEALTH_BAR_WIDTH / 100 * _singlePoint);
        }
        return _healthBarWidth;
    }

    private int GetStaminaBarWidth() {
        if (_player.GetMaxStamina() - _player.GetStamina() == 0) { // full stamina
            _staminaBarWidth = STAMINA_BAR_WIDTH; // full size of the bar
        } else if (_player.GetMaxStamina() - _player.GetStamina() == _player.GetMaxStamina()) { // out of stamina
            _staminaBarWidth = 0;
        } else {
            // what is the percentage of your current stamina, compared to your max stamina
            float _singlePoint = Mathf.Round((float)_player.GetStamina() / _player.GetMaxStamina() * 100);
            // get new stamina
            _staminaBarWidth = Mathf.Round((float)STAMINA_BAR_WIDTH / 100 * _singlePoint);
        }
        return _staminaBarWidth;
    }

    void Update()
    {
        try
        {
            _playerInfo.graphics.Clear(Color.Transparent); // clear hud
            /* FlexMan Face */
            SetFlexManImage();
            /* Name */
            _playerInfo.graphics.DrawString(_player.Name.ToUpper(), _font, Brushes.White, 120, 25); // name
            /* Score */
            _playerInfo.graphics.DrawString(_player.Score.ToString("D8"), _font, Brushes.DarkOrange, 790, 25); // score
            _playerInfo.graphics.DrawString("PTS", _font, Brushes.White, 946, 25); // score texts
            /* Health */
            _playerInfo.graphics.DrawString("HP", _font, Brushes.White, 120, 75); // hp text in front of hp bar
            _playerInfo.graphics.DrawRectangle(Pens.WhiteSmoke, 180, 81, HEALTH_BAR_OUTLINE_WIDTH, 23); // hp bar outline
            _playerInfo.graphics.FillRectangle(GetHealthBrushColor(), 181, 82, GetHealthBarWidth(), 22); // hp bar itself
            /* Stamina */
            _playerInfo.graphics.DrawString("STM", _font, Brushes.White, 560, 75); // stamina text in front of stamina bar
            _playerInfo.graphics.DrawRectangle(Pens.WhiteSmoke, 645, 81, STAMINA_BAR_OUTLINE_WIDTH, 23); // stamina bar outline
            _playerInfo.graphics.FillRectangle((_player.HasEnoughStamina() ? Brushes.RoyalBlue : Brushes.Red), 646, 82, GetStaminaBarWidth(), 22); // stamina bar itself

            // DEBUG
            //_playerInfo.graphics.DrawString(_player.GetHealth() + " - " + _player.GetMaxHealth(), _font, Brushes.White, 800, 74);
        } catch {
            // empty
        }
    }
}
