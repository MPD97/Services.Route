using System;
using System.Collections;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Services.Route.Application.DTO;

namespace Services.Route.Application.Queries
{
    public class GetRoute : IQuery<RouteDto>
    {
        public Guid RouteId { get; set; }
    }
}