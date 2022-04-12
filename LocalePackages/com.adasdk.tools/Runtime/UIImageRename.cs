using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIImageRename : MonoBehaviour
{
    [SerializeField]
    private Image[] images;
    private void OnEnable()
    {
        images = GetComponentsInChildren<Image>();
        foreach (Image img in images)
            img.gameObject.name = img.sprite.name;
    }
}
