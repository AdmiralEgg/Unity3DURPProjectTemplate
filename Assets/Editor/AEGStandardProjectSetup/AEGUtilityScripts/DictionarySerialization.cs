using System.Collections.Generic;
using System;
using UnityEngine;

public static class DictionarySerialization
{
    // Define a Serializable KeyValuePair class for Unity serialization
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    // Define a Serializable dictionary wrapper
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> _serializedList = new List<SerializableKeyValuePair<TKey, TValue>>();

        public void OnBeforeSerialize()
        {
            _serializedList.Clear();
            foreach (var kvp in this)
            {
                _serializedList.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();
            foreach (var kvp in _serializedList)
            {
                this[kvp.Key] = kvp.Value;
            }
        }
    }
}