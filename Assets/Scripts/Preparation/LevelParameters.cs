using System;
using Cards;

namespace Preparation
{
    [Serializable]
    public class LevelParameters
    {
        public int rows, columns;
        public CardBundleData cardBundleData;
    }
}