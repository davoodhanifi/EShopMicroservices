namespace Catalog.Api.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Categories, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required!")
                            .Length(2,150).WithMessage("Name must have between 2 and 150 characters!");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories is required!");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required!");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required!");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero!");
    }
}

public class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IDocumentSession _session = session;
    private readonly ILogger<CreateProductCommandHandler> _logger = logger;

    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"CreateProductCommandHandler.Handle called with command: {command}");
        var product = new Product
        {
            Name = command.Name,
            Categories = command.Categories,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken);
        
        return new CreateProductResult(product.Id);
    }
}
