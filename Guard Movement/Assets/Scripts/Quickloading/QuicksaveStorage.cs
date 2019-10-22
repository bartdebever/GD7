using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Quickloading
{
    public class QuicksaveStorage
    {
        private Dictionary<GUID, Dictionary<string, object>> _saveStates;
        private Dictionary<GUID, ISaveableScript> _scripts;

        private QuicksaveStorage()
        {
            _scripts = new Dictionary<GUID, ISaveableScript>();
            _saveStates = new Dictionary<GUID, Dictionary<string, object>>();
        }

        public static QuicksaveStorage Get { get; } = new QuicksaveStorage();

        public void AddScript(ISaveableScript script)
        {
            _scripts.Add(script.UniqueId, script);
        }

        public void Load()
        {
            foreach (var saveState in _saveStates)
            {
                var script = _scripts.First(savedScript => savedScript.Key == saveState.Key);

                script.Value.Load(saveState.Value);
            }
        }

        public void Save()
        {
            _saveStates = new Dictionary<GUID, Dictionary<string, object>>();
            foreach (var script in _scripts)
            {
                var saveState = script.Value.Save();

                _saveStates.Add(script.Key, saveState);
            }
        }
    }
}
