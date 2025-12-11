using UnityEngine;

public class LevelTarget : DefaultObserverEventHandler
{
    [Header("Which Level? (1, 2, or 3)")]
    public int levelNumber = 1;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        CharacterSelectionManager manager = FindObjectOfType<CharacterSelectionManager>();
        if (manager != null)
        {
            PlayerPrefs.SetInt("Selected_Level", levelNumber);

            // convert Level 1 ? index 0, etc.
            int index = levelNumber - 1;
            manager.UpdateSpriteVisibility(manager.levelSelectionSprites, index);

            Debug.Log("Level selected via Vuforia: Level " + levelNumber);
        }
    }
}
