using CarPool.BL;

namespace CarPool.App.Messages
{
    public class CarSelectedMessage
    {
        public CarDetailModel Car { get;  }

        public CarSelectedMessage(CarDetailModel car)
        {
            Car = car;
        }
    }
}
