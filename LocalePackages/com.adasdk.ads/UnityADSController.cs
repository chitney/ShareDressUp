using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public enum UnityADSPlacementIds
{
    main, video
}

public class UnityADSController : Controller, IUnityAdsListener, ISerializable
{
    public static Action OnDidFinish;

    [SerializeField]
    private float Delta = 43200f; //interval between shows 
    [SerializeField]
    private int MinLevel = 2; 
    private DateTime LastShow; 

    public static UnityADSController Instance => Controllers.Get<UnityADSController>();
    IUnityAdsListener listener;

    public bool ShowBanner = true;

    public void Start()
    {
        Advertisement.Initialize(GameSettings.UnityADSgameId, GameSettings.TestMode);
        listener = this;
        Advertisement.AddListener(listener);
        if (ShowBanner) 
            StartCoroutine(ShowBannerWhenReady());
    }

    public string Name()
    {
        return "adsc";
    }


    public JSONObject Serialize()
    {
        JSONObject obj = new JSONObject();
        obj.Add("ads", LastShow.ToBinary().ToString());
        return obj;
    }

    public void Deserialize(JSONObject obj)
    {
        if (obj == null) return;
        string lastShow = obj["ads"];
        if (lastShow == "")
        {
            LastShow = new DateTime();
        }
        else
        {
            long temp = Convert.ToInt64(lastShow);
            LastShow = DateTime.FromBinary(temp);
        }
    }

    public bool AdsEnable => !CheckTime() && MinLevel <= PlayerInfoController.Instance.Level;

    bool CheckTime()
    {
        return (DateTime.Now - LastShow).TotalSeconds < Delta;
    }

    #region Banner

    public static void BannerShow()
    {
        Instance.ShowBanner = true;
        Instance.StartCoroutine("ShowBannerWhenReady");
    }

    public static void BannerHide()
    {
        Instance.ShowBanner = false;
        Instance.StopCoroutine("ShowBannerWhenReady");
        Advertisement.Banner.Hide();
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(UnityADSPlacementIds.main.ToString()))
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (ShowBanner)
        {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
            Advertisement.Banner.Show(UnityADSPlacementIds.main.ToString());
        }
        else BannerHide();
    }

    #endregion

    #region timed Video
    public static bool CheckAndShow(string placementId)
    {
        if (Instance.MinLevel > PlayerInfoController.Instance.Level)
            return false;

        if (Instance.CheckTime())
            return false;

        return Show(placementId);
    }

    public static bool Show(string placementId)
    {
        if (Advertisement.IsReady(placementId))
        {
            Instance.LastShow = DateTime.Now;
            Advertisement.Show(placementId: placementId);
            return true;
        }
        else {
            //if (Application.internetReachability == NetworkReachability.NotReachable)
              //  GoogleTools.LogEvent("NoInternet");
             return false;
        }
    }


    void IUnityAdsListener.OnUnityAdsReady(string placementId)
    {
        //Debug.Log("ready");
    }

    void IUnityAdsListener.OnUnityAdsDidError(string message)
    {
        //Debug.Log("error");
    }

    void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
    {
        //Debug.Log("start");
    }

    void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        OnDidFinish?.Invoke();
        //GoogleTools.LogEvent("FinishAds"+showResult.ToString());
    }
    #endregion


}
