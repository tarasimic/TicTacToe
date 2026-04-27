using UnityEngine;
using System.Collections;

public class StrikeAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform strikeLine;
    [SerializeField] private float animationDuration = 0.3f;

    public void PlayStrike(Vector2 startPos, Vector2 endPos)
    {
        strikeLine.gameObject.SetActive(true);
        StartCoroutine(AnimateStrike(startPos, endPos));
    }

    private IEnumerator AnimateStrike(Vector2 startPos, Vector2 endPos)
    {
        float elapsed = 0f;
        Vector2 center = (startPos + endPos) / 2f;
        float distance = Vector2.Distance(startPos, endPos);
        float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;

        strikeLine.anchoredPosition = center;
        strikeLine.rotation = Quaternion.Euler(0, 0, angle);
        strikeLine.sizeDelta = new Vector2(0, 20);

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            strikeLine.sizeDelta = new Vector2(Mathf.Lerp(0, distance, t), 20);
            yield return null;
        }

        strikeLine.sizeDelta = new Vector2(distance, 20);
    }
}