using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPool.DAL.Entities
{
    public record NumberOfRidesEntity(
        Guid Id,
        Guid UserId,
        Guid RideId
        ):IEntity
    {
        // Number of Rides is part of 0 .. n rides
        public RideEntity? Ride { get; init; }

        // Number of Rides is taken by 1 .. n passengers
        public UserEntity? User { get; init; }
    }
}
