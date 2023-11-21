namespace CarPool.DAL.Entities
{
    public record UserEntity(
        Guid Id,
        string UserName,
        string Name,
        string Surname,
        string? ImageUrl) : IEntity
    {

        // User offers 0 .. n rides
        public ICollection<RideEntity?> Offering { get; init; } = new List <RideEntity?>();

        // User owns 0 .. n cars
        public ICollection<CarEntity?> CarsOwned { get; init; } = new List<CarEntity?>();

        public ICollection<NumberOfRidesEntity> RidesTaking { get; init; } = new List<NumberOfRidesEntity>();

    }
}