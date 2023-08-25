using Microsoft.AspNetCore.Mvc;

namespace BreakfastApi.Controllers;

public class ErrorsController : ControllerBase
{
  [Route("/error")]
  public IActionResult Error()
  {
    return Problem();
  }
}