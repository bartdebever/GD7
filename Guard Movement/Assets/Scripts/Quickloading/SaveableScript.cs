using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Scripts.QuickLoading
{
    /// <summary>
    /// Interface to use when making an entity that should quick save.
    /// </summary>
    public interface ISaveableScript
    {
        /// <summary>
        /// Stores the important data for the entity in a dictionary.
        /// This dictionary will be used as the state of the entity.
        /// </summary>
        /// <returns>
        /// A dictionary filled with the data from the entity.
        /// </returns>
        Dictionary<string, object> Save();

        /// <summary>
        /// Loads the provided <paramref name="saveState"/> previously made by
        /// the <see cref="Save"/> method, into the entity.
        /// </summary>
        /// <param name="saveState"></param>
        void Load(Dictionary<string, object> saveState);

        /// <summary>
        /// The unique identifier for this entity.
        /// Most be completely unique in the entire application.
        /// Recommended to use <see cref="GUID.Generate"/> method in the Start method.
        /// </summary>
        GUID UniqueId { get; }
    }
}
