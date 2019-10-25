using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Game
    {
        public static GameObject PlayerObject { get; set; }

        public static UISingleton UI { get; set; }

        public static bool IsPaused { get; set; } = true;
    }
}
