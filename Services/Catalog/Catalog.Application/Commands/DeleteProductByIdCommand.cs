using MediatR;

namespace Catalog.Application.Commands;

public record DeleteProductByIdCommand(string productId):IRequest<bool>
{
    
}