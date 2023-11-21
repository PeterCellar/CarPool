using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL.Facades
{
    public class UserFacade : CRUDFacade<UserEntity, UserListModel, UserDetailModel>
    {
        public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
        }
    }
}
