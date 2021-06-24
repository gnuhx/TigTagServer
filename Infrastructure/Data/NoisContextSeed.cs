using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class NoisContextSeed
    {
        public static async Task SeedAsync(NoisContext context,
           ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                // TODO: Only run this if using a real database
                // context.Database.Migrate();

                //if (!context.Printers.Any())
                //{
                //    context.Printers.AddRange(
                //        GetPreconfiguredPrinters());

                //    await context.SaveChangesAsync();
                //}
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<NoisContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, retryForAvailability);
                }
            }
        }


    }
}
