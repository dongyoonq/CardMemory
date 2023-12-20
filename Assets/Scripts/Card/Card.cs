using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
    public CardData cardData;

    public bool isFlipped;
    public bool isCompleted;

    public IEnumerator FlipCard(bool flip)
    {
        if (isFlipped && flip)
        {
            yield break;
        }

        float flipDuration = 0.4f;
        float startAngle = transform.rotation.eulerAngles.z;
        float targetAngle = startAngle + 180f;
        float elapsedTime = 0f;

        while (elapsedTime < flipDuration)
        {
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / flipDuration);
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

            yield return null;

            elapsedTime += Time.deltaTime;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

        isFlipped = flip;

        if (flip)
        {
            GameManager.Stage.UpdateCard(this);
        }

        GameManager.Sound.PlaySFX("CardFlip");
    }

    public IEnumerator RevertCard()
    {
        float flipDuration = 0.4f;
        float startAngle = transform.rotation.eulerAngles.z;
        float targetAngle = startAngle + 180f;
        float elapsedTime = 0f;

        while (elapsedTime < flipDuration)
        {
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / flipDuration);

            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

            yield return null;

            elapsedTime += Time.deltaTime;
        }

        // 최종 각도 적용 (오차 보정)
        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

        isFlipped = !isFlipped;

        GameManager.Sound.PlaySFX("CardFlip");
    }
}
