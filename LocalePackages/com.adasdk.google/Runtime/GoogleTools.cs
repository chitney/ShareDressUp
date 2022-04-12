using SimpleJSON;
using System.Collections;
using UnityEngine;
using Google.Play.Review;
using Firebase;
using Firebase.Analytics;
using System.Collections.Generic;

public class GoogleTools : Controller
{
    public static GoogleTools Instance => Controllers.Get<GoogleTools>();

    #region Rate
    private Coroutine rating;
    private int rateCount = 0;
   
    private bool showRateUI = true;

    public bool ShowRateUI { get => showRateUI; set => showRateUI = value; }
    public int RateCount  => rateCount;

    public void NeverShowRate()
    {
        rateCount = 1234;
        showRateUI = false;
    }

    public void StartRating(int maxRateCountEx, bool openUrl = true)
    {
        if (rating==null)
        {
            if (rateCount < maxRateCountEx)
            {
                rating = StartCoroutine(requireRate(openUrl));
            }
            else if (openUrl)
                Application.OpenURL(GameSettings.GooglePlayLink);

            rateCount++;
        }       
    }

    private IEnumerator requireRate(bool openUrl)
    {
        // Create instance of ReviewManager
        ReviewManager _reviewManager;
        // ...
        _reviewManager = new ReviewManager();
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            if (openUrl) 
                Application.OpenURL(GameSettings.GooglePlayLink);
            yield break;
        }

        PlayReviewInfo _playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            if (openUrl) 
                Application.OpenURL(GameSettings.GooglePlayLink);
            yield break;
        }

        rating = null;

        if (!requestFlowOperation.IsSuccessful)
            if (openUrl) 
                Application.OpenURL(GameSettings.GooglePlayLink);

        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
    #endregion

    #region Crash
    // Use this for initialization
    void StartCrashlytics()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Crashlytics will use the DefaultInstance, as well;
                // this ensures that Crashlytics is initialized.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here for indicating that your project is ready to use Firebase.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    #endregion Crash

    #region Analystics
    public static void LogEvent(string _event)
    {
        _event = _event.Replace(" ", "");
        if (_event.Length > 40)
            _event = _event.Remove(39);
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent(_event);
    }

    public static void LogLevelStart(string levelName)
    {
        Firebase.Analytics.FirebaseAnalytics
         .LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelStart,
            "level_name", levelName);
    }

    public static void LogLevelEnd(string levelName, string time)
    {
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelEnd, new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter("level_name", levelName),
                new Firebase.Analytics.Parameter("time", time)}); ;
    }

    public static void LogLevelEnd(string levelName)
    {
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelEnd, "level_name", levelName);
    }

    public static void LogLevelUp(int level)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
        Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, level)});
    }
    #endregion

    public override string Name()
    {
        return "gac";
    }

    public override void Deserialize(JSONObject obj)
    {
        StartCrashlytics();
        if (obj == null) return;
        rateCount = obj["rc"];
        showRateUI = obj["sr"];            
    }

    public override JSONObject Serialize()
    {
        var obj = new JSONObject();
        obj["rc"] = rateCount;
        obj["sr"] = showRateUI;
        return obj;
    }
}
