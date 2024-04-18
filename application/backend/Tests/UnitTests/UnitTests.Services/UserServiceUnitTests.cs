using MewingPad.Services.UserService;
using MewingPad.Common.Exceptions;

namespace UnitTests.Services;

public class UserServiceUnitTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public UserServiceUnitTests()
    {
        _userService = new UserService(_mockUserRepository.Object);
    }

    [Fact]
    public async Task TestGetAllUsers()
    {
        var users = CreateMockUsers();
        _mockUserRepository.Setup(s => s.GetAllUsers()).ReturnsAsync(users);

        var receivedUsers = await _userService.GetAllUsers();

        Assert.Equal(users, receivedUsers);
    }

    [Fact]
    public async Task TestGetAllUsersEmpty()
    {
        var users = new List<User>();
        _mockUserRepository.Setup(s => s.GetAllUsers()).ReturnsAsync(users);

        var receivedUsers = await _userService.GetAllUsers();

        Assert.Equal(users, receivedUsers);
    }

    [Fact]
    public async Task TestGetExistentUserById()
    {
        var users = CreateMockUsers();
        Guid userId = users[0].Id;

        _mockUserRepository.Setup(s => s.GetUserById(userId))
                            .ReturnsAsync(users.Find(e => e.Id == userId)!);

        var expectedUser = users[0];
        var receivedUser = await _userService.GetUserById(userId);

        Assert.Equal(expectedUser, receivedUser);
    }

    [Fact]
    public async Task TestGetNonexistentUserById()
    {
        var userId = Guid.NewGuid();
        async Task<User> Action() => await _userService.GetUserById(userId);
        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    private static List<User> CreateMockUsers()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), "user1", "u1@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y", true),
            new(Guid.NewGuid(), Guid.NewGuid(), "user2", "u2@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y", false),
            new(Guid.NewGuid(), Guid.NewGuid(), "user3", "u3@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y", false),
            new(Guid.NewGuid(), Guid.NewGuid(), "user4", "u4@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y", true)
        ];
    }
}