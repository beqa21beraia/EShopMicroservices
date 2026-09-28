namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);

    internal class GetProductByIdQueryHandler
        (IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) 
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductByIdQuery for id {ProductId}.", query.id);
            var product = await session.LoadAsync<Product>(query.id, cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product with id {ProductId} not found.", query.id);
                throw new ProductNotFoundException(); 
            }

            return new GetProductByIdResult(product);
        }
    }
}
