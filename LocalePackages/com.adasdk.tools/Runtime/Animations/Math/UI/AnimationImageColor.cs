using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class AnimationImageColor : MathAnimation
{
    [SerializeField]
    protected float TimeStart = 1f;
    [SerializeField]
    private Color start;
    [SerializeField]
    private Color end;

    /*[ExecuteInEditMode]
    protected override void OnEnable()
    {
        TimeStart = Random.Range(0f, 2f);
        Speed = Random.Range(5f, 15f);
        start.a = Random.Range(0, 0.5f);
        end.a = Random.Range(0.95f, 1f);
    }*/

    private Image targetImage;

    private Color target;
    private Color from;
    float t = 0;
    protected override IEnumerator Animate()
    {
        if (targetImage == null)
            targetImage = gameObject.GetComponent<Image>();
        targetImage.color = start;
        yield return new WaitForSeconds(TimeStart);

        while (check_anim())
        {
            if (Loop)
            {
                from = targetImage.color;
                if (from == start) target = end;
                else if (from == end) target = start;
                t = 0;
            }

            while (t <= 1f)
            {
                t += Time.deltaTime / Speed;
                targetImage.color = Color.Lerp(from, target, t);
                yield return null;
            }

            yield return null;
        }
    }
}
