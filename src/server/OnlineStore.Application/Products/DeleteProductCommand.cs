using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.Products;

public sealed record DeleteProductCommand(Guid Id) : IRequest;

public sealed class DeleteProductCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
            throw new NotFoundException($"Product with id '{request.Id}' not found");

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(ct);
    }
}
