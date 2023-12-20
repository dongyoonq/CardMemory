using System.Collections.Generic;
using UnityEngine;

public class StockCard : MonoBehaviour
{
    [SerializeField] Card[] spadeCardPrefabs;
    [SerializeField] Card[] diamondCardPrefabs;
    [SerializeField] Card[] heartCardPrefabs;
    [SerializeField] Card[] clubCardPrefabs;

    public Dictionary<string, Card[]> cardPrefabsMap;

    void Awake()
    {
        cardPrefabsMap = new Dictionary<string, Card[]>
        {
            { "¢¼", spadeCardPrefabs },
            { "¡ß", diamondCardPrefabs },
            { "¢¾", heartCardPrefabs },
            { "¢À", clubCardPrefabs }
        };
    }
}