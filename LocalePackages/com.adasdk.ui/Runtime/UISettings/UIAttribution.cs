public class UIAttribution : UIWithCloseButton
{
    protected override void OnClose()
    {
        SoundController.Instance.StopAll();
        base.OnClose();
    }
}
