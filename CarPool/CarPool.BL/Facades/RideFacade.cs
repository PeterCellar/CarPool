using AutoMapper;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace CarPool.BL.Facades
{
    public class RideFacade : CRUDFacade<RideEntity, RideListModel, RideDetailModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RideListModel>> GetByTime(DateTime time)
        {
            if (time == null)
                return Enumerable.Empty<RideListModel>();
            await using var uow = _unitOfWorkFactory.Create();
            var query = uow
                .GetRepository<RideEntity>()
                .Get()
                .Where(e => e.StartTime > time.AddHours(-2) && e.StartTime < time.AddHours(2));
            return await _mapper.ProjectTo<RideListModel>(query).ToArrayAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<RideListModel>> GetByPlace(string start, string end)
        {
            await using var uow = _unitOfWorkFactory.Create();
            var query = uow
                .GetRepository<RideEntity>()
                .Get()
                .Where(e => e.StartLocation == start && e.EndLocation == end);
            return await _mapper.ProjectTo<RideListModel>(query).ToArrayAsync().ConfigureAwait(false);
        }
        public async Task DeleteFromPassengersAsync(Guid Ride_id, Guid Passenger_id)
        {
            await using var uow = _unitOfWorkFactory.Create();
            var query = uow
                .GetRepository<NumberOfRidesEntity>()
                .Get()
                .Where(e => e.UserId == Passenger_id && e.RideId == Ride_id);

            var help = await _mapper.ProjectTo<NumberOfRidesDetailModel>(query).SingleOrDefaultAsync().ConfigureAwait(false);
            uow.GetRepository<NumberOfRidesEntity>().Delete(help.Id);
            await uow.CommitAsync().ConfigureAwait(false);
        }

        public async Task<RideListModel?> GetAsyncRideDetail(Guid id)
        {
            await using var uow = _unitOfWorkFactory.Create();
            var query = uow
                .GetRepository<RideEntity>()
                .Get()
                .Where(e => e.Id == id);
            return await _mapper.ProjectTo<RideListModel>(query).SingleOrDefaultAsync().ConfigureAwait(false);
        }

    }
}
