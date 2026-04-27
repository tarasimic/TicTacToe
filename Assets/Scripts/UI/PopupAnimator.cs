using UnityEngine;
using System.Collections;

public class PopupAnimator : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.2f;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(AnimateOpen());
    }

    private IEnumerator AnimateOpen()
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            float scale = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}