using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public bool ReceiveItem(StealableObject item)
    {
        if (item == null)
        {
            return false;
        }

        Game.UI.IncreaseMoney(item.Earnings);
        return true;
    }
}
