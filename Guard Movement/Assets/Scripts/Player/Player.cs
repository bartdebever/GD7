using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Camera BackupCamera;
    public GameObject CarryingObject;

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

    private StealableObject _carryingObject;

    // Start is called before the first frame update
    private void Start()
    {
        Game.PlayerObject = gameObject;

        _actions = new Dictionary<KeyCode, Action>
        {
            {KeyCode.E, StealObjects},
            {KeyCode.F, DeliverObject }
        };
    }

    private void DeliverObject()
    {
        // Player is not carrying anything, no need to do anything.
        if (_carryingObject == null)
        {
            return;
        }

        var deliverySystem = FindComponentInRange<DeliverySystem>();

        if (deliverySystem.ReceiveItem(_carryingObject))
        {
            _carryingObject = null;
        }

        CarryingObject.SetActive(false);
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
        // If the player is carrying an object, they should not be able to pick
        // up another object.
        if (_carryingObject != null)
        {
            return;
        }

        var stealableObject = FindComponentInRange<StealableObject>();
        
        // No object found, return.
        if (stealableObject == null)
        {
            return;
        }

        stealableObject.StealObject();

        _carryingObject = stealableObject;

        CarryingObject.SetActive(true);
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

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // TODO Invoke never happens as object already gets destroyed.
        Invoke(nameof(ReloadScene), 1);
    }

    private void ReloadScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Finds the first object within the sphere collider of type <typeparamref name="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">
    /// The type of component to be found.
    /// </typeparam>
    /// <returns>The found component or null.</returns>
    private TComponent FindComponentInRange<TComponent>()
        where TComponent : MonoBehaviour
    {
        var hitColliders = Physics.OverlapSphere(gameObject.transform.position, _interactRadius);

        return hitColliders
            .Where(hitCollider => hitCollider.gameObject.GetComponent<TComponent>() != null)
            .Select(hitCollider => hitCollider.GetComponent<TComponent>())
            .FirstOrDefault();
    }
}
