using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Repositories;

public class UserRepositoryIntegrationTests() : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();

    [Fact]
    public async Task TestCreateUser()
    {
        var expectedUser = new User(Guid.NewGuid(), Guid.Empty, "", "", "", false, false);

        await _dbFixture.UserRepository.AddUser(expectedUser);
        var actualUser = await _dbFixture.UserRepository.GetUserById(expectedUser.Id);

        Assert.Equal(expectedUser, actualUser);
    }

    [Fact]
    public async Task TestGetAllUsers()
    {
        var expectedUsers = InMemoryDbFixture.CreateMockUsers();
        await _dbFixture.InsertUsers(expectedUsers);

        var actualUsers = await _dbFixture.UserRepository.GetAllUsers();

        Assert.Equal(expectedUsers, actualUsers);
    }

    [Fact]
    public async Task TestGetUserById()
    {
        const int ind = 0;
        var expectedUsers = InMemoryDbFixture.CreateMockUsers();
        await _dbFixture.InsertUsers(expectedUsers);

        var actualUser = await _dbFixture.UserRepository.GetUserById(expectedUsers[ind].Id);

        Assert.Equal(expectedUsers[ind], actualUser);
    }

    [Fact]
    public async Task TestUpdateUser()
    {
        var users = InMemoryDbFixture.CreateMockUsers();
        await _dbFixture.InsertUsers(users);

        var expectedUser = new User(users.First())
        {
            IsAdmin = true
        };

        var actualUser = await _dbFixture.UserRepository.UpdateUser(expectedUser);

        Assert.Equal(expectedUser, actualUser);
    }

    public void Dispose() => _dbFixture.Dispose();
}