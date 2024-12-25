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

        public GameObject GetCorrectCard(CardData cardData) 
            => GetCard($"{cardData.Identifier}-CorrectCard", cardData.Sprite, _gameChecker.PassLevel);

        public GameObject GetIncorrectCard(CardData cardData) 
            => GetCard($"{cardData.Identifier}-IncorrectCard", cardData.Sprite, _gameChecker.FailLevel);

        private GameObject GetCard(string name, Sprite sprite, UnityAction action)
        {
            var card = new GameObject(name);
            var button = card.AddComponent<Button>();
            button.onClick.AddListener(action);
            button.onClick.AddListener(() => { card.transform.DOScale(1.5f, 0.1f); }); // привести в порядок
            card.AddComponent<Image>().sprite = sprite;
            return card;
        }
    }
}