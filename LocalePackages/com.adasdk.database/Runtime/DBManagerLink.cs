using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManagerLink : MonoBehaviour
{
    [SerializeField]
    private DBManager _DBManager;

    private static DBManagerLink instance;
    private static DBManagerLink Instance {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DBManagerLink>();
            return instance;
        }
    }
    public static DBManager DBManager => Instance._DBManager;

    private void Awake()
    {
        instance = this;
    }
}
