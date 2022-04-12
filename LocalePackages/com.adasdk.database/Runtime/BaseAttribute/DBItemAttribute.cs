using System;
using UnityEngine;

public class DBItemAttribute : PropertyAttribute
{

    public Type type;

    public DBItemAttribute(Type type)
    {
        this.type = type;
    }

}