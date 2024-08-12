using BuildingBlocks.CQRS;
using MediatR;
using System.Windows.Input;

namespace Catalog.Api.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Categories, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
