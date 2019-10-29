using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public bool ReceiveItem(StealableObject item)
    {
        Debug.Log(item);
        Game.UI.IncreaseMoney(item.Earnings);
        return true;
    }
}
