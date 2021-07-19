using System;

namespace Services.Route.Core.Entities
{
    [Flags]
    public enum ActivityKind
    {
        Walking = 1 << 0,
        Hike    = 1 << 1,
        Running = 1 << 2,
        Cycling = 1 << 3
    }
}