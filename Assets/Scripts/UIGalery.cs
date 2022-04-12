using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGalery : UIWithCloseButton
{
    [SerializeField]
    private UIList images;

    protected override void BeforeOpen()
    {
        InitItems();
        base.BeforeOpen();
        //GoogleTools.LogEvent("UIGalery");
        ScreenShotsSaver.onPngRemoved += InitItems;
    }

    protected override void OnClose()
    {
        ScreenShotsSaver.onPngRemoved -= InitItems;
        base.OnClose();
    }

    private void InitItems()
    {
        images.Clear();
        var textures = ScreenShotsSaver.Instance.Textures;

        for (int i = textures.Count - 1; i >= 0; i--)
        {
            Texture2D tex = textures[i];
            UIItemRawImage img = images.GetNext() as UIItemRawImage;
            img.Init(tex);
        }
    }

    public void OpenPreview(RawImage img)
    {
        UIPreview.Instance.Open(img.texture);
    }

}
