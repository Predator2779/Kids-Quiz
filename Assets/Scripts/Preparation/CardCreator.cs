using Cards;
using DG.Tweening;
using Gameplay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Preparation
{
    public class CardCreator
    {
        private readonly GameChecker _gameChecker;

        public CardCreator(GameChecker gameChecker)
        {
            _gameChecker = gameChecker;
        }

        public Card GetCorrectCard(CardData cardData) 
            => GetCard($"{cardData.Identifier}-CorrectCard", cardData, _gameChecker.PassLevel);

        public Card GetIncorrectCard(CardData cardData) 
            => GetCard($"{cardData.Identifier}-IncorrectCard", cardData, _gameChecker.FailLevel);

        private Card GetCard(string nameCard, CardData cardData, UnityAction action)
        {
            var card = new GameObject(nameCard).AddComponent<Card>();
            card.CardData = cardData;
            var button = card.gameObject.AddComponent<Button>();
            button.onClick.AddListener(action);
            button.onClick.AddListener(() => { card.transform.DOScale(1.5f, 0.1f); }); // привести в порядок
            card.gameObject.AddComponent<Image>().sprite = cardData.Sprite;
            return card;
        }
    }
}