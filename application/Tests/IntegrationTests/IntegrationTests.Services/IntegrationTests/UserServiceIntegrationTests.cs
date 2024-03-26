using MewingPad.Services.UserService;
using IntegrationTests.Services.DbFixtures;

namespace IntegrationTests.Services.IntegratonTests;

public class UserRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture;
    private readonly IUserService _userService;

    public UserRepositoryIntegrationTests()
    {
        _dbFixture = new();
        _userService = new UserService(_dbFixture.UserRepository);
    }

    [Fact]
    public async Task TestGetAllUsers()
    {
        var expectedUsers = InMemoryDbFixture.CreateMockUsers();
        await _dbFixture.InsertUsers(expectedUsers);

        var actualUsers = await _userService.GetAllUsers();

        Assert.Equal(expectedUsers, actualUsers);
    }

    [Fact]
    public async Task TestGetAllUsersEmpty()
    {
        var users = await _userService.GetAllUsers();

        Assert.Empty(users);
    }

    [Fact]
    public async Task TestGetUserById()
    {
        const int ind = 0;
        var expectedUsers = InMemoryDbFixture.CreateMockUsers();
        await _dbFixture.InsertUsers(expectedUsers);

        var actualUser = await _userService.GetUserById(expectedUsers[ind].Id);

        Assert.Equal(expectedUsers[ind], actualUser);
    }

    [Fact]
    public async Task TestGetUserNotFound()
    {
        Task Action() => _userService.GetUserById(Guid.NewGuid());

        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    [Fact]
    public async Task TestChangeUserPermissions()
    {
        const int ind = 0;
        var expectedUsers = InMemoryDbFixture.CreateMockUsers();
        expectedUsers[ind].IsAdmin = false;
        await _dbFixture.InsertUsers(expectedUsers);

        var actualUser = await _userService.ChangeUserPermissions(expectedUsers[ind].Id, true);

        Assert.Equal(expectedUsers[ind].Id, actualUser.Id);
        Assert.Equal(expectedUsers[ind].FavouritesId, actualUser.FavouritesId);
        Assert.Equal(expectedUsers[ind].Username, actualUser.Username);
        Assert.Equal(expectedUsers[ind].Email, actualUser.Email);
        Assert.Equal(expectedUsers[ind].PasswordHashed, actualUser.PasswordHashed);
        Assert.True(actualUser.IsAdmin);
    }

    [Fact]
    public async Task TestChangeUserNonexistentPermissions()
    {
        Task Action() => _userService.ChangeUserPermissions(Guid.Empty, true);

        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    public void Dispose() => _dbFixture.Dispose();
}