using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameEventSystem
{
    [Serializable]
    public class GameEventCondition : ScriptableObject
    {
        public string conditionKey;
        public string conditionValue;
        public ConditionComparer conditionComparer;
        public StructCast structCast;

        public enum ConditionComparer
        {
            EqualTo = 0,
            GreaterOrEqualTo = 1,
            LessOrEqualTo = 2,
            GreaterThan = 3,
            LessThan = 4,
            Contains = 5
        }

        public enum StructCast
        {
            Bool = 0,
            Int = 1,
            Float = 2,
            String = 3,
        }
    }
}
