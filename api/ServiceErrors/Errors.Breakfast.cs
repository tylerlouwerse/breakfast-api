using ErrorOr;

namespace BreakfastApi.ServiceErrors;

public static class Errors
{
  public static class Breakfast
  {
    public static Error InvalidName => Error.Validation(
      code: "Breakfast.InvalidName",
      description: $"Breakfast name must be between {Models.Breakfast.MinNameLength}" +
      $" and {Models.Breakfast.MaxNameLength} characters long"
    );
    public static Error InvalidDescription => Error.Validation(
      code: "Breakfast.InvalidDescription",
      description: "Breakfast description is invalid"
    );
    public static Error NotFound => Error.NotFound(
      code: "Breakfast.NotFound",
      description: "The breakfast you requested could not be found."
    );
  }
}