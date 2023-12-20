using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [SerializeField] Transform startPosition;
    [SerializeField] float spacingX;
    [SerializeField] float spacingY;

    public int rows;
    public int columns;
    public Card[,] instanceCards;

    private float cardXSize;
    private float cardYSize;

    private CardData[,] deck;
    private System.Random random;

    void OnEnable()
    {
        Renderer tmpCard = GameManager.Data.cardStock.cardPrefabsMap["♠"][0].GetComponent<Renderer>();
        cardXSize = tmpCard.bounds.size.x;
        cardYSize = tmpCard.bounds.size.y;

        deck = new CardData[rows, columns];
        instanceCards = new Card[rows, columns];
        random = new System.Random();
    }

    public void CreateDeck()
    {
        InitializeDeck();
        AssignCardNumbersAndSuits();
        InstantiateCard();
    }

    private void InstantiateCard()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Instantiate
                Card card = Instantiate(GameManager.Data.cardStock.cardPrefabsMap[deck[i, j].Suit][deck[i, j].Rank - 1], 
                    startPosition.position + new Vector3(j * (cardXSize + spacingX), 0f, -i * (cardYSize + spacingY)), 
                    Quaternion.Euler(0, 0, 180f));

                card.cardData = deck[i, j];
                instanceCards[i, j] = card;
            }
        }
    }

    private void InitializeDeck()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                deck[i, j] = new CardData("", 0); // 초기화되지 않은 값으로 설정
            }
        }
    }

    private void AssignCardNumbersAndSuits()
    {
        List<CardData> cards = GetAllCards();
        List<int> cardIndices = Enumerable.Range(0, rows * columns).ToList();

        foreach (var card in cards)
        {
            AssignMatchingCardIndices(cardIndices, card);
        }
    }

    private List<CardData> GetAllCards()
    {
        List<CardData> cards = new List<CardData>();
        string[] suits = { "♠", "◆", "♥", "♣" };

        foreach (var suit in suits)
        {
            for (int number = 1; number <= 13; number++)
            {
                cards.Add(new CardData(suit, number));
            }
        }

        return cards.OrderBy(x => random.Next()).ToList();
    }

    private void AssignMatchingCardIndices(List<int> cardIndices, CardData card)
    {
        for (int i = 0; i < 2 && cardIndices.Count > 0; i++)
        {
            int randomIndex = random.Next(cardIndices.Count);
            int index = cardIndices[randomIndex];
            cardIndices.RemoveAt(randomIndex);

            int row = index / columns;
            int col = index % columns;

            deck[row, col].Rank = card.Rank;
            deck[row, col].Suit = card.Suit;
        }
    }

    public Card GetFlippedOtherCard(Card exceptCard)
    {
        foreach (Card card in instanceCards)
        {
            if (!card.isCompleted && card != exceptCard && card.isFlipped)
            {
                return card;
            }
        }

        return null;
    }
}