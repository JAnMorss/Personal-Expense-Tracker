using MediatR;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.SharedKernel.Mediator.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
