using MediatR;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.Categories;

public record CreateCategoryCommand() : IRequest;

public sealed class CreateCategoryCommandHandler(IApplicationDbContext dbContext)
	: IRequestHandler<CreateCategoryCommand>
{
	public Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}