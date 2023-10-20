using UnityEditor;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    [MenuItem("Tools/Delete PlayerPrefs")]
    public static void DeletePlayerPrefs()
    {
        SaveSystem.ResetSaveSystem();
        Debug.Log("PlayerPrefs have been deleted.");
    }
}
