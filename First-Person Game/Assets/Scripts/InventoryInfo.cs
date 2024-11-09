using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInfo : MonoBehaviour
{
    [SerializeField]
    public string inventoryName;
    public int inventoryType;
    public string InventoryDescrip;

    public string getInventoryDescrip()
    {
        return InventoryDescrip;
    }
}
