using FunicularSwitch;

namespace Application;

public class StoreBasket
{
    private readonly IBasketAddedItemRepository basketAddedItemRepository;

    public StoreBasket(IBasketAddedItemRepository basketAddedItemRepository)
    {
        this.basketAddedItemRepository = basketAddedItemRepository;
    }

    public Task<Result<Unit>> Event(BasketAddedItem basketAddedItem)
    {
        return basketAddedItemRepository.Add(basketAddedItem);
    }
}