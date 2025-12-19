namespace SpendWise.SharedKernel.Repositories;

public interface IUserOwned
{
    Guid CreatedByUserId { get; }
}
