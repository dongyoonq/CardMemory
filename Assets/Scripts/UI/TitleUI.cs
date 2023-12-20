using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] HistoryUI historyUI;

    [SerializeField] Button startBtn;
    [SerializeField] Button historyBtn;
    [SerializeField] Button exitBtn;

    void Awake()
    {
        startBtn.onClick.AddListener(() => StartGame());
        historyBtn.onClick.AddListener(() => ShowHistory());
        exitBtn.onClick.AddListener(() => ExitGame());
    }

    void OnEnable()
    {
        GameManager.Sound.PlayMusic("Title");
    }

    void StartGame()
    {
        gameObject.SetActive(false);
        GameManager.Stage.StageStart();
        GameManager.Sound.PlaySFX("BtnClick");
    }

    void ShowHistory()
    {
        gameObject.SetActive(false);
        historyUI.gameObject.SetActive(true);
        historyUI.ShowTotal();
        historyUI.ShowResult(0);
        GameManager.Sound.PlaySFX("BtnClick");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
                    Application.OpenURL(webplayerQuitURL);
        #else
                    Application.Quit();
        #endif

        GameManager.Sound.PlaySFX("BtnClick");
    }
}
