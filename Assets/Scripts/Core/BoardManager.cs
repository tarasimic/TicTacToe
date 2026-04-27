using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // Required for Coroutines (IEnumerator)

public class BoardManager : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private CellAnimator cellAnimator;
    [Header("Strike")]
    [SerializeField] private StrikeAnimation strikeAnimation;

    [Header("Board")]
    [SerializeField] private Button[] cells;
    [SerializeField] private Image[] markImages;

    [Header("Themes")]
    [SerializeField] private Sprite[] xSprites;
    [SerializeField] private Sprite[] oSprites;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI player1MovesText;
    [SerializeField] private TextMeshProUGUI player2MovesText;

    [Header("Game Over Popup")]
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI durationText;

    [Header("Settings Popup")]
    [SerializeField] private GameObject settingsPopup;
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle sfxToggle;

    private int[] boardState = new int[9];
    private int currentPlayer = 1;
    private bool gameActive = false;
    private float elapsedTime = 0f;
    private int p1Moves = 0;
    private int p2Moves = 0;

    private readonly int[][] winCombos = new int[][]
    {
        new int[] {0, 1, 2},
        new int[] {3, 4, 5},
        new int[] {6, 7, 8},
        new int[] {0, 3, 6},
        new int[] {1, 4, 7},
        new int[] {2, 5, 8},
        new int[] {0, 4, 8},
        new int[] {2, 4, 6}
    };

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (!gameActive) return;

        elapsedTime += Time.deltaTime;
        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void StartGame()
    {
        boardState = new int[9];
        currentPlayer = 1;
        elapsedTime = 0f;
        p1Moves = 0;
        p2Moves = 0;
        gameActive = true;

        player1MovesText.text = "P1: 0";
        player2MovesText.text = "P2: 0";
        timerText.text = "00:00";

        // Reset UI
        gameOverPopup.SetActive(false);
        
        // Hide all marks
        for (int i = 0; i < markImages.Length; i++)
        {
            markImages[i].sprite = null;
            markImages[i].color = new Color(1, 1, 1, 0);
        }

        // Setup Buttons
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].onClick.RemoveAllListeners();
            int index = i;
            cells[i].onClick.AddListener(() => OnCellClicked(index));
        }
    }

    private void OnCellClicked(int index)
    {
        if (!gameActive || boardState[index] != 0) return;

        boardState[index] = currentPlayer;

        // Use ThemeManager if available, otherwise fallback to local sprites
        if (currentPlayer == 1)
        {
            markImages[index].sprite = ThemeManager.Instance != null ? 
                ThemeManager.Instance.GetXSprite() : xSprites[0];
            markImages[index].color = Color.white;
            p1Moves++;
            player1MovesText.text = $"P1: {p1Moves}";
        }
        else
        {
            markImages[index].sprite = ThemeManager.Instance != null ? 
                ThemeManager.Instance.GetOSprite() : oSprites[0];
            markImages[index].color = Color.white;
            p2Moves++;
            player2MovesText.text = $"P2: {p2Moves}";
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.placementClip);

        if (cellAnimator != null)
            cellAnimator.PlayPlacementAnimation(cells[index].transform);
            
        int[] winCombo = CheckWin();
        if (winCombo != null)
        {
            EndGame(currentPlayer, winCombo);
            return;
        }

        if (CheckDraw())
        {
            EndGame(0, null);
            return;
        }

        currentPlayer = currentPlayer == 1 ? 2 : 1;
    }

    private int[] CheckWin()
    {
        foreach (int[] combo in winCombos)
        {
            if (boardState[combo[0]] != 0 &&
                boardState[combo[0]] == boardState[combo[1]] &&
                boardState[combo[1]] == boardState[combo[2]])
            {
                return combo;
            }
        }
        return null;
    }

    private bool CheckDraw()
    {
        foreach (int cell in boardState)
        {
            if (cell == 0) return false;
        }
        return true;
    }

    private void EndGame(int winner, int[] winCombo)
    {
        gameActive = false;

        // Update Stats in GameManager
        if (GameManager.Instance != null)
        {
            GameData data = GameManager.Instance.gameData;
            data.totalGames++;
            data.totalDuration += elapsedTime;

            if (winner == 1) data.player1Wins++;
            else if (winner == 2) data.player2Wins++;
            else data.draws++;

            GameManager.Instance.SaveGame();
        }

        // Handle UI Text
        if (winner == 0)
        {
            resultText.text = "Draw!";
        }
        else
        {
            resultText.text = $"Player {winner} Wins!";
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.winClip);
        }

        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        durationText.text = $"Duration: {minutes:00}:{seconds:00}";

        // Trigger Strike Animation or Show Popup immediately
        if (winCombo != null && strikeAnimation != null)
        {
            Vector2 startPos = cells[winCombo[0]].GetComponent<RectTransform>().anchoredPosition;
            Vector2 endPos = cells[winCombo[2]].GetComponent<RectTransform>().anchoredPosition;
            strikeAnimation.PlayStrike(startPos, endPos);
            StartCoroutine(ShowGameOverDelayed());
        }
        else
        {
            gameOverPopup.SetActive(true);
        }
    }

    public void OnRetryClicked()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        StartGame();
    }

    public void OnExitClicked()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        SceneManager.LoadScene("PlayScene");
    }

    public void OnSettingsClicked()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        
        if (GameManager.Instance != null)
        {
            bgmToggle.isOn = GameManager.Instance.gameData.bgmEnabled;
            sfxToggle.isOn = GameManager.Instance.gameData.sfxEnabled;
        }
        settingsPopup.SetActive(true);
    }

    public void OnCloseSettingsClicked()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        settingsPopup.SetActive(false);
    }

    public void OnBGMToggleChanged(bool value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetBGMEnabled(value);
    }

    public void OnSFXToggleChanged(bool value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXEnabled(value);
    }

    private IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        gameOverPopup.SetActive(true);
    }
}