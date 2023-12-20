using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private Canvas toastMsgCanvas;

    private void Awake()
    {
        toastMsgCanvas = Instantiate(Resources.Load<Canvas>("UI/ToastMsgCanvas"));
        toastMsgCanvas.gameObject.name = "ToastMsgCanvas";
    }

    public void SetFloating(GameObject target, int score, Color color = default(Color), float moveSpeed = 0.4f, float destroyTime = 4f)
    {
        GameObject floatingText = Instantiate(Resources.Load<GameObject>("UI/Floating Text"));
        Vector3 uiPosition = Camera.main.WorldToScreenPoint(target.transform.position);

        if (color != default(Color))
            floatingText.GetComponent<TMP_Text>().color = color;

        floatingText.transform.localPosition = uiPosition;
        floatingText.transform.SetParent(toastMsgCanvas.transform);
        floatingText.GetComponent<FloatingText>().moveSpeed = moveSpeed;
        floatingText.GetComponent<FloatingText>().destroyTime = destroyTime;
        floatingText.GetComponent<FloatingText>().print(score.ToString());
    }

    public void SetFloating(GameObject target, string text, Color color = default(Color), float moveSpeed = 0.4f, float destroyTime = 4f)
    {
        GameObject floatingText = Instantiate(Resources.Load<GameObject>("UI/Floating Text"));
        Vector3 uiPosition = Camera.main.WorldToScreenPoint(target.transform.position);

        if (color != default(Color))
            floatingText.GetComponent<TMP_Text>().color = color;

        floatingText.transform.localPosition = uiPosition;
        floatingText.transform.SetParent(toastMsgCanvas.transform);
        floatingText.GetComponent<FloatingText>().moveSpeed = moveSpeed;
        floatingText.GetComponent<FloatingText>().destroyTime = destroyTime;
        floatingText.GetComponent<FloatingText>().print(text);
    }
}
