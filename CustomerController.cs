
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CustomerController : Controller
{
  [HttpGet("{customerId}")]
  public async Task<IActionResult> GetCustomer(string customerId, CancellationToken cancellationToken)
  {
    try
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      // to simulate a long-running operation
      var randomDelay = new Random().Next(3000, 5000);
      Console.WriteLine($"Delaying for {randomDelay}ms");

      await Task.Delay(randomDelay, cancellationToken);

      var customer = new Customer
      {
        Id = customerId,
        Name = "John Doe",
      };


      var a = await ApiCall(cancellationToken);
      cancellationToken.ThrowIfCancellationRequested();
      Console.WriteLine($"Api call {a}");
      Console.WriteLine($"Api call took {stopwatch.ElapsedMilliseconds}ms");

      return Ok(customer);
    }
    catch (OperationCanceledException)
    {
      if (cancellationToken.IsCancellationRequested)
      {
        Console.WriteLine("Request was cancelled");
      }
      return BadRequest("Request was cancelled due to timeout or cancellation token");
    }
    catch (Exception ex)
    {
      return StatusCode(500, "Internal server error: " + ex.Message);
    }
  }

  private static async Task<string> ApiCall(CancellationToken cancellationToken)
  {
    var randomDelay = new Random().Next(3000, 9000);
    await Task.Delay(randomDelay, cancellationToken);

    return "completed";
  }
}