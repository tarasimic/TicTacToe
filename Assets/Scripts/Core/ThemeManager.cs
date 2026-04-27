using UnityEngine;


public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    [Header("Theme Sprites")]
    public Sprite[] xSprites;
    public Sprite[] oSprites;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Sprite GetXSprite()
    {
        int index = GameManager.Instance.gameData.selectedThemeIndex;
        return xSprites[index];
    }

    public Sprite GetOSprite()
    {
        int index = GameManager.Instance.gameData.selectedThemeIndex;
        return oSprites[index];
    }
}