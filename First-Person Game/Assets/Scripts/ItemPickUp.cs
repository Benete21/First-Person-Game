using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemInfo ItemInfo;
    public void PutInInventory()
    {
        ItemManager.Instance.Add(ItemInfo);
    }
}
