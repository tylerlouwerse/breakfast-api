using ErrorOr;
using BreakfastApi.Contracts.Breakfast;
using Microsoft.AspNetCore.Mvc;
using BreakfastApi.Models;
using BreakfastApi.Services.Breakfasts;
using BreakfastApi.ServiceErrors;

namespace BreakfastApi.Contollers;

public class BreakfastController : ApiController
{
  private readonly IBreakfastService _breakfastService;
  public BreakfastController(IBreakfastService breakfastService)
  {
    _breakfastService = breakfastService;
  }

  [HttpPost]
  public IActionResult CreateBreakfast(CreateBreakfastRequest request)
  {
    ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(request);

    if(requestToBreakfastResult.IsError) {
      return Problem(requestToBreakfastResult.Errors);
    }

    var breakfast = requestToBreakfastResult.Value;
    ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

    return createBreakfastResult.Match(
      created => CreateAsGetBreakfast(breakfast),
      errors => Problem(errors)
    );
  }

  [HttpGet("{id:guid}")]
  public IActionResult GetBreakfast(Guid id)
  {
    ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

    return getBreakfastResult.Match(
      breakfast => Ok(MapBreakfastResponse(breakfast)),
      errors => Problem(errors)
    );
  }

  [HttpPut("{id:guid}")]
  public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
  {
    ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(id, request);

    if(requestToBreakfastResult.IsError) {
      return Problem(requestToBreakfastResult.Errors);
    }

    var breakfast = requestToBreakfastResult.Value;
    ErrorOr<UpsertedBreakfast> breakfastUpsertedResult = _breakfastService.UpsertBreakfast(breakfast);

    breakfastUpsertedResult.Match(
      upserted => upserted.IsNewlyCreated ? CreateAsGetBreakfast(breakfast) : NoContent(),
      errors => Problem(errors)
    );

    return NoContent();
  }

  [HttpDelete("{id:guid}")]
  public IActionResult DeleteBreakfast(Guid id)
  {
    ErrorOr<Deleted> breakfastDeletedResponse = _breakfastService.DeleteBreakfast(id);

    return breakfastDeletedResponse.Match(
      deleted => NoContent(),
      errors => Problem(errors)
    );
  }

  private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
  {
    return new BreakfastResponse(
      breakfast.Id,
      breakfast.Name,
      breakfast.Description,
      breakfast.StartDateTime,
      breakfast.EndDateTime,
      breakfast.LastModifiedDateTime,
      breakfast.Savory,
      breakfast.Sweet
    );
  }

  private CreatedAtActionResult CreateAsGetBreakfast(Breakfast breakfast)
  {
      return CreatedAtAction(
        actionName: nameof(GetBreakfast),
        routeValues: new { id = breakfast.Id },
        value: MapBreakfastResponse(breakfast)
      );
  }
}