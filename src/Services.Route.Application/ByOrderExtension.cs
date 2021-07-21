using System.Collections;
using System.Collections.Generic;
using Services.Route.Core.ValueObjects;

namespace Services.Route.Application
{
    public class ByOrderExtension : IComparer<Point>
    {
        public int Compare(Point x, Point y)
            => (new CaseInsensitiveComparer()).Compare(x.Order, y.Order);
    }
}