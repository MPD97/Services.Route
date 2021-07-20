using System;

namespace Services.Route.Application.Exceptions
{
    public class InvalidRouteDifficultyException: AppException
    {       
        public override string Code { get; } = "invalid_route_difficulty";
        public Guid UserId { get; }
        public string Difficulty { get; }
        public InvalidRouteDifficultyException(Guid userId, string difficulty) 
            : base($"User with id: {userId} has given invalid route difficulty: {difficulty}.")

        {
            UserId = userId;
            Difficulty = difficulty;
        }
    }
}