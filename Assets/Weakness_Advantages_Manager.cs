using UnityEngine;

public class Weakness_Advantages_Manager : MonoBehaviour
{
    [Header("Own Health")]
    public Health myHealth; // drag your own Health component here
    public CharacterID myID; // optional, can auto-assign

    [Header("Modifiers")]
    public float advantageModifier = 0f;
    public float weaknessModifier = 0f;

    [Header("Character Advantages/Weaknesses")]
    [Tooltip("List of character names this character is strong against")]
    public string[] strongAgainstNames;

    [Tooltip("List of character names this character is weak against")]
    public string[] weakAgainstNames;

    private void Awake()
    {
        if (myHealth == null)
            myHealth = GetComponent<Health>();

        if (myID == null)
            myID = GetComponent<CharacterID>();
    }

    private void Start()
    {
        ApplyWeaknessAndAdvantage();
    }

    private void ApplyWeaknessAndAdvantage()
    {
        Health[] allCharacters = FindObjectsOfType<Health>();

        foreach (Health other in allCharacters)
        {
            if (other == myHealth) continue; // skip self

            CharacterID otherID = other.GetComponent<CharacterID>();
            if (otherID == null) continue;

            // Check for advantage
            foreach (string name in strongAgainstNames)
            {
                if (otherID.characterName == name)
                {
                    myHealth.AdjustHealth(advantageModifier);
                    Debug.Log($"{myID.characterName} gains {advantageModifier} health against {otherID.characterName}");
                    break;
                }
            }

            // Check for weakness
            foreach (string name in weakAgainstNames)
            {
                if (otherID.characterName == name)
                {
                    myHealth.AdjustHealth(weaknessModifier);
                    Debug.Log($"{myID.characterName} loses {weaknessModifier} health against {otherID.characterName}");
                    break;
                }
            }
        }
    }
}
