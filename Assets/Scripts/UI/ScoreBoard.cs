using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] TitleUI titleUI;

    [SerializeField] TMP_Text getScoreTxt;
    [SerializeField] TMP_Text bestScoreTxt;
    [SerializeField] TMP_Text stageTxt;

    [SerializeField] Button titleBtn;

    void Awake()
    {
        titleBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            titleUI.gameObject.SetActive(true);
        });
    }

    public void ShowBoard(bool clear, bool updateScore, int stage)
    {
        if (clear)
            stageTxt.text = $"Stage {stage + 1} (clear)";
        else
            stageTxt.text = $"Stage {stage + 1} (fail)";

        getScoreTxt.text = $"You Got Score : {GameManager.Data.CurrScore}";

        if (updateScore)
            bestScoreTxt.text = $"Your Best Score : {GameManager.Data.HighScore} (New)";
        else
            bestScoreTxt.text = $"Your Best Score : {GameManager.Data.HighScore}";
    }
}