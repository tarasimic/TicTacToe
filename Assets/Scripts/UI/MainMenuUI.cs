using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("Popups")]
    [SerializeField] private GameObject themePopup;
    [SerializeField] private GameObject statsPopup;
    [SerializeField] private GameObject settingsPopup;
    [SerializeField] private GameObject exitPopup;

    [Header("Settings Toggles")]
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle sfxToggle;

    [Header("Stats Texts")]
    [SerializeField] private TextMeshProUGUI totalGamesText;
    [SerializeField] private TextMeshProUGUI player1WinsText;
    [SerializeField] private TextMeshProUGUI player2WinsText;
    [SerializeField] private TextMeshProUGUI drawsText;
    [SerializeField] private TextMeshProUGUI avgDurationText;

    private void Start()
    {
        bgmToggle.isOn = GameManager.Instance.gameData.bgmEnabled;
        sfxToggle.isOn = GameManager.Instance.gameData.sfxEnabled;

        bgmToggle.onValueChanged.AddListener(OnBGMToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);

        AudioManager.Instance.PlayBGM();
    }

    private void OpenPopup(GameObject popup)
    {
        popup.SetActive(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.popupClip);
    }

    private void ClosePopup(GameObject popup)
    {
        popup.SetActive(false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.popupClip);
    }

    private void UpdateStatsUI()
    {
        GameData data = GameManager.Instance.gameData;

        totalGamesText.text = $"Total Games: {data.totalGames}";
        player1WinsText.text = $"Player 1 Wins: {data.player1Wins}";
        player2WinsText.text = $"Player 2 Wins: {data.player2Wins}";
        drawsText.text = $"Draws: {data.draws}";

        if (data.totalGames > 0)
        {
            float avg = data.totalDuration / data.totalGames;
            int minutes = (int)avg / 60;
            int seconds = (int)avg % 60;
            avgDurationText.text = $"Avg Duration: {minutes:00}:{seconds:00}";
        }
        else
        {
            avgDurationText.text = "Avg Duration: 00:00";
        }
    }

    public void OnPlayButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        OpenPopup(themePopup);
    }

    public void OnStatsButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        UpdateStatsUI();
        OpenPopup(statsPopup);
    }

    public void OnSettingsButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        OpenPopup(settingsPopup);
    }

    public void OnExitButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        OpenPopup(exitPopup);
    }

    public void OnStartButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        SceneManager.LoadScene("GameScene");
    }

    public void OnBGMToggleChanged(bool value)
    {
        AudioManager.Instance.SetBGMEnabled(value);
    }

    public void OnSFXToggleChanged(bool value)
    {
        AudioManager.Instance.SetSFXEnabled(value);
    }

    public void OnConfirmExitClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        Application.Quit();
    }

    public void OnCancelExitClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        ClosePopup(exitPopup);
    }

    public void OnThemeSelected(int index)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickClip);
        GameManager.Instance.gameData.selectedThemeIndex = index;
        GameManager.Instance.SaveGame();
    }

    public void OnCloseThemePopup()
    {
        ClosePopup(themePopup);
    }

    public void OnCloseStatsPopup()
    {
        ClosePopup(statsPopup);
    }

    public void OnCloseSettingsPopup()
    {
        ClosePopup(settingsPopup);
    }
}