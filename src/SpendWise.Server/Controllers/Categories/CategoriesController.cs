using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendWise.Application.Categories.Commands.CreateCategory;
using SpendWise.Application.Categories.Commands.DeleteCategory;
using SpendWise.Application.Categories.Commands.UpdateCategory;
using SpendWise.Application.Categories.Queries.GetAllCategories;
using SpendWise.Application.Categories.Queries.GetCategoryById;
using SpendWise.Application.Categories.Response;
using SpendWise.Server.Abstractions;
using SpendWise.Server.Controllers.Categories.Requests;
using SpendWise.SharedKernel.Helpers;
using SpendWise.SharedKernel.PageSize;

namespace SpendWise.Server.Controllers.Categories;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/category")]
[Authorize]
public class CategoriesController : ApiController
{
    public CategoriesController(ISender sender) 
        : base(sender)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories(
        [FromQuery] QueryObject queryObject,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var query = new GetAllCategoriesQuery(queryObject, userId.Value);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<PaginatedResult<CategoryResponse>>(
                result.Value,
                "Categories retrieved successfully."))
            : HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var query = new GetCategoryByIdQuery(id, userId.Value);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<CategoryResponse>(
                result.Value,
                "Category retrieved successfully."))
            : HandleFailure(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var command = new CreateCategoryCommand(
            request.CategoryName,
            request.Icon,
            userId.Value);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<CategoryResponse>(
                    result.Value,
                    "Category created successfully."))
            : HandleFailure(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(
        [FromRoute] Guid id,
        [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var command = new UpdateCategoryCommand(
            id,
            userId.Value,
            request.CategoryName,
            request.Icon );

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<CategoryResponse>(
                    result.Value,
                    "Category updated successfully."))
            : HandleFailure(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var command = new DeleteCategoryCommand(id, userId.Value);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse("Category deleted successfully."))
            : HandleFailure(result);
    }
}
