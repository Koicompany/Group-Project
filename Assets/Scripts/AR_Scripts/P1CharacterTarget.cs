using UnityEngine;

public class P1CharacterTarget : DefaultObserverEventHandler
{
    [Header("Which Character? 0=Char1, 1=Char2, 2=Char3")]
    public int characterIndex = 0;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        CharacterSelectionManager manager = FindObjectOfType<CharacterSelectionManager>();
        if (manager != null)
        {
            PlayerPrefs.SetInt("P1_Char_Index", characterIndex);
            manager.UpdateSpriteVisibility(manager.p1SelectionSprites, characterIndex);
            Debug.Log("P1 selected via Vuforia: " + characterIndex);
        }
    }
}
