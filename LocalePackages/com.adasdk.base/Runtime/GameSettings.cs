using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettings : ScriptableObject
{
    private static GameSettings Instance
    {
        get
        {
            if (instance == null)
                instance = DBManager.Get<GameSettings>();
            return instance;
        }
    }

    private static GameSettings instance;

    void Awake()
    {
        if (instance == null)
            instance = DBManager.Get<GameSettings>();
    }
#if UNITY_EDITOR
    void OnEnable()
    {
        Awake();
    }
#endif

    #region main
    public static string ApplicationIdentifier { get => Application.identifier; }
    #endregion

    #region contact
    [SerializeField]
    private string email = "info@adagames.fun";
    public static string Email { get => Instance.email; }

    #endregion contact

    #region ADS
    [SerializeField]
    private string unityADSgameId = "3611347";
    [SerializeField]
    private bool testMode = true;

    public static string UnityADSgameId { get => Instance.unityADSgameId; }
    public static bool TestMode { get => Instance.testMode; }


    #endregion

    #region GooglePlay
    [SerializeField]
    private string leaderboardId = "CgkItOyo38AREAIQAQ";

    public static string LeaderboardId { get => Instance.leaderboardId; }

    [SerializeField]
    private string googlePlayLink = "https://play.google.com/store/apps/dev?id=7361026415103032733";
    public static string GooglePlayLink { get => Instance.googlePlayLink; }
    #endregion GooglePlay
}
