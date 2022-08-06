using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public enum GameState
    {
        Undefined,
        Game,
        Next,
        BackToMenu,
        GameFailed,
        GameClear
    }
}