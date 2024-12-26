using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly int _scaleModificator;

        public GridGenerator(CardCreator cardCreator, Canvas canvas, int scaleModificator)
        {
            _cardCreator = cardCreator;
            _canvas = canvas;
            _scaleModificator = scaleModificator;
        }

        public void Generate(LevelParameters level)
        {
            if (level.columns * level.rows > level.cardBundleData.Cards.Length)
                throw new Exception("Card bundle data less than the length of the array");

            GenerateGrid(level.cardBundleData, level.rows, level.columns, _canvas.transform);
        }

        /// <summary>
        /// Генерирует сетку карточек на основе переданных параметров.
        /// </summary>
        private void GenerateGrid(CardBundleData cardBundleData, int rows, int columns, Transform parent)
        {
            var cards = GetCardsArray(cardBundleData, rows * columns);
            Vector2 spacing = GetSpacing(cards[0]);

            // Подготовка данных для генерации
            float[] rowHeights = CalculateRowHeights(cards, rows, columns);
            float totalHeight = CalculateTotalHeight(rowHeights, spacing.y);
            float startY = totalHeight / 2f;

            // Размещение карточек
            PlaceCardsInGrid(cards, rows, columns, parent, rowHeights, startY, spacing.x);
        }

        private Vector2 GetSpacing(Card card)
        {
            return card.CardData.Sprite.bounds.size * _scaleModificator / 2;
        }

        /// <summary>
        /// Размещает карточки в сетке.
        /// </summary>
        private void PlaceCardsInGrid(Card[] cards, int rows, int columns, Transform parent, float[] rowHeights,
            float startY, float spacing)
        {
            int cardIndex = 0;

            for(int row = 0; row < rows; row++)
            {
                float rowWidth = CalculateRowWidth(cards, cardIndex, columns, spacing);
                float startX = -rowWidth / 2f;
                float rowPositionY = startY - rowHeights.Take(row).Sum() - row * spacing - rowHeights[row] / 2f;

                cardIndex = PlaceRow(cards, columns, parent, startX, rowPositionY, spacing, cardIndex);
            }
        }

        /// <summary>
        /// Рассчитывает ширину строки.
        /// </summary>
        private float CalculateRowWidth(Card[] cards, int startIndex, int columns, float spacing) =>
            Enumerable.Range(0, columns)
                .Where(col => startIndex + col < cards.Length)
                .Select(col => cards[startIndex + col].CardData.Sprite.bounds.size.x * _scaleModificator + spacing)
                .Sum() - spacing;

        /// <summary>
        /// Рассчитывает максимальную высоту каждой строки.
        /// </summary>
        private float[] CalculateRowHeights(Card[] cards, int rows, int columns)
        {
            var rowHeights = new float[rows];
            int cardIndex = 0;

            for(int row = 0; row < rows; row++)
            {
                rowHeights[row] = Enumerable.Range(0, columns)
                    .Where(col => cardIndex < cards.Length)
                    .Select(col => cards[cardIndex++].CardData.Sprite.bounds.size.y * _scaleModificator)
                    .DefaultIfEmpty(0)
                    .Max();
            }

            return rowHeights;
        }

        /// <summary>
        /// Рассчитывает общую высоту сетки.
        /// </summary>
        private float CalculateTotalHeight(float[] rowHeights, float spacing) =>
            rowHeights.Sum() + (rowHeights.Length - 1) * spacing;

        /// <summary>
        /// Размещает карточки одной строки.
        /// </summary>
        private int PlaceRow(Card[] cards, int columns, Transform parent, float startX,
            float rowPositionY, float spacing, int cardIndex)
        {
            for(int col = 0; col < columns; col++)
            {
                if (cardIndex >= cards.Length) break;

                var card = cards[cardIndex++];
                var rect = card.GetComponent<RectTransform>();
                var cardSize = card.CardData.Sprite.bounds.size * _scaleModificator;

                rect.SetParent(parent);
                rect.sizeDelta = cardSize;
                rect.anchoredPosition = new Vector2(startX + cardSize.x / 2, rowPositionY);
                rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
                rect.localScale = Vector3.one;

                startX += cardSize.x + spacing;
            }

            return cardIndex;
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