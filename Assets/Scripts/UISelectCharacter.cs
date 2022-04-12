using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI for choosing a doll
/// </summary>
public class UISelectCharacter : UserInterface
{
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Animator gameAnimation;
    [SerializeField]
    private UIList items;

    private readonly int ShowGame = Animator.StringToHash("Start");

    private int currentCharecterId;
    private Dictionary<int, UIItemCheckBox> itemsList = new Dictionary<int, UIItemCheckBox>();

    protected override void OnClose()
    {
        gameAnimation.SetBool(ShowGame, true);
        UIGame.Instance.Open((CharactersInfo)CharactersDatabase.Instance.Items[currentCharecterId]);
        base.OnClose();
    }

    public override void Open()
    {
        gameAnimation.SetBool(ShowGame, false);
        closeButton.onClick.AddListener(()=>Close());

        for (int i = 0; i < CharactersDatabase.Instance.Items.Count; i++)
        {
            CharactersInfo item = (CharactersInfo)CharactersDatabase.Instance.Items[i];

            UIItemCheckBox checkBox;

            if (itemsList.ContainsKey(item.id))
            {
                checkBox = (UIItemCheckBox)items[i];
            }
            else
            {
                checkBox = (UIItemCheckBox)items[i];
                itemsList.Add(item.id, checkBox);
            }

            checkBox.Init(item.Icon, () => SetCurrentCharactersInfo(item.id), i == 0 ? 0 : 1);
        }

        SetCurrentCharactersInfo(1);

        base.Open();
    }

    private void SetCurrentCharactersInfo(int id)
    {
        foreach (var item in itemsList)
        {
            if (item.Key == id)
            {
                currentCharecterId = id;
                item.Value.SetMode(0);
            }
                
            else item.Value.SetMode(1);
        }
    }
}
