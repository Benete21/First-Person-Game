using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemInfo
{ 
    public string names;
    [TextArea(3, 10)]
    public string[] itemNames;

    public string description;
    [TextArea(3, 10)]
    public string[] itemDesc;

}
