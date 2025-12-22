using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendWise.Application.Expenses.Commands.AddExpenseToCategory;
using SpendWise.Application.Expenses.Commands.DeleteExpense;
using SpendWise.Application.Expenses.Commands.UpdateExpense;
using SpendWise.Application.Expenses.Queries.GetAllExpenses;
using SpendWise.Application.Expenses.Queries.GetExpenseById;
using SpendWise.Application.Expenses.Response;
using SpendWise.Server.Abstractions;
using SpendWise.Server.Controllers.Expenses.Requests;
using SpendWise.SharedKernel.Helpers;
using SpendWise.SharedKernel.PageSize;

namespace SpendWise.Server.Controllers.Expenses;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/expense")]
[Authorize]
public class ExpenseController : ApiController
{
    public ExpenseController(ISender sender) 
        : base(sender)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses(
        [FromQuery] QueryObject queryObject, 
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var query = new GetAllExpensesQuery(queryObject, userId.Value);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<PaginatedResult<ExpenseResponse>>(
                result.Value, 
                "Expenses retrieved successfully."))
            : HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetExpenseById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var query = new GetExpenseByIdQuery(id, userId.Value);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<ExpenseResponse>(
                result.Value, 
                "Expense retrieved successfully."))
            : HandleFailure(result);
    }

    [HttpPost("{categoryId:guid}")]
    public async Task<IActionResult> AddExpenseToCategory(
        [FromRoute] Guid categoryId,
        [FromBody] ExpenseRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var command = new AddExpenseToCategoryCommand(
            categoryId, 
            userId.Value, 
            request.Amount, 
            request.Date,
            request.Description);

        var result = await _sender.Send(command, cancellationToken);
        
        return result.IsSuccess
            ? Ok(new ApiResponse<ExpenseResponse>(
                result.Value!, 
                "Expense added successfully."))
            : HandleFailure(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateExpense(
    [FromRoute] Guid id,
    [FromBody] ExpenseRequest request,
    CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var command = new UpdateExpenseCommand(
            id,              
            userId.Value,    
            request.Amount,
            request.Date,
            request.Description
        );

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<ExpenseResponse>(
                result.Value,
                "Expense updated successfully."))
            : HandleFailure(result);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExpense(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var command = new DeleteExpenseCommand(id, userId.Value);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse("Expense deleted successfully."))
            : HandleFailure(result);
    }
}
