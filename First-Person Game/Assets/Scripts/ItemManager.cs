using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{ 
    public static ItemManager Instance;
    public List<ItemInfo> Items = new List<ItemInfo>();

    private void Awake()
    {
        Instance = this;
    }
    public void Add(ItemInfo item)
    {
        Items.Add(item);
    }
    public void Remove(ItemInfo item)
    { 
        Items.Remove(item);
    }
}
