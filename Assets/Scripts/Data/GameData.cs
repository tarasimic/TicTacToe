using System;

[Serializable]
public class GameData
{
    public int totalGames;
    public int player1Wins;
    public int player2Wins;
    public int draws;
    public float totalDuration;
    public bool bgmEnabled = true;
    public bool sfxEnabled = true;
    public int selectedThemeIndex = 0;
}