using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPreview : UIWithCloseButton
{
    [SerializeField]
    private RawImage prewiew;

    private static UIPreview instance;

    public static UIPreview Instance
    {
        get
        {
            if (instance == null)
                instance = UIController.Instance.GetUI<UIPreview>();
            
            return instance;
        }
    }

    public void Open(Texture rawImage)
    {
        prewiew.texture = rawImage;
        Open();
    }

    public void UI_Share(RawImage image)
    {
        ScreenShotsSaver.Instance.Share(image.texture.name);
        Close();
    }


    public void UI_Delete(RawImage image)
    {
        ScreenShotsSaver.Instance.Remove(image.texture.name);
        Close();
    }
}
