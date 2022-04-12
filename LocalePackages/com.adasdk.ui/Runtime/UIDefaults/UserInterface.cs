using UnityEngine;

public abstract class UserInterface : MonoBehaviour
{
    public bool DoNotClose = false;
    public bool ShowShadow = true;

    [SerializeField]
    private Animation myAnimation;

    public string OpenAnim = "WindowsDefaultOpen";
    public string CloseAnim = "WindowsDefaultClose";

    /// <summary>
    /// вызывается до анимации открытия и активации обьекта
    /// </summary>
    protected virtual void BeforeOpen() { }
    /// <summary>
    /// вызывается после завершешения анимации открытия обьекта
    /// </summary>
    protected virtual void _UI_OnOpen() { }
    /// <summary>
    /// вызывается до анимации закрытия
    /// </summary>
    protected virtual void BeforeClose() { }
    /// <summary>
    /// вызывается после анимации закрытия и деактивации обьекта
    /// </summary>
    protected virtual void OnClose() { }

    /// <summary>
    /// открыть интерфейс
    /// </summary>
    public virtual void Open()
    {
        if (myAnimation != null) 
            myAnimation.Stop();
        UIController.Instance.OnOpened(this);
        BeforeOpen();
        Activate();
        if (myAnimation != null) myAnimation.Play(OpenAnim);
        else _UI_OnOpen();
    }

    /// <summary>
    /// закрыть интерфейс
    /// </summary>
    public virtual void CloseAll()
    {
        if (gameObject.activeSelf && !DoNotClose)
        {
            Close();
        }
    }

    /// <summary>
    /// закрыть интерфейс
    /// </summary>
    public virtual bool Close()
    {
        if (gameObject.activeSelf)
        {
            BeforeClose();
            if (myAnimation != null) myAnimation.Play(CloseAnim);
            else Deactivate();
            return true;
        }
        return false;
    }

    protected void Activate()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// вызывается в анимации
    /// </summary>
    protected void Deactivate()
    {
        gameObject.SetActive(false);
        UIController.Instance.OnWindowClosed(this);
        OnClose();
    }

    /// <summary>
    /// вызывается в анимации
    /// </summary>
    protected void _UI_Deactivate()
    {
        Deactivate();
    }

    public static TUserInterface Show<TUserInterface>() where TUserInterface : UserInterface
    {
        return (TUserInterface)UIController.Instance.OpenUI<TUserInterface>();
    }

    public static void Hide<TUserInterface>() where TUserInterface : UserInterface
    {
        UIController.Instance.CloseUI<TUserInterface>();
    }

}
