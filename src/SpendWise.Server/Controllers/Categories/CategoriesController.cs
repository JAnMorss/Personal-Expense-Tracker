using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendWise.Application.Categories.Queries.GetAllCategories;
using SpendWise.Application.Categories.Response;
using SpendWise.Server.Abstractions;
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
}
