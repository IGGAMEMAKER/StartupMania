﻿using System;

namespace Assets.Utils
{
    public static class RandomEnum<T>
    {
        public static T GenerateValue()
        {
            Array array = Enum.GetValues(typeof(T));

            return PickRandomItem(array);
        }

        static T PickRandomItem(Array array)
        {
            int id = UnityEngine.Random.Range(0, array.Length);

            return (T)array.GetValue(id);
        }
    }
}
