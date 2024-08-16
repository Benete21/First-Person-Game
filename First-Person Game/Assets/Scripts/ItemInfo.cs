using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ClueItem.asset", menuName = "Inventory/Item", order = 0)]
public class ItemInfo : ScriptableObject
{
    [SerializeField]
    private string name;
    public string Name => name;

    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    private string description;
    public string Description => description;   

    
}
