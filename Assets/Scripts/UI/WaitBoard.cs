using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitBoard : MonoBehaviour
{
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text notice;

    public void SetTimer(int time)
    {
        timer.text = time.ToString();
    }

    public void SetNotice(string notice)
    {
        this.notice.text = notice;
    }
}
