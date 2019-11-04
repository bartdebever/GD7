using UnityEngine;

namespace Assets.Scripts
{
    public static class Game
    {
        /// <summary>
        /// A static version of the player object.
        /// Can be null when not yet set or destroyed.s
        /// </summary>
        public static GameObject PlayerObject { get; set; }

        /// <summary>
        /// The manager of the UI. To be treated like a singleton.
        /// </summary>
        public static UISingleton UI { get; set; }

        public static PatternGenerator PatternGenerator { get; set; }

        /// <summary>
        /// Defines if the game is paused or not.
        /// </summary>
        public static bool IsPaused { get; set; } = true;
    }
}
