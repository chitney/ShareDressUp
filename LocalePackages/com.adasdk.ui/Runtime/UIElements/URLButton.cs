using UnityEngine;
using UnityEngine.UI;

public class URLButton : MonoBehaviour
{
    [SerializeField]
    private string url;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenURL);
    }

    private void OpenURL()
    {
        Application.OpenURL(url);
    }

}
