﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.QuickLoading
{
    public class Quicksaver : MonoBehaviour
    {
        public KeyCode saveKey;
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
