using System;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<CarPoolDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<CarPoolDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}
