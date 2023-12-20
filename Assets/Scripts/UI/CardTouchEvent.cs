using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTouchEvent : MonoBehaviour
{
    bool isTouched;

    void Update()
    {
        if (!isTouched && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Card card = hit.collider.GetComponent<Card>();

                if (card != null)
                {
                    StartCoroutine(card.FlipCard(true));
                }
            }

            isTouched = !isTouched;

            StartCoroutine(MouseCoolTimer());
        }
    }

    IEnumerator MouseCoolTimer()
    {
        yield return new WaitForSeconds(0.4f);

        isTouched = !isTouched;
    }
}
