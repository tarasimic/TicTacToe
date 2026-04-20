using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BoardManager : MonoBehaviour
{
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

        gameOverPopup.SetActive(false);

        for (int i = 0; i < markImages.Length; i++)
        {
            markImages[i].sprite = null;
            markImages[i].color = new Color(1, 1, 1, 0);
        }

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

        int themeIndex = GameManager.Instance.gameData.selectedThemeIndex;

        if (currentPlayer == 1)
        {
            markImages[index].sprite = xSprites[themeIndex];
            markImages[index].color = Color.white;
            p1Moves++;
            player1MovesText.text = $"P1: {p1Moves}";
        }
        else
        {
            markImages[index].sprite = oSprites[themeIndex];
            markImages[index].color = Color.white;
            p2Moves++;
            player2MovesText.text = $"P2: {p2Moves}";
        }

        AudioManager.Instance.PlaySFX(AudioManager.Instance.placementClip);

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

        GameData data = GameManager.Instance.gameData;
        data.totalGames++;
        data.totalDuration += elapsedTime;

        if (winner == 1)
        {
            data.player1Wins++;
            resultText.text = "Player 1 Wins!";
            AudioManager.Instance.PlaySFX(AudioManager.Instance.winClip);
        }
        else if (winner == 2)
        {
            data.player2Wins++;
            resultText.text = "Player 2 Wins!";
            AudioManager.Instance.PlaySFX(AudioManager.Instance.winClip);
        }
        else
        {
            data.draws++;
            resultText.text = "Draw!";
        }

        GameManager.Instance.SaveGame();

        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        durationText.text = $"Duration: {minutes:00}:{seconds:00}";

        gameOverPopup.SetActive(true);
    }

    public void OnRetryClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        StartGame();
    }

    public void OnExitClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        SceneManager.LoadScene("PlayScene");
    }

    public void OnSettingsClicked()
    {
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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        settingsPopup.SetActive(false);
    }

    public void OnBGMToggleChanged(bool value)
    {
        AudioManager.Instance.SetBGMEnabled(value);
    }

    public void OnSFXToggleChanged(bool value)
    {
        AudioManager.Instance.SetSFXEnabled(value);
    }
}