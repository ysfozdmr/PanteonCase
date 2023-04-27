using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fenrir.EventBehaviour
{
    public abstract partial class BaseGameEvents
    {
        public const int LevelLoaded = 0;
        public const int StartGame = 1;
        public const int FinishGame = 2;
        public const int WinGame = 3;
        public const int LoseGame = 4;
        public const int RestartGame =5;
        public const int NextLevel = 6;
    }
}