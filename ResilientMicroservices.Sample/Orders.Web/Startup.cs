using AutoMapper;
using Common.Contracts.Events;
using Common.Domain;
using Common.Infrastructure.Kafka;
using Common.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ResilientMicroservices.Sample.Orders.Data;
using ResilientMicroservices.Sample.Orders.Domain;
using ResilientMicroservices.Sample.Orders.Domain.Commands;
using ResilientMicroservices.Sample.Orders.Domain.EventHandlers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ResilientMicroservices.Sample.Orders.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var log = new Log();
            services.AddSingleton<ILog>(log);
            ConfigureKafka(services);
            ConfigureCommandHandlers(services);
            ConfigureAutoMapper(services);
            ConfigureRepositories(services);
            ConfigureEventHandlers(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private static void ConfigureEventHandlers(IServiceCollection services)
        {
            services.AddTransient<OrderValidatedEventHandler>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddSingleton(new MongoDbSettings
            {
                ServerConnection = Configuration["ConnectionStrings:MongoDb:ServerConnection"],
                Database = Configuration["ConnectionStrings:MongoDb:DatabaseName"]
            });
            services.AddTransient<IOrderRepository, OrderRepository>();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CommandToContractMapperProfile());
                cfg.AddProfile(new ContractToCommandMapperProfile());
            });
            mapperConfig.AssertConfigurationIsValid();
            services.AddSingleton(provider => mapperConfig.CreateMapper());
        }

        private static void ConfigureCommandHandlers(IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<CreateOrderCommand>, CreateOrderCommandHandler>();
        }

        private void ConfigureKafka(IServiceCollection services)
        {
            services.Configure<KafkaEventConsumerConfiguration>(Configuration.GetSection("KafkaConsumer"));
            services.PostConfigure<KafkaEventConsumerConfiguration>(options =>
            {
                options.RegisterConsumer<OrderValidatedEvent, OrderValidatedEventHandler>();
            });
            services.AddSingleton<IHostedService, KafkaConsumer>();
            services.Configure<KafkaEventProducerConfiguration>(Configuration.GetSection("KafkaProducer"));
            services.PostConfigure<KafkaEventProducerConfiguration>(options =>
            {
                options.SerializerSettings =
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            });
            services.AddTransient<IKakfaProducer, KafkaProducer>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
