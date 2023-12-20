using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public string Suit { get; set; }
    public int Rank { get; set; }

    public CardData(string suit, int rank)
    {
        Suit = suit;
        Rank = rank;
    }
}
