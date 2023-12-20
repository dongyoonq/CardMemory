using UnityEngine;

public struct StageHistory
{
    public int BestScore { get; set; }
    public int BestTime { get; set; }
}

public class Stage : MonoBehaviour
{
    public CardDeck cardDeck;

    public int bonus;
    public float orgTimer;
    public float timer;

    void Awake()
    {
        orgTimer = timer;
    }

    void OnDisable()
    {
        foreach (var card in cardDeck.instanceCards)
        {
            Destroy(card.gameObject);
        }

        timer = orgTimer;
    }
}