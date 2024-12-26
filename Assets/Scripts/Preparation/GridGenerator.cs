using System;
using System.Collections.Generic;
using Cards;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Preparation
{
    public class GridGenerator
    {
        private readonly CardCreator _cardCreator;
        private readonly Canvas _canvas;

        public GridGenerator(CardCreator cardCreator, Canvas canvas)
        {
            _cardCreator = cardCreator;
            _canvas = canvas;
        }

        public void Generate(LevelParameters level)
        {
            if (level.columns * level.rows > level.cardBundleData.Cards.Length)
                throw new Exception("Card bundle data less than the length of the array");

            GenerateGrid(level.cardBundleData, level.rows, level.columns, _canvas.transform);
        }

        private void GenerateGrid(CardBundleData cardBundleData, int rows, int columns, Transform parent)
        {
            var cards = GetCardsArray(cardBundleData, rows * columns);
            const float spacing = 50f;

            int cardIndex = 0;

            for (int row = 0; row < rows; row++)
            {
                float rowWidth = 0f;
                for (int col = 0; col < columns; col++)
                {
                    var cardSprite = cards[cardIndex++].CardData.Sprite;
                    rowWidth += cardSprite.bounds.size.x * 100 + spacing;
                }

                rowWidth -= spacing;          
                float startX = -rowWidth / 2f;
                
                cardIndex -= columns;
                for (int col = 0; col < columns; col++)
                {
                    var card = cards[cardIndex++];
                    var rect = card.GetComponent<RectTransform>();
                    var cardSize = card.CardData.Sprite.bounds.size * 100;

                    rect.SetParent(parent);
                    rect.sizeDelta = cardSize;
                    rect.anchoredPosition = new Vector2(startX + cardSize.x / 2, -row * (cardSize.y + spacing));
                    rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
                    rect.localScale = Vector3.one;

                    startX += cardSize.x + spacing;
                }
            }
        }

        private Card[] GetCardsArray(CardBundleData cardBundleData, int length)
        {
            if (length > cardBundleData.Cards.Length)
                throw new Exception("Card bundle data less than the length of the array");

            var arr = new Card[length];
            var availableIndexes = new List<int>();
            
            for(int i = 0; i < cardBundleData.Cards.Length; i++)
                availableIndexes.Add(i);

            var correctCardIndex = UnityEngine.Random.Range(0, length);

            for(int i = 0; i < length; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableIndexes.Count);
                int cardIndex = availableIndexes[randomIndex];
                
                arr[i] = i == correctCardIndex
                    ? _cardCreator.GetCorrectCard(cardBundleData.Cards[cardIndex])
                    : _cardCreator.GetIncorrectCard(cardBundleData.Cards[cardIndex]);
                
                availableIndexes.RemoveAt(randomIndex);
            }

            return arr;
        }
    }
}