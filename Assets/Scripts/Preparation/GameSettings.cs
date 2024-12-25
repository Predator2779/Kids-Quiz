using System;
using UnityEngine;

namespace Preparation
{
    [Serializable]
    public class GameSettings
    {
        [SerializeField] private LevelParameters[] _levels;

        public LevelParameters[] Levels => _levels;
    }
}