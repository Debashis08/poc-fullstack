using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace poc.azure.function.app.Functions
{
    public class I0002_1_CreateUser
    {
        private readonly ILogger<I0002_1_CreateUser> _logger;
        private readonly AppDbContext _context;

        // Inject the AppDbContext via the constructor
        public I0002_1_CreateUser(ILogger<I0002_1_CreateUser> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("I0002_1_CreateUser")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("Processing request to create a new user.");

                // 1. Read the JSON payload from the request body
                var requestBody = await req.ReadAsStringAsync().ConfigureAwait(false);
                var userToCreate = JsonSerializer.Deserialize<User>(requestBody!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (userToCreate == null || string.IsNullOrEmpty(userToCreate.UserName))
                {
                    var badRequestResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // 2. Add the user to Entity Framework tracking
                _context.Users.Add(userToCreate!);

                // 3. Save changes to the physical local SQL Server database
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully inserted user: {userToCreate?.UserName}");

                // 4. Return a success response
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
            catch(Exception ex)
            {
                var message = ex.Message.ToString();
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}