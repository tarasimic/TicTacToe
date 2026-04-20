using UnityEngine;
using UnityEngine.UI;

public class BoardSetup : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform boardParent;

    [ContextMenu("Generate Board")]
    public void GenerateBoard()
    {
        // Clear existing cells
        for (int i = boardParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(boardParent.GetChild(i).gameObject);
        }

        // Create 9 cells
        for (int i = 0; i < 9; i++)
        {
            GameObject cell = Instantiate(cellPrefab, boardParent);
            cell.name = $"Cell_{i}";

            // Remove button text
            Transform tmp = cell.transform.Find("Text (TMP)");
            if (tmp != null)
                DestroyImmediate(tmp.gameObject);

            // Add mark image
            GameObject markObj = new GameObject("MarkImage");
            markObj.transform.SetParent(cell.transform, false);
            Image markImage = markObj.AddComponent<Image>();
            markImage.color = new Color(1, 1, 1, 0);

            RectTransform rt = markObj.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
        }

        Debug.Log("Board generated successfully");
    }
}