
namespace CarPool.BL
{
    public abstract record ModelBase : IModel
    {
        public Guid Id { get; set; }
    }
}
