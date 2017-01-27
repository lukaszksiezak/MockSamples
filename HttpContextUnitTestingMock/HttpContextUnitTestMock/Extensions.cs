using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpContextUnitTestMock
    {
    public static class Extensions
        {
        public static void AddMany<T>(this List<T> list, params T[] users)
            {
            list.AddRange(users);
            }
        }
    }