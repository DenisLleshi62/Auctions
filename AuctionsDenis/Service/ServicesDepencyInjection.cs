using AuctionsDenis.Service.UserService;
using AuctionsProject.Data;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Authorization;
using Mapper = MapsterMapper.Mapper;

namespace AuctionsDenis.Service;

 public static class ServicesDependencyInjection
    {
        /// <summary>
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService, UserService.UserService>();
            serviceCollection.AddTransient<IJwtUtils, JwtUtils>();
            serviceCollection.AddTransient< MapsterMapper.IMapper, Mapper>();
        
        }
    }