using UnityEngine;
using UnityEngine.UI;

public class URLButtonWithText : MonoBehaviour
{
    [SerializeField]
    private string url;
    [SerializeField]
    private Text text;

    public void Init(string text, string url)
    {
        this.url = url;
        this.text.text = text;
    }

    public void _UI_OpenURL()
    {
        Application.OpenURL(url);
    }

}
