using UnityEngine;
using UnityEngine.UI;

public class UIButtonToggle : MonoBehaviour
{
    [SerializeField]
    private UIModeSwitcher mode;
    [SerializeField]
    public Button button;

    private bool active;

    public bool Active
    {
        set
        {
            active = value;
            mode.Mode = active ? 1 : 0;
        }
    }
}
