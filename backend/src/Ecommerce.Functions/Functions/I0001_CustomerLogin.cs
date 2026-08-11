using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ecommerce.Functions.Functions;

public class I0001_CustomerLogin
{
    private readonly ILogger<I0001_CustomerLogin> _logger;
    private readonly IValidator<Customer> _validator;

    public I0001_CustomerLogin(ILogger<I0001_CustomerLogin> logger, IValidator<Customer> validator)
    {
        _logger = logger;
        _validator = validator;
    }

    [Function("CustomerLogin")]
    public async Task<IActionResult> ProcessRequest([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - started");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var customer = JsonSerializer.Deserialize<Customer>(requestBody);

        var validationResult = await _validator.ValidateAsync(customer!, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult($"Invalid Request Body - {string.Join('|', validationResult.Errors)}");
        }

        _logger.LogTrace($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - finished");

        return new OkObjectResult("ok");
    }
}