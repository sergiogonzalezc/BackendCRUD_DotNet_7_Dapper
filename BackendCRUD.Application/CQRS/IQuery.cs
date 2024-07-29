using MediatR;

namespace BackendCRUD.Application.CQRS;
public interface IQuery<out TResponse> : IRequest<TResponse>  
    where TResponse : notnull
{
}
