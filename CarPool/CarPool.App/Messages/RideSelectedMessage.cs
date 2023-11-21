using CarPool.BL;

namespace CarPool.App.Messages
{
    public class RideSelectedMessage
    {
        public RideDetailModel Ride { get; }

        public RideSelectedMessage(RideDetailModel ride)
        {
            Ride = ride;
        }
    }
}
