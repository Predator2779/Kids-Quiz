using System;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public class CardData
    {
        [SerializeField] private string _identifier;
        [SerializeField] private Sprite _sprite;

        public string Identifier => _identifier;
        public Sprite Sprite => _sprite;
    }
}