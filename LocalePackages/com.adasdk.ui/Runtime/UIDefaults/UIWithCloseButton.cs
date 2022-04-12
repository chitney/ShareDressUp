using UnityEngine;
using UnityEngine.UI;

public class UIWithCloseButton : UserInterface
{
    [SerializeField] private Button closeButton;

    protected override void BeforeOpen()
    {
        closeButton.onClick.AddListener(() => Close());
        base.BeforeOpen();
    }

    protected override void BeforeClose()
    {
        closeButton.onClick.RemoveAllListeners();
        base.BeforeClose();
    }
}
