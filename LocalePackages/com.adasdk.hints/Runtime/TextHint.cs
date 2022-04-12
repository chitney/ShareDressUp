using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextHint : Hint
{
    [SerializeField]
    private Text textField;
    [SerializeField]
    private CanvasGroup alpha;

    public void Init(string text)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            textField.text = text;
            alpha.alpha = 1;
            StartCoroutine(WaitOfStart());
        }
    }

    public override void Stop()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        base.Stop();
    }

    protected IEnumerator WaitOfStart()
    {
        float t = 5f;
        while (t > 0)
        {
            if (EndAction())
                break;
            
            t -= Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(WaitOfEndAction());
    }

    public override bool EndAction()
    {
        return base.EndAction() || alpha.alpha<=0;
    }

    protected override void FrameUptade()
    {
        alpha.alpha -= Time.deltaTime;
        base.FrameUptade();
    }
}
