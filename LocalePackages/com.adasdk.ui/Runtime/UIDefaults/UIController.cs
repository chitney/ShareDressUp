using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIController : Controller
{
    public static UIController Instance => Controllers.Get<UIController>();

    public static Action<UserInterface> OnUserInterfaceOpened;

    public static Action<UserInterface> OnUserInterfaceClosed;

    [SerializeField]
    private Button shadow;

    private List<UserInterface> shadowOwners = new List<UserInterface>();

    private Dictionary<Type, UserInterface> UserInterfaces;

    [SerializeField]
    private List<UserInterface> AllUserInterfaces;


    private void Awake()
    {
        shadow.onClick.AddListener(CloseByShadow);
    }

    private void CloseByShadow()
    {
        if (shadowOwners.Count>0&& !shadowOwners[0].DoNotClose)
            shadowOwners[0].Close();
    }

    public void CloseAll()
    {
        foreach (var ui in UserInterfaces)
            ui.Value.CloseAll();
    }

    public void OnWindowClosed(UserInterface userInterface)
    {
        if (userInterface.ShowShadow && shadowOwners.Count>0) 
        {
            if (shadowOwners[0] == userInterface && shadowOwners.Count==1)
            {
                shadow.gameObject.SetActive(false);
                shadowOwners.Remove(userInterface);
            }
            else
            {
                shadowOwners.Remove(userInterface);
                shadow.transform.SetSiblingIndex(shadowOwners[0].transform.GetSiblingIndex() - 1);
            }
        }
        OnUserInterfaceClosed?.Invoke(userInterface);
    }

    public void OnOpened(UserInterface ui)
    {
        if (ui.ShowShadow )
        {
            shadow.transform.SetSiblingIndex(ui.transform.GetSiblingIndex() - 1);
            shadow.gameObject.SetActive(true);
            shadowOwners.Insert(0, ui);
        }
    }

    public UserInterface AnyWindowCanBeClosed()
    {
        foreach (var ui in UserInterfaces)
        {
            if (ui.Value.gameObject.activeSelf && !ui.Value.DoNotClose) 
                return ui.Value;
        }
        return null;
    }

    /// <summary>
    /// Открывает окно выбранного типа и возвращает ссылку на него
    /// </summary>
    public UserInterface OpenUI<TUserInterface>() where TUserInterface : UserInterface
    {
        UserInterface ui = GetUI<TUserInterface>();
        if (ui != null && !ui.gameObject.activeSelf)
        {
            ui.Open();
        }
        return ui;
    }


    /// <summary>
    /// Закрывает окно выбранного типа 
    /// </summary>
    public void CloseUI<TUserInterface>() where TUserInterface : UserInterface
    {
        UserInterface ui = GetUI<TUserInterface>();
        if (ui != null)
        {
            ui.Close();
        }
    }

    public TUserInterface GetUI<TUserInterface>() where TUserInterface : UserInterface
    {
        try
        {
            if (UserInterfaces == null || UserInterfaces.Count==0)
                Init();
            
            return (TUserInterface)UserInterfaces[typeof(TUserInterface)];
        }
        catch
        {
            Debug.LogError("interface was not found");
            return null;
        }
    }

    public UserInterface GetUI(Type type) 
    {
        try
        {
            return UserInterfaces[type];
        }
        catch
        {
            return null;
        }
    }


    public override void Init()
    {
        UserInterfaces = new Dictionary<Type, UserInterface>();
        foreach (var ui in AllUserInterfaces)
            UserInterfaces.Add(ui.GetType(), ui);
    }

#if UNITY_EDITOR

    [ExecuteInEditMode]
    private void OnEnable()
    {
        GetComponentsInChildren(true, AllUserInterfaces);
    }

#endif

}
