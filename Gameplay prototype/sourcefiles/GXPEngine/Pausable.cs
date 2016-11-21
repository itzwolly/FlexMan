using System;
using System.Collections.Generic;
using GXPEngine;

public class Pausable : AnimationSprite
{
    static List<Pausable> pauseList = new List<Pausable>();

    public Pausable(string fileName, int col, int row) : base(fileName, col, row) {
        pauseList.Add(this);
    }

    public static void Pause() {
        foreach (Pausable pausable in pauseList) {
            Game.main.game.Remove(pausable);
        }
    }

    public static void UnPause() {
        foreach (Pausable pausable in pauseList) {
            Game.main.game.Add(pausable);
        }
    }
}

