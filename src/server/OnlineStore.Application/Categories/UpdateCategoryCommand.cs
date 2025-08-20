using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Categories;

public sealed record UpdateCategoryCommand(Guid Id, string Name, Guid? ParentCategoryId = null) : IRequest<CategoryDto>;

public sealed class UpdateCategoryCommandHandler(IApplicationDbContext dbContext)
        : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var category = await dbContext.Categories
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync(ct);

        if (category is null)
            throw new NotFoundException($"Category with id '{request.Id}' not found");

        category.Name = request.Name;
        category.ParentCategoryId = request.ParentCategoryId;

        await dbContext.SaveChangesAsync(ct);

        return new CategoryDto(category.Id, category.Name, category.ParentCategoryId, new List<CategoryDto>());
    }
}
