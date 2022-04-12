using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptsDatabase", menuName = "ScriptableObjects/DB/ScriptsDatabase")]
public class ScriptsDatabase : DBBase
{
    public static ScriptsDatabase Instance { get { return DBManager.Get<ScriptsDatabase>(); } }




}
