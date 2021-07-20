using System;
using Convey.MessageBrokers.RabbitMQ;
using Services.Route.Application.Events.Rejected;
using Services.Route.Application.Exceptions;
using Services.Route.Core.Exceptions;
using InvalidPointOrderException = Services.Route.Application.Exceptions.InvalidPointOrderException;

namespace Services.Route.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                UserNotFoundException ex => new CreateRouteRejected(ex.Message, ex.Code),
                InvalidAggregateIdException ex => new CreateRouteRejected(ex.Message, ex.Code),
                InvalidPointOrderException ex => new CreateRouteRejected(ex.Message, ex.Code),
                InvalidRouteDescriptionException ex => new CreateRouteRejected(ex.Message, ex.Code),
                InvalidRouteNameException ex => new CreateRouteRejected(ex.Message, ex.Code),
                RouteDescriptionTooLongException ex => new CreateRouteRejected(ex.Message, ex.Code),
                RouteNameTooLongException ex => new CreateRouteRejected(ex.Message, ex.Code),
                RouteNameTooShortException ex => new CreateRouteRejected(ex.Message, ex.Code),
            };
    }
}