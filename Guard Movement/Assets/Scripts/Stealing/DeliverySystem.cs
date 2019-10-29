using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public bool ReceiveItem(StealableObject item)
    {
        Debug.Log(item);
        return true;
    }
}
