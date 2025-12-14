using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [Header("Match Rules")]
    public int maxWins = 2; // Best of 3

    [Header("Scene Names")]
    public string characterSelectScene = "CharacterSelectScene";
    public string gameScene = "GameLevelScene";

    [Header("Match State")]
    public int p1Wins = 0;
    public int p2Wins = 0;
    public int lastRoundWinner = 0; // 1 = P1, 2 = P2

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RoundOver(int winningPlayer)
    {
        lastRoundWinner = winningPlayer;

        if (winningPlayer == 1) p1Wins++;
        else if (winningPlayer == 2) p2Wins++;

        Debug.Log($"Round Over ? P1: {p1Wins}, P2: {p2Wins}");

        if (p1Wins >= maxWins || p2Wins >= maxWins)
        {
            Debug.Log("MATCH OVER");
            SceneManager.LoadScene("CharacterSelect");
            ResetMatch();
        }
        else
        {
            SceneManager.LoadScene("CharacterSelect");
        }
    }

    public void ResetMatch()
    {
        p1Wins = 0;
        p2Wins = 0;
        lastRoundWinner = 0;
    }
}
