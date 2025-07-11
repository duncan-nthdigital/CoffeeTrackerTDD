using System.Runtime.CompilerServices;

// Make the Program class accessible to tests
[assembly: InternalsVisibleTo("CoffeeTracker.Api.IntegrationTests")]
[assembly: InternalsVisibleTo("CoffeeTracker.Api.Tests")]
[assembly: InternalsVisibleTo("Microsoft.AspNetCore.Mvc.Testing")]
