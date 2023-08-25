using ErrorOr;
using BreakfastApi.Models;


namespace BreakfastApi.Services.Breakfasts;

public interface IBreakfastService
{
  ErrorOr<Created> CreateBreakfast(Breakfast breakfast);
  ErrorOr<Breakfast> GetBreakfast(Guid id);
  ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast);
  ErrorOr<Deleted> DeleteBreakfast(Guid id);
}