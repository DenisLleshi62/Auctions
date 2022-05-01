using AuctionsDenis.BackgroundService;
using System;
using System.Threading.Tasks;
using AuctionsDenis.BackgroundService;
using AuctionsDenis.Service.ProductService;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionsDenis.ScheduleTask
{
    public class ExpiredProductSchedule : ScheduledProcessor
    {

        public ExpiredProductSchedule(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
                
        }

        protected override string Schedule => "*/1 * * * *"; // every 1 min 

        public override async Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            IProductService productService = scopeServiceProvider.GetRequiredService<IProductService>();
            productService.ExpiredProduct();
            // return Task.CompletedTask;


            await Task.Run(() => {
                return Task.CompletedTask;
            });
        }
    }
}
