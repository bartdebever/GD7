using System;
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
        private void Update()
        {
            if (Input.anyKeyDown && Input.GetKeyDown(KeyCode.F8))
            {
                QuickSaveStorage.Get.Save();
            }

            if (Input.anyKeyDown && Input.GetKeyDown(KeyCode.F9))
            {
                QuickSaveStorage.Get.Load();
            }
        }
    }
}
