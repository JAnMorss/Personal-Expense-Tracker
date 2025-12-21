using SpendWise.Application.Users.Response;
using SpendWise.SharedKernel.Mediator.Query;

namespace SpendWise.Application.Users.Queries.GetMyProfile;

public sealed record GetMyProfileQuery(Guid UserId) : IQuery<UserResponse>;