using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarPool.BL;
using CarPool.BL.Facades;
using CarPool.DAL.Factories;
using CarPool.DAL;
using Microsoft.EntityFrameworkCore;
using CarPool.App.Settings;
using Microsoft.Extensions.Options;
using CarPool.App.Views;
using CarPool.App.ViewModels;
using CarPool.App.Services;

namespace CarPool.App
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {

            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
                .Build();
        }
        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.AddJsonFile("appsettings.json", false, false);
        }

        private static void ConfigureServices(IConfiguration configuration,
            IServiceCollection services)
        {
            services.AddBLServices();

            services.Configure<DALSettings>(configuration.GetSection("CookBook:DAL"));

            services.AddSingleton<IDbContextFactory<CarPoolDbContext>>(provider =>
            {
                var dalSettings = provider.GetRequiredService<IOptions<DALSettings>>().Value;
                return new SqlLiteDbContextFactory("Data Source={database.db}", dalSettings.SkipMigrationAndSeedDemoData);
            });

            services.AddSingleton<MainWindow>();
            services.AddSingleton<Navbar>();
            services.AddSingleton<UserListModel>();

            services.AddSingleton<IMessenger, Messenger>();
            services.AddSingleton<PassengerView>();
            services.AddSingleton<PassengerViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<DriverViewModel>();
            services.AddSingleton<DriverView>();
            services.AddSingleton<NumberOfRidesFacade>();
            services.AddSingleton<NavbarViewModel>();
            services.AddSingleton<LoginView>();
            services.AddSingleton<ICommandFactory, CommandFactory>();
            services.AddSingleton<LoginViewModel>();



            /* services.AddSingleton<IIngredientListViewModel, IngredientListViewModel>();
             services.AddFactory<IIngredientDetailViewModel, IngredientDetailViewModel>();

             services.AddFactory<IRecipeDetailViewModel, RecipeDetailViewModel>();
             services.AddFactory<IIngredientAmountDetailViewModel, IngredientAmountDetailViewModel>();
            */
            //Add carpool factiories and model when needed
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
       
            await _host.StartAsync();

            var dbContextFactory = _host.Services.GetRequiredService<IDbContextFactory<CarPoolDbContext>>();

            var dalSettings = _host.Services.GetRequiredService<IOptions<DALSettings>>().Value;

            await using (var dbx = await dbContextFactory.CreateDbContextAsync())
            {
                    await dbx.Database.EnsureDeletedAsync();
                    await dbx.Database.EnsureCreatedAsync();
                
            }

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }

}

