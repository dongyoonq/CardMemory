using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    TMP_Text floatingText;
    [NonSerialized] public float moveSpeed;
    [NonSerialized] public float destroyTime;

    private void OnEnable()
    {
        floatingText = GetComponent<TMP_Text>();
    }

    public void print(string Text)
    {
        floatingText.text = Text;
    }

    private void Start()
    {
        moveSpeed = 0.4f;
        destroyTime = 4f;
    }

    void Update()
    {

        Vector3 vector = new Vector3(floatingText.transform.position.x, floatingText.transform.position.y + (moveSpeed + Time.deltaTime), floatingText.transform.position.z);
        floatingText.transform.position = vector;
        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
