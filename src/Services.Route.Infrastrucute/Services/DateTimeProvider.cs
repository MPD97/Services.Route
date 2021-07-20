using System;
using Services.Route.Application.Services;

namespace Services.Route.Infrastrucute.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}