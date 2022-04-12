using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UserInterface
{
    public static UIMain Instance { get; private set; }

    private void OnEnable()
    {
        Instance = this;
        if (Serializer.IsDeserialized) 
            Open();
        else
            Serializer.OnDeserialize += OnDeserialize;
        
    }

    private void OnDestroy()
    {
        Serializer.OnDeserialize -= OnDeserialize;
    }

    private void OnDeserialize()
    {
        Serializer.OnDeserialize -= OnDeserialize;
        Open(); 
    }

    public override void Open()
    {
        base.Open();
        Show<UISelectCharacter>();
    }

    protected override void BeforeClose()
    {
        base.BeforeClose();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit(); 
        }
    }


    public void Quit()
    {
        UserInterface ui = UIController.Instance.AnyWindowCanBeClosed();
        if (ui != null)
            ui.Close();
        else 
            Application.Quit();
    }

}
