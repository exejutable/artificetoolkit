using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.ArtificeToolkit.Editor.Artifice_SerializedDictionary
{
    [Serializable]
    public class SerializedDictionary<TK, TV> : IDictionary<TK, TV>, ISerializationCallbackReceiver
    {
        [Serializable]
        private class SerializedDictionaryPair
        {
            public TK Key;
            public TV Value;

            public SerializedDictionaryPair(TK tk, TV tv)
            {
                Key = tk;
                Value = tv;
            }
        }
    
        [SerializeField]
        private List<SerializedDictionaryPair> _list = new();
    
        public Dictionary<TK, TV> Dict = new();

        #region ISerializationCallbackReceiver Interface
    
        public void OnBeforeSerialize()
        {
            _list.Clear();
            foreach (var pair in Dict)
            {
                _list.Add(new SerializedDictionaryPair(pair.Key, pair.Value));
            }
        }
        public void OnAfterDeserialize()
        {
            Dict.Clear();
            foreach (var entry in _list)
            {
                if (entry.Key == null) // TODO[zack]: warning here. Something is not serialzable.
                    continue;
                Dict.Add(entry.Key, entry.Value);
            }
        }
    
        #endregion
    
        #region IDictionary Interface 

        public TV this[TK key]
        {
            get => Dict[key];
            set => Dict[key] = value;
        }

        public ICollection<TK> Keys => Dict.Keys;

        public ICollection<TV> Values => Dict.Values;

        public int Count => Dict.Count;

        public bool IsReadOnly => ((IDictionary<TK, TV>)Dict).IsReadOnly;

        public void Add(TK key, TV value) => Dict.Add(key, value);

        public void Add(KeyValuePair<TK, TV> item) => ((IDictionary<TK, TV>)Dict).Add(item);

        public void Clear() => Dict.Clear();

        public bool Contains(KeyValuePair<TK, TV> item) => ((IDictionary<TK, TV>)Dict).Contains(item);

        public bool ContainsKey(TK key) => Dict.ContainsKey(key);

        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex) => ((IDictionary<TK, TV>)Dict).CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => Dict.GetEnumerator();

        public bool Remove(TK key) => Dict.Remove(key);

        public bool Remove(KeyValuePair<TK, TV> item) => ((IDictionary<TK, TV>)Dict).Remove(item);

        public bool TryGetValue(TK key, out TV value) => Dict.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
        #endregion
    }
}