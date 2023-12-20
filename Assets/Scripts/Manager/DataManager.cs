using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int CurrScore { get; set; } = 0;
    public int HighScore { get; set; } = 0;
    public int HighStage { get; set; } = 0;

    public StageHistory[] History { get; set; }

    public StockCard cardStock;

    void Start()
    {
        cardStock = FindAnyObjectByType<StockCard>();
    }
}