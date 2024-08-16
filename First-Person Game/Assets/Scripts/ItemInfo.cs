using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ClueItem.asset", menuName = "Inventory/Item", order = 0)]
public class ItemInfo : ScriptableObject
{
    [SerializeField]
    private string names;
    public string Names => names;

    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    private string description;
    public string Description => description;

    public ItemInfo(string names, string description)
    {
        // Assigning
        this.name = name;
        this.description = description;
    }
    public string getName()
    {
        return names;
    }
    public string getDescription() 
    { 
        return description;
    }

}
