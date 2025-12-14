using UnityEngine;
using UnityEngine.UI;

public class RoundDisplay : MonoBehaviour
{
    public Text p1Text;
    public Text p2Text;

    private void Start()
    {
        p1Text.text = "P1 Rounds: " + MatchManager.Instance.p1Wins;
        p2Text.text = "P2 Rounds: " + MatchManager.Instance.p2Wins;
    }
}
