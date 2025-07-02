using System.Diagnostics;
using MovieApi.Data;

namespace MovieApi.Extensions;

public static class MoviesApplicationExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<MovieContext>();

            //await context.Database.EnsureDeletedAsync();
            //await context.Database.MigrateAsync();

            try
            {
                await SeedData.InitAsync(context);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }

        }


    }

}
