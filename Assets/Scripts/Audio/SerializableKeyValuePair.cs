using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public struct SerializableKeyValuePair <TK, TV>
    {
        public TK key;
        public TV value;
    }
}