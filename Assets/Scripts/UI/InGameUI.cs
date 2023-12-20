using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] public Button backBtn;
    [SerializeField] public Button revertBtn;
    [SerializeField] public Button resetBtn;

    [SerializeField] TMP_Text stageTxt;
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] TMP_Text timeTxt;

    public void SetStage(int stage)
    {
        stageTxt.text = $"Stage {stage}";
    }

    public void SetScore(int score)
    {
        scoreTxt.text = $"Point {score}";
    }

    public void SetTime(int time)
    {
        timeTxt.text = $"Time: {time} sec";
    }
}
