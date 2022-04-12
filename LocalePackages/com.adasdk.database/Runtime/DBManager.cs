using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBManager", menuName = "ScriptableObjects/DBManager", order = 0)]
[ExecuteInEditMode]
public class DBManager : ScriptableObject
{
    [SerializeField]
    private ScriptableObject[] ScriptableObjects;

    private Dictionary<Type, object> cachedScriptableObjects;

    public static DBManager Instance => instance;

    private static DBManager instance;

    private void InitDictionary() {
        cachedScriptableObjects = new Dictionary<System.Type, object>();
        foreach (ScriptableObject obj in ScriptableObjects)
            cachedScriptableObjects.Add(obj.GetType(), obj);
    }

    private T GetByType<T>()
    {
        Type type = typeof(T);
        if (cachedScriptableObjects==null)
            InitDictionary();
        return (T)cachedScriptableObjects[type];
    }

    public static T Get<T>()
    {
        if (instance == null)
            instance = Resources.Load<DBManager>("DBManager");

        return Instance.GetByType<T>();
    }

    void Awake()
    {
        instance = Resources.Load<DBManager>("DBManager");
    }

    void Start()
    {
        if (instance == null)
            instance = DBManagerLink.DBManager;
    }
#if UNITY_EDITOR
    void OnEnable()
    {
        instance = Resources.Load<DBManager>("DBManager");
    }
#endif
}
