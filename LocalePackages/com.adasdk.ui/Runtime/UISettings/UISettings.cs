using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIWithCloseButton
{
    [SerializeField]
    private UIButtonToggle SoundsToggle;
    [SerializeField]
    private UIButtonToggle MusicToggle;
    [SerializeField]
    private UIButtonToggle VibroToggle;
    [SerializeField]
    private Button FeedbackButton;
    [SerializeField]
    private Button RateButton;
    [SerializeField]
    private Button AttributionButton;

    protected override void _UI_OnOpen()
    {
        SoundsToggle.button.onClick.AddListener(SetSound);
        MusicToggle.button.onClick.AddListener(SetMusic); 
        VibroToggle.button.onClick.AddListener(SetVibro);
        FeedbackButton.onClick.AddListener(SendFeedback);
        AttributionButton.onClick.AddListener(ShowAttribution);
        RateButton.onClick.AddListener(Rate);

        MusicToggle.Active = SoundController.Instance.MusicActive;
        SoundsToggle.Active = SoundController.Instance.SoundsActive;
        VibroToggle.Active = SoundController.Instance.VibroActive;
        base._UI_OnOpen();
    }

    protected override void OnClose()
    {
        SoundsToggle.button.onClick.RemoveAllListeners();
        MusicToggle.button.onClick.RemoveAllListeners();
        VibroToggle.button.onClick.RemoveAllListeners();
        FeedbackButton.onClick.RemoveAllListeners();
        AttributionButton.onClick.RemoveAllListeners();
        RateButton.onClick.RemoveAllListeners();
        base.OnClose();
    }

    private void Rate()
    {
        //GoogleTools.Instance.StartRating();
    }

    private void SetSound()
    {
        SoundController.Instance.SetActiveSound(!SoundController.Instance.SoundsActive);
        SoundsToggle.Active = SoundController.Instance.SoundsActive;
    }

    private void SetMusic()
    {
        SoundController.Instance.SetActiveMusic(!SoundController.Instance.MusicActive);
        MusicToggle.Active = SoundController.Instance.MusicActive;
    }

    private void SetVibro()
    {
        SoundController.Instance.SetActiveVibro(!SoundController.Instance.VibroActive);
        VibroToggle.Active = SoundController.Instance.VibroActive;
    }

    private void SendFeedback()
    {
        Application.OpenURL("mailto:" + GameSettings.Email + "?subject=" + GameSettings.ApplicationIdentifier);
    }

    private void ShowAttribution()
    {
        UIController.Instance.OpenUI<UIAttribution>();
    }

    public static void ChangeLanguage(Language _language)
    {
        LocalisationsController.Instance.SetLanguage(_language);
    }

}
