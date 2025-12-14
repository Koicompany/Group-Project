using UnityEngine;
using UnityEngine.UI;

public class RoundScoreUI : MonoBehaviour
{
    [Header("Score Text")]
    public Text p1ScoreText;
    public Text p2ScoreText;

    [Header("Winner Text")]
    public Text roundWinnerText;

    private void Start()
    {
        if (MatchManager.Instance == null)
        {
            Debug.LogError("MatchManager not found!");
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        p1ScoreText.text = $"P1 Wins: {MatchManager.Instance.p1Wins} / 2";
        p2ScoreText.text = $"P2 Wins: {MatchManager.Instance.p2Wins} / 2";

        if (MatchManager.Instance.lastRoundWinner == 1)
            roundWinnerText.text = "PLAYER 1 WON THE ROUND!";
        else if (MatchManager.Instance.lastRoundWinner == 2)
            roundWinnerText.text = "PLAYER 2 WON THE ROUND!";
        else
            roundWinnerText.text = "";
    }
}
