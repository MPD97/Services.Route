using System;

namespace Services.Route.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}