using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;


public class HUD : Canvas
{
    Player _player;
    Enemy _enemy;

    public HUD(Player pPlayer, Enemy pEnemy) : base(Game.main.width, 32)
    {
        _player = pPlayer;
        _enemy = pEnemy;
    }

    void Update()
    {
        try
        {
            graphics.Clear(Color.Transparent);
            graphics.DrawString("Player Health: " + _player.GetHealth() + " - Enemy Health: " + _enemy.GetHealth(), SystemFonts.DefaultFont, Brushes.White, 0, 0);
        }
        catch
        {
            // empty
        }
    }
}
