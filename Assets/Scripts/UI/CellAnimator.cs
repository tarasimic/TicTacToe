using UnityEngine;
using System.Collections;

public class CellAnimator : MonoBehaviour
{
    public void PlayPlacementAnimation(Transform cell)
    {
        StartCoroutine(AnimateCell(cell));
    }

    private IEnumerator AnimateCell(Transform cell)
    {
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration * 0.6f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (duration * 0.6f);
            float scale = Mathf.Lerp(0f, 1.2f, t);
            cell.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration * 0.4f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (duration * 0.4f);
            float scale = Mathf.Lerp(1.2f, 1f, t);
            cell.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        cell.localScale = Vector3.one;
    }
}