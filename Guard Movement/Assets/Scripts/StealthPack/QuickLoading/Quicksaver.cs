﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.QuickLoading
{
    /// <summary>
    /// Simple MonoBehaviour that allows for entities to be quick saved and
    /// loaded.
    /// </summary>
    public class Quicksaver : MonoBehaviour
    {
        /// <summary>
        /// The key used to save the current entities.
        /// </summary>
        public KeyCode saveKey;

        /// <summary>
        /// The key used to load the current entities.
        /// </summary>
        public KeyCode loadKey;
        
        private void Update()
        {
            if (Input.anyKeyDown && Input.GetKeyDown(saveKey))
            {
                QuickSaveStorage.Get.Save();
            }

            if (Input.anyKeyDown && Input.GetKeyDown(loadKey))
            {
                QuickSaveStorage.Get.Load();
            }
        }
    }
}
