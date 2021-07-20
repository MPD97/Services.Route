using System;
using Convey.Types;
using Services.Route.Core.Entities;

namespace Services.Route.Infrastructure.Mongo.Documents
{
    public class UserDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }  
        public State State { get; set; }
    }
}