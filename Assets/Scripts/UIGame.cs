using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGame : UserInterface
{
    [SerializeField]
    private UIList items;
    [SerializeField]
    private UIList variants;
    [SerializeField]
    private Doll doll;
    [SerializeField]
    private GameObject btns;
    [SerializeField]
    private Button changeBgBtn;
    [SerializeField]
    private Button saveBtn;

    private static UIGame _instance;
    public static UIGame Instance
    {
        get
        {
            if (_instance == null) _instance = UIController.Instance.GetUI<UIGame>();
            return _instance;
        }
    }

    CharactersInfo currentCharacter;
    CharachtersItem currentItem;

    public void Open(CharactersInfo character)
    {
        changeBgBtn.onClick.AddListener(ShowBgVariants);
        saveBtn.onClick.AddListener(Save);

        currentCharacter = character;

        UIItemButton btn;
        items.Clear();

        doll.Init(character);

        for (int i = 0; i < currentCharacter.Items.Length; i++)
        {
            CharachtersItem item = currentCharacter.Items[i];
            btn = items.Get(i) as UIItemButton;
            int index = i;
            btn.Init(item.Icon, () => SetCurrentsVariants(item, index));
        }

        SetCurrentsVariants(currentCharacter.Items[0], 0);

        Doll.OnItemRemoved += CheckItemInGame;

        Open();

        //GoogleTools.LogLevelStart(character.Name);
    }

    private void SetCurrentsVariants(CharachtersItem item, int index)
    {
        UIItemButton btn;
        currentItem = item;
        variants.Clear();
        for (int i = 0; i < item.Variants.Length; i++)
        {
            btn = variants.Get(i) as UIItemButton;
            int variant = i;
            btn.Init(item.Variants[i].icon, () => SetItemVariant(item,index, variant));
        }

        btns.SetActive(!item.isDefault && doll.IsItemInGame(item.category));
    }

    private void SetItemVariant(CharachtersItem item, int index, int variant)
    {
        doll.InitItem(item, variant);
        btns.SetActive(!item.isDefault && doll.IsItemInGame(item.category));
    }


    WaitForSeconds ms = new WaitForSeconds(0.05f);

    public IEnumerator OpenCoroutine()
    {
        yield return ms;
    }

    protected override void OnClose()
    {
        base.OnClose();
        Doll.OnItemRemoved -= CheckItemInGame;
        changeBgBtn.onClick.RemoveAllListeners();
        saveBtn.onClick.RemoveAllListeners();
        Show<UISelectCharacter>();
        SetBgVariants(0);
    }

    #region Save
    public CameraScreenShot screenMaker;
    public Doll doll4ScreenShot;
    public Image bg4ScreenShot; 
    public Image mainBg4ScreenShot;

    private void Save()
    {
        screenMaker.gameObject.SetActive(true);
        doll4ScreenShot.CopyImages(doll);
        if (bg.gameObject.activeSelf)
        {
            bg4ScreenShot.sprite = bg.sprite;
            bg4ScreenShot.enabled = true;
            mainBg4ScreenShot.enabled = false;
        }
        else
        {
            mainBg4ScreenShot.enabled = true;
            bg4ScreenShot.enabled = false;
        }

        ScreenShotsSaver.Instance.Save(screenMaker.TakeScreenShot());
    }

    #endregion

    #region BG
    [SerializeField]
    private Animator gameAnimation;
    [SerializeField]
    private Image bg;

    private readonly int ShowDefaultBg = Animator.StringToHash("Enable");

    private void ShowBgVariants()
    {
        variants.Clear();
        UIItemButton btn;
        for (int i = 0; i < CharactersDatabase.Instance.Bachgrounds.Length; i++)
        {
            btn = variants.Get(i) as UIItemButton;
            int variant = i;
            btn.Init(CharactersDatabase.Instance.Bachgrounds[i].icon, () => SetBgVariants(variant));
        }

        btns.SetActive(false);
    }

    private void SetBgVariants(int index)
    {
        if (index == 0)
        {
            gameAnimation.SetBool(ShowDefaultBg, true);
            bg.gameObject.SetActive(false);
        }
        else
        {
            gameAnimation.SetBool(ShowDefaultBg, false);
            bg.gameObject.SetActive(true);
            bg.sprite = CharactersDatabase.Instance.Bachgrounds[index].sprite;
        }
    }


    #endregion

    #region Buttons > < x
    private void CheckItemInGame()
    {
        if (currentItem!=null)
            btns.SetActive(!currentItem.isDefault && doll.IsItemInGame(currentItem.category));
    }

    public void _UI_UpCurrentItem()
    {
        doll.Up(currentItem.category);
    }

    public void _UI_DownCurrentItem()
    {
        doll.Down(currentItem.category);
    }

    public void _UI_DeleteCurrentItem()
    {
        doll.Remove(currentItem.category);
    }
    #endregion 

    protected override void _UI_OnOpen()
    {
        base._UI_OnOpen();
    }
}
