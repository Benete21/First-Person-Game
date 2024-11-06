using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]

public class ItemInfo : ScriptableObject
{ 
    public int id;
    public string itemName;
    public string itemDescription;
}
