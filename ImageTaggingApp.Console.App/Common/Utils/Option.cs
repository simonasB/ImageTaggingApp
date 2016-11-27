using System.Collections;
using System.Collections.Generic;

namespace ImageTaggingApp.Console.App.Common.Utils {
    /// <summary>
    /// To avoid returning null reference from queries introducing option functional type.
    /// Object of this class is the IEnumerable which either contains one element or contains none.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Option<T> : IEnumerable<T> {
        private readonly T[] data;

        private Option(T[] data) {
            this.data = data;
        }

        public static Option<T> Create(T element) {
            return new Option<T>(new[] {element});
        }

        public static Option<T> CreateEmpty() {
            return new Option<T>(new T[0]);
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>) this.data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
