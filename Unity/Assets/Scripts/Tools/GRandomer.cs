using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Tools
{
    public class GRandomer
    {
        private static Random _randomer = new Random();

        public static int RandomMinAndMax(int min, int max)
        {
            return _randomer.Next(min, max);
        }

        public static T RandomList<T>(List<T> list)
        { 
            return list[_randomer.Next(list.Count)];
        }

        public static T RandomArray<T>(T[] array)
        {
            return array[_randomer.Next(array.Length)];
        }

        public static bool Probability10(int value)
        {
            if (value <= 0) return false;
            if (value >= 10) return true;
            return _randomer.Next(0, 10) >=value;
        }

        public static bool Probability100(int value)
        {
            if (value <= 0) return false;
            if (value >= 100) return true;
            return _randomer.Next(0, 100) >= value;
        }
    }
}
