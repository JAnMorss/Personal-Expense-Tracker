using MediatR;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.SharedKernel.Mediator.Query;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
