using System;

namespace Helper
{
    [Serializable]
    public struct SerializableKeyValuePair <TK, TV>
    {
        public TK key;
        public TV value;
    }
}