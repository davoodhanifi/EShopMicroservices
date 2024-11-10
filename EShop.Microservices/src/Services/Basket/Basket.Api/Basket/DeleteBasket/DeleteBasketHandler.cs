
using Basket.Api.Data;

namespace Basket.Api.Basket.DeleteBasket;

public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x=> x.Username).NotEmpty().WithMessage("Username is required.");
    }
}

public class DeleteBasketHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    private readonly IBasketRepository _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));

    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await _basketRepository.Delete(command.Username, cancellationToken);
        return new DeleteBasketResult(result);
    }
}
