using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    /// <summary>
    /// The key used to pause the game.
    /// </summary>
    public KeyCode Key;

    public Image PausePanel;

    // Update is called once per frame
    void Update()
    {
        // If no key is pressed or the right key is not pressed.
        // Stop executing this script as no action is required.
        if (!Input.anyKey || !Input.GetKeyDown(Key))
        {
            return;
        }

        // Pause the game or unpause the game.
        Game.IsPaused = !Game.IsPaused;

        PausePanel.gameObject.SetActive(Game.IsPaused);
    }
}
