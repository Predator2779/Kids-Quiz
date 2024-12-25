using System;
using System.Collections.Generic;
using Cards;
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

            GenerateGrid(level.columns, level.rows, level.cardBundleData, _canvas.transform);
        }

        private void GenerateGrid(int columns, int rows, CardBundleData cardBundleData, Transform parent)
        {
            var arr = GetCardsArray(cardBundleData, columns * rows);
            var index = 0;

            // Отступ между ячейками
            const float spacing = 10f;

            // Находим максимальные размеры ячеек
            float maxWidth = 0;
            float maxHeight = 0;

            foreach (var card in arr)
            {
                var sprite = card.GetComponent<Image>().sprite;
                maxWidth = Mathf.Max(maxWidth, sprite.bounds.size.x * 100);   // magic num for scaling
                maxHeight = Mathf.Max(maxHeight, sprite.bounds.size.y * 100); // magic num for scaling
            }

            // Размеры ячейки с учётом отступов
            float cellWidth = maxWidth + spacing;
            float cellHeight = maxHeight + spacing;

            // Общие размеры сетки
            float gridWidth = columns * cellWidth - spacing;
            float gridHeight = rows * cellHeight - spacing;

            // Начальная позиция для выравнивания по центру
            Vector2 startPosition = new Vector2(-gridWidth / 2 + cellWidth / 2, gridHeight / 2 - cellHeight / 2);

            // Размещение объектов
            for(int row = 0; row < rows; row++)
            {
                for(int column = 0; column < columns; column++)
                {
                    var card = arr[index];

                    RectTransform rectTransform = card.GetComponent<RectTransform>();
                    rectTransform.SetParent(parent);

                    // Устанавливаем позицию
                    float xPosition = startPosition.x + column * cellWidth;
                    float yPosition = startPosition.y - row * cellHeight;
                    rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);

                    // Устанавливаем размеры
                    var sprite = card.GetComponent<Image>().sprite;
                    rectTransform.sizeDelta = sprite.bounds.size * 100; // magic num for scaling

                    // Убираем растягивание
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    rectTransform.localScale = Vector3.one;

                    index++;
                }
            }
        }

        private GameObject[] GetCardsArray(CardBundleData cardBundleData, int length)
        {
            if (length > cardBundleData.Cards.Length)
                throw new Exception("Card bundle data less than the length of the array");

            var arr = new GameObject[length];
            var availableIndexes = new List<int>();

            // Инициализация списка доступных индексов
            for (int i = 0; i < cardBundleData.Cards.Length; i++)
            {
                availableIndexes.Add(i);
            }

            // Выбор индекса для правильной карты
            var correctCardIndex = UnityEngine.Random.Range(0, length);

            for (int i = 0; i < length; i++)
            {
                // Случайный индекс из доступных
                int randomIndex = UnityEngine.Random.Range(0, availableIndexes.Count);
                int cardIndex = availableIndexes[randomIndex];

                // Добавляем карту в массив
                arr[i] = i == correctCardIndex
                    ? _cardCreator.GetCorrectCard(cardBundleData.Cards[cardIndex])
                    : _cardCreator.GetIncorrectCard(cardBundleData.Cards[cardIndex]);

                // Удаляем использованный индекс
                availableIndexes.RemoveAt(randomIndex);
            }

            return arr;
        }

    }
}