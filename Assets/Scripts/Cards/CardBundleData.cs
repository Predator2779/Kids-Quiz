using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "New CardBundleData", menuName = "Card Bundle Data", order = 10)]
    public class CardBundleData : ScriptableObject
    {
        [SerializeField] private CardData[] _cards;

        public CardData[] Cards => _cards;
    }
}