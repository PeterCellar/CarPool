using System.ComponentModel.DataAnnotations.Schema;

namespace CarPool.DAL.Entities
{
    public record CarEntity (
        Guid Id,
        string Manufacturer,
        string Type,
        DateTime FirstRegistration,
        Guid OwnerId,
        int Seats,
        string? ImageUrl) : IEntity
    {
        //Automapper requires parameter less constructor for collection synchronization for now
#nullable disable
        public CarEntity() : this(default, default, default, default, default, default, default) { }
#nullable enable

        // Car is used for 0 .. n rides
        public ICollection<RideEntity?> RidesUsedFor { get; init; } = new List<RideEntity?>();

        // Car is owned by one user
       
        public UserEntity Owner { get; init; }
    }
}