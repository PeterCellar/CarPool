using AutoMapper;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace CarPool.BL.Facades
{
    public class NumberOfRidesFacade : CRUDFacade<NumberOfRidesEntity,UserDetailModel, NumberOfRidesDetailModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        public NumberOfRidesFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mapper = mapper;
        }
        async public Task<Boolean> hasUserJointRide(Guid rideId, Guid userId)
        {
            await using var uow = _unitOfWorkFactory.Create();
            var query = uow
                .GetRepository<NumberOfRidesEntity>()
                .Get()
                .Where(e => e.UserId == userId && e.RideId == rideId );
            
            var collection = await _mapper.ProjectTo<NumberOfRidesDetailModel>(query).SingleOrDefaultAsync().ConfigureAwait(false);
            if(collection == null)
                return false;
            else
                return true;
        }
        
    }
}
