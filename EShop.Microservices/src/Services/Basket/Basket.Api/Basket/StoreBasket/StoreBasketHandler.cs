using Basket.Api.Data;
using Discount.Grpc;

namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string Username);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
        RuleFor(x => x.Cart.Username).NotNull().WithMessage("Username is required.");
    }
}

public class StoreBasketHandler(IBasketRepository basketRepository,
                                DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    private readonly IBasketRepository _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProto = discountProto ?? throw new ArgumentNullException(nameof(discountProto));

    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Cart, cancellationToken);
        await _basketRepository.Store(command.Cart, cancellationToken);
       
        return new StoreBasketResult(command.Cart.Username);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await _discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName },
                                                                 cancellationToken: cancellationToken);

            item.Price -= coupon.Amount;
        }
    }
}
