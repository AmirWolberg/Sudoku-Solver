using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace suduko
{
    /// <summary>
    /// Contains all self made list functions (designed to work as a generic class and inhereits all already available list functions from List<T>)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class List_funcs<T>: List<T>
    {
        /// <summary>
        /// Input: gets 2 lists of type T
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns>Output: returns a list containing only the values present in both lists</returns>
        public static List<T> Merge_lists(List<T> l1, List<T> l2)
        {
            List<T> l_return = new List<T>();
            foreach (T i in l1)
            {
                foreach (T j in l2)
                {
                    if (i.Equals(j))
                    {
                        l_return.Add(i);
                    }
                }
            }

            return l_return;
        }
    }
}
