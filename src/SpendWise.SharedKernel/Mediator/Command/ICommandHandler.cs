using MediatR;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.SharedKernel.Mediator.Command;

public interface ICommandHandler<TCommand> 
    : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{

}

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{ 

}
