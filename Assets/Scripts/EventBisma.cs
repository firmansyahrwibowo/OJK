using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BismaEvent
{
    public class DurationCutEvent : GameEvent
    {
        public float Value;

        public DurationCutEvent(float value)
        {
            Value = value;
        }
    }
    public class ScoreSetEvent : GameEvent
    {
        public int Value;

        public ScoreSetEvent(int value)
        {
            Value = value;
        }
    }
}
