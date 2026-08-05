using UnityEngine;
using Steamworks;

public class SteamManager : MonoBehaviour
{
    public static bool Initialized { get; private set; }
    
    private void Awake()
    {
        if (Initialized)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (!SteamAPI.Init())
        {
            Debug.LogError("Can't initialize Steam!.");
            return;
        }

        Initialized = true;
        
        Debug.Log("Steam initialized.");
        Debug.Log("Steam User: " + SteamFriends.GetPersonaName());
    }
    
    public void UnlockAchievement(string achievementID)
    {
        if (!Initialized)
        {
            return;
        }

        bool success = SteamUserStats.SetAchievement(achievementID);

        if (success)
        {
            SteamUserStats.StoreStats();
        }
    }

    private void OnDestroy()
    {
        if (Initialized)
        {
            SteamAPI.Shutdown();
            Initialized = false;
        }
    }
    
    // To unlock the ending success : SteamManager.Instance.UnlockAchievement("LAMAPADRIE_SUCCESS_ENDING");
    // To unlock the peephole success : SteamManager.Instance.UnlockAchievement("LAMAPADRIE_SUCCESS_PEEPHOLE");
}