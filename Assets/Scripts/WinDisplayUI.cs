using UnityEngine;
using TMPro;

public class WinDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;

    private void Start()
    {
        if (MatchManager.Instance == null)
        {
            Debug.LogError("MatchManager not found!");
            return;
        }

        int winner = MatchManager.Instance.GetMatchWinner();

        if (winner == 1)
            winText.text = "PLAYER 1 WINS!";
        else if (winner == 2)
            winText.text = "PLAYER 2 WINS!";
        else
            winText.text = "NO WINNER";
    }

    // Called by UI Button
    public void ReturnToCharacterSelect()
    {
        MatchManager.Instance.ResetMatch();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            MatchManager.Instance.characterSelectScene
        );
    }
}
