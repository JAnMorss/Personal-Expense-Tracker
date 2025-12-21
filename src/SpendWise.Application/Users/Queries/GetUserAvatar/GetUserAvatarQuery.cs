using SpendWise.Application.Users.Response;
using SpendWise.SharedKernel.Mediator.Query;

namespace SpendWise.Application.Users.Queries.GetUserAvatar;

public sealed record GetUserAvatarQuery(Guid UserId) : IQuery<UserAvatarResponse>;