using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL.Facades
{
    public class CarFacade : CRUDFacade<CarEntity, CarListModel, CarDetailModel>
    {
        public CarFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {

        }
    }
}

