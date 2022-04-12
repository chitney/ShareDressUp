using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIOpenButton : MonoBehaviour
{
    [SerializeField]
    private UserInterface Interface;
    [SerializeField]
    protected Button btn;

    private UserInterface _wnd;

    [SerializeField]
    private Type Type => Interface.GetType();

    protected virtual void OnEnable()
    {
        if (btn==null)
        {
            btn = GetComponent<Button>();
        }
        if (Type == null)
        {
            #if UNITY_EDITOR
                        Debug.LogError("Type is null. " + gameObject.transform.parent.name + " / " + gameObject.name);
            #endif
            return;
        }
        btn.onClick.AddListener(OpenUI);
    }
    
    protected virtual void OnDisable()
    {
        btn.onClick.RemoveAllListeners();
    }
    
    private void OpenUI()
    {
            if (_wnd == null)
            {
            _wnd = UIController.Instance.GetUI(Type); 
                if (_wnd == null)
                {
                    #if UNITY_EDITOR
                        Debug.LogError("Window " + Type + "not found." + gameObject.transform.parent.name + "/" + gameObject.name);
                    #endif
                    return;
                }
            }
            _wnd.Open();
        }
}

    

