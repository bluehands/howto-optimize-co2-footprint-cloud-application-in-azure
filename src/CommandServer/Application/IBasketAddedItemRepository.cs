using FunicularSwitch;

namespace Application;

public interface IBasketAddedItemRepository
{
    public Task<Result<Unit>> Add(BasketAddedItem basketAddedItem);

    public Task<Result<IReadOnlyCollection<BasketAddedItem>>> GetByBasketId(Guid basketId);
}