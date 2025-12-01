using FluentAssertions;
using SpendWise.Domain.UnitTests.Infrastructure;
using SpendWise.Domain.Users.Entities;
using SpendWise.Domain.Users.Events;

namespace SpendWise.Domain.UnitTests.Users;

public class UserTests : BaseTest
{
    private static User CreateValidUser()
    {
        return User.Create(
            UserData.Id,
            UserData.FirstName.Value,
            UserData.LastName.Value,
            UserData.Age.Value,
            UserData.Email.Value,
            UserData.Avatar.Value,
            UserData.PasswordHash.Value).Value;
    }

    [Fact]
    public void Create_Should_ShouldReturnSuccess_And_Raise_Events()
    {
        // Act
        var result = User.Create(
            UserData.Id,
            UserData.FirstName.Value,
            UserData.LastName.Value,
            UserData.Age.Value,
            UserData.Email.Value,
            UserData.Avatar.Value,
            UserData.PasswordHash.Value);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var user = result.Value;

        user.FirstName.Value.Should().Be(UserData.FirstName.Value);
        user.LastName.Value.Should().Be(UserData.LastName.Value);
        user.Age.Value.Should().Be(UserData.Age.Value);
        user.Email.Value.Should().Be(UserData.Email.Value);
        user.Avatar!.Value.Should().Be(UserData.Avatar.Value);
        user.PasswordHash.Value.Should().Be(UserData.PasswordHash.Value);
        user.Id.Should().NotBe(Guid.Empty);

        var domainEvents = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);
        domainEvents.Id.Should().Be(user.Id);
    }

    [Fact]
    public void UpdateDetails_Should_Update_User_Values_And_Raise_Events()
    {
        // Arrange
        var user = CreateValidUser();
        var newFirstName = "Janmors";
        var newLastName = "Morales";
        var newAge = 30;
        var newEmail = "morales@gmail.com";

        // Act
        var result = user.UpdateDetails(
            newFirstName,
            newLastName,
            newAge,
            newEmail);

        // Assert
        result.IsSuccess.Should().BeTrue();

        user.FirstName.Value.Should().Be(newFirstName);
        user.LastName.Value.Should().Be(newLastName);
        user.Age.Value.Should().Be(newAge);
        user.Email.Value.Should().Be(newEmail);
        user.UpdateAt.Should().NotBeNull();

        var domainEvents = AssertDomainEventWasPublished<UserUpdatedDomainEvent>(user);
        domainEvents.Id.Should().Be(user.Id);
    }

    [Fact]
    public void UpdateAvatar_Should_Update_Avatar_And_Raise_Events()
    {
        // Arrange
        var user = CreateValidUser();
        var newAvatarUrl = "https://example.com/newavatar.png";

        // Act
        var result = user.UpdateAvatar(newAvatarUrl);

        // Assert
        result.IsSuccess.Should().BeTrue();

        user.Avatar!.Value.Should().Be(newAvatarUrl);
        user.UpdateAt.Should().NotBeNull();

        var domainEvents = AssertDomainEventWasPublished<UserAvatarUpdatedDomainEvent>(user);
        domainEvents.Id.Should().Be(user.Id);

    }
}
