using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class ListHelper
    {
        // Usage Example:
        // var groupedByProperty = ListHelper.GroupByFunction(MyList, (item1, item2) => item1.MyProperty == item2.MyProperty);
        public static List<List<T>> GroupByFunction<T>(List<T> items, Func<T, T, bool> condition)
        {
            var groupedResults = new List<List<T>>();
            foreach (var item in items)
            {
              bool found = false;
              foreach (var group in groupedResults)
              {
                if (condition(group[0], item))
                {
                  group.Add(item);
                  found = true;
                  break;
                }
              }
              if (!found)
              {
                groupedResults.Add(new List<T> {item});
              }
            }
            return groupedResults;
        }
    }
}
