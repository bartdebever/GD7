using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Camera BackupCamera;

    /// <summary>
    /// The actions that can be performed by the player
    /// and their respective keycode.
    /// </summary>
    private Dictionary<KeyCode, Action> _actions;

    /// <summary>
    /// The radius in which a player can interact
    /// </summary>
    [SerializeField]
    private float _interactRadius = 2.5f;

    // Start is called before the first frame update
    private void Start()
    {
        Game.PlayerObject = gameObject;

        _actions = new Dictionary<KeyCode, Action>
        {
            {KeyCode.E, StealObjects}
        };
    }

    private void Update()
    {
        foreach (var keyValuePair in _actions)
        {
            if (Input.GetKeyDown(keyValuePair.Key))
            {
                keyValuePair.Value.Invoke();
            }
        }
    }

    private void StealObjects()
    {
        var hitColliders = Physics.OverlapSphere(gameObject.transform.position, _interactRadius);

        var stealableObjects = hitColliders
            .Where(hitCollider => hitCollider.gameObject.GetComponent<StealableObject>() != null)
            .Select(hitCollider => hitCollider.GetComponent<StealableObject>());

        foreach (var stealableObject in stealableObjects)
        {
            stealableObject.StealObject();
        }
    }

    private void OnDestroy()
    {
        // Prevent a bug when on closing the Unity editor the Player would
        // get destroyed and trigger this script.
        if (BackupCamera == null || BackupCamera.gameObject == null)
        {
            return;
        }

        // Before destroying the player, a backup camera needs to be activated.
        // The main camera will be destroyed and we don't want the game to crash or not function.
        BackupCamera.gameObject.SetActive(true);

        // When the player dies the game should go into pause mode.
        // This stops a lot of systems which might also rely on the player.
        // Most of these systems use Game.PlayerObject
        Game.IsPaused = true;

        Game.UI.SetBottomText("Game over.");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // TODO Invoke never happens as object already gets destroyed.
        Invoke(nameof(ReloadScene), 1);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
