using AutoMapper;
using AutoMapper.EquivalencyExpression;
using CarPool.BL;
using CarPool.BL.Facades;
using CarPool.DAL;
using CarPool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace CarPool.BL;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        services.AddSingleton<CarFacade>();
        services.AddSingleton<RideFacade>();
        services.AddSingleton<UserFacade>();

        services.AddAutoMapper((serviceProvider, cfg) =>
        {
            cfg.AddCollectionMappers();

            var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<CarPoolDbContext>>();
            using var dbContext = dbContextFactory.CreateDbContext();
            cfg.UseEntityFrameworkCoreModel<CarPoolDbContext>(dbContext.Model);
        }, typeof(BusinessLogic).Assembly);
        return services;
    }
}