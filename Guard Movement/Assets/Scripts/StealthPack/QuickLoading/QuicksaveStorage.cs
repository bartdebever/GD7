﻿using System;
using System.Collections.Generic;
using System.Linq;
 using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.QuickLoading
{
    /// <summary>
    /// A singleton storage class for saving save states.
    /// </summary>
    public class QuickSaveStorage
    {
        /// <summary>
        /// The states gathered from the <see cref="_scripts"/>.
        /// </summary>
        private readonly Dictionary<GUID, Dictionary<string, object>> _saveStates;

        /// <summary>
        /// The dictionary where the scripts are saved.
        /// </summary>
        private readonly Dictionary<GUID, ISaveableScript> _scripts;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickSaveStorage"/> class.
        /// </summary>
        private QuickSaveStorage()
        {
            _scripts = new Dictionary<GUID, ISaveableScript>();
            _saveStates = new Dictionary<GUID, Dictionary<string, object>>();
        }

        /// <summary>
        /// Gets the instance of the <see cref="QuickSaveStorage"/> class.
        /// </summary>
        public static QuickSaveStorage Get { get; } = new QuickSaveStorage();

        /// <summary>
        /// Adds the <paramref name="script"/> to the list of scripts to call.
        /// </summary>
        /// <param name="script">
        /// The script that can be loaded and saved on demand.
        /// </param>
        /// <exception cref="ArgumentException">
        /// When either the <paramref name="script"/> is null or the
        /// <see cref="ISaveableScript.UniqueId"/> is already found within the dictionary.
        /// </exception>
        public void AddScript(ISaveableScript script)
        {
            if (script == null)
            {
                throw new ArgumentNullException($"{nameof(script)} can not be null.");
            }

            if (_scripts.ContainsKey(script.UniqueId))
            {
                throw new ArgumentException(
                    $"{nameof(script)}.{nameof(script.UniqueId)} was already used.");
            }

            _scripts.Add(script.UniqueId, script);
        }

        /// <summary>
        /// Loads the currently stored save states into the entities.
        /// </summary>
        public void Load()
        {
            // If the player tries to load a save state while there are none.
            // Don't bother to do anything.
            if (_saveStates == null || !_saveStates.Any())
            {
                return;
            }

            // Go over all available save states.
            // TODO What about entities that spawned in?
            foreach (var saveState in _saveStates)
            {
                // Find the first script that belongs to this save state.
                var script = _scripts.First(savedScript => savedScript.Key == saveState.Key);

                // Load the save state.
                try
                {
                    script.Value.Load(saveState.Value);
                }
                catch (Exception)
                {
                    Debug.LogError($"Tried to load a save state but operation failed for entity {saveState.Key}");
                }
            }
        }

        /// <summary>
        /// Saves the data from all entities into the <see cref="_saveStates"/>.
        /// </summary>
        public void Save()
        {
            // Clear the states that are currently saved.
            _saveStates.Clear();

            // Go over all scripts available
            foreach (var script in _scripts)
            {
                // If the script has been destroyed or no longer available,
                // just continue.
                if (script.Value == null)
                {
                    continue;
                }

                try
                {
                    var saveState = script.Value.Save();
                    _saveStates.Add(script.Key, saveState);
                }
                catch
                {
                    Debug.LogError($"Tried to add a save state for object {script.Key} but failed.");
                }
            }
        }
    }
}
