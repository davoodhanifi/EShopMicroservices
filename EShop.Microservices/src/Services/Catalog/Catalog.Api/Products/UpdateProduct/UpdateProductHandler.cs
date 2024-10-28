
namespace Catalog.Api.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool Result);

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required!");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required!")
                            .Length(2, 150).WithMessage("Name must have between 2 and 150 characters!");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories is required!");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required!");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required!");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero!");
    }
}

public class UpdateProductHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        product.Name = command.Name;
        product.Categories = command.Categories;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        _session.Update(product);
        await _session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
