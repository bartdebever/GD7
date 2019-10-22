using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Scripts.Quickloading
{
    public interface ISaveableScript
    {
        Dictionary<string, object> Save();

        void Load(Dictionary<string, object> saveState);

        GUID UniqueId { get; }
    }
}
