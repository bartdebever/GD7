using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera BackupCamera;

    private void OnDestroy()
    {
        BackupCamera.gameObject.SetActive(true);
        Game.IsPaused = true;
    }
}
