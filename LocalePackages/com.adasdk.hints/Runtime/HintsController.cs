using SimpleJSON;
using UnityEngine;

public class HintsController : Controller
{
    [SerializeField]
    private DragHint dragHint;
    [SerializeField]
    private ClickHint clickHint;
    [SerializeField]
    private TextHint textHint;

    #region DragHint
    public static void ShowDragHint(GameObject obj1, GameObject obj2)
    {
        Instance.DragHint(obj1, obj2);
    }

    public static void ShowDragHint(Vector3 from, Vector3 to)
    {
        Instance.DragHint(from, to);
    }

    public static void StopDragHint()
    {
        Instance.dragHint.Stop();
    }


    public void DragHint(GameObject obj1, GameObject obj2)
    {
        dragHint.Init(obj1, obj2);
    }

    public void DragHint(Vector3 from, Vector3 to)
    {
        dragHint.Init(from, to);
    }

    #endregion DragHint

    #region Click

    public static void ShowClickHint(Vector3 target)
    {
        Instance.clickHint.Init(target);
    }

    public static void StopClickHint()
    {
        Instance.clickHint.Stop();
    }


    #endregion

    #region Text

    public static void ShowText(string key)
    {
        Instance.textHint.Init(key.Locale());
    }

    #endregion

    #region base
    public static HintsController Instance => Controllers.Get<HintsController>();

    #endregion
}
