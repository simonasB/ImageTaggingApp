using System;
using System.Threading;

namespace ImageTaggingApp.Console.App.Common.Utils {
    internal class ThreadSafe {
        public static double Add(ref double location1, double value) {
            double newCurrentValue = location1; // non-volatile read, so may be stale
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (Math.Abs(newCurrentValue - currentValue) < 0.00001)
                    return newValue;
            }
        }
    }
}
