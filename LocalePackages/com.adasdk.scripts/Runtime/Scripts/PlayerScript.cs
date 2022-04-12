using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private SriptDBItem info;
    [SerializeField]
    private List<ScriptAction> actions = new List<ScriptAction>();

    private ScriptAction currentAction;

    public List<ScriptAction> Actions => actions;

    public SriptDBItem Info
    {
        get
        {
            return info;
        }
        set
        {
            info = value;
        }
    }

    #region EDITOR

#if UNITY_EDITOR
    public void AddAction(Type _type)
    {
        ScriptAction component = gameObject.AddComponent(_type) as ScriptAction;
        component.Owner = this;

        if (actions == null)
            actions = new List<ScriptAction>();

        actions.Add(component);
    } 
    
    public void DeleteAction(ScriptAction action)
    {
        actions.Remove(action);
        DestroyImmediate(action, true);
    }

    public void MoveActionUp(ScriptAction action)
    {
        int index = actions.IndexOf(action);
        if (index > 0)
        {
            actions.Move(action, index - 1);
            UnityEditorInternal.ComponentUtility.MoveComponentUp(action);
        } 
    }

    public void InitActions()
    {
        actions = new List<ScriptAction>();
    }

    public void MoveActionDown(ScriptAction action)
    {
        int index = actions.IndexOf(action);
        if (index < actions.Count - 1)
        {
            actions.Move(action, index + 1);
            UnityEditorInternal.ComponentUtility.MoveComponentDown(action);
        }
    }

#endif
    #endregion

    private void Start()
    {
        StartAction(actions[0]);
    }

    private void SetScriptsDone()
    {
        PlayerScriptsController.Instance.SetScriptDone(this);
    }

    private void StartAction(ScriptAction action)
    {
        currentAction = action;
        currentAction.OnStart();
    }

    public void OnActionEnd(ScriptAction action)
    {
        int index = Actions.IndexOf(action);
        if (index == Actions.Count-1)
        {
            SetScriptsDone();
        }
        else
        {
            StartAction(Actions[++index]);
        }

    }
}
