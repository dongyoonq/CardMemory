using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryUI : MonoBehaviour
{
    [SerializeField] TitleUI titleUI;

    [SerializeField] TMP_Text totalTxt;
    [SerializeField] TMP_Text resultTxt;
    [SerializeField] Button backBtn;
    [SerializeField] RectTransform contents;

    [SerializeField] Button stageButtonPrefab;

    void Awake()
    {
        backBtn.onClick.AddListener(() => BackTitle());
    }

    void Start()
    {
        for (int i = 0; i < GameManager.Data.History.Length; i++)
        {
            int stageIndex = i; // 새로운 변수를 만들어 현재의 i 값을 저장

            Button btnInst = Instantiate(stageButtonPrefab, contents);
            btnInst.onClick.AddListener(() => ShowResult(stageIndex));

            TMP_Text btnName = btnInst.GetComponentInChildren<TMP_Text>();
            btnName.text = $"Stage {i + 1}";
        }
    }

    void BackTitle()
    {
        gameObject.SetActive(false);
        titleUI.gameObject.SetActive(true);
        GameManager.Sound.PlaySFX("BtnClick");
    }

    public void ShowTotal()
    {
        totalTxt.text =
        $"Stage : {GameManager.Data.HighStage + 1}\nHigh Score : {GameManager.Data.HighScore}";
    }

    public void ShowResult(int stage)
    {
        resultTxt.text = 
        $"Stage : {stage + 1}\nBest Score : {GameManager.Data.History[stage].BestScore}\nClear Time : {GameManager.Data.History[stage].BestTime}";
        GameManager.Sound.PlaySFX("BtnClick");
    }
}