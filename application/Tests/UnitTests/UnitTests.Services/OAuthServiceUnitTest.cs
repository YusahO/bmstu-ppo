using MewingPad.Common.Exceptions;
using MewingPad.Utils.PasswordHasher;
using MewingPad.Services.OAuthService;

namespace UnitTests.Services;

public class OAuthServiceUnitTests
{
    private readonly IOAuthService _oauthService;
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public OAuthServiceUnitTests()
    {
        _oauthService = new OAuthService(_mockUserRepository.Object);
    }

    [Fact]
    public async Task TestRegister()
    {
        var users = CreateMockUsers();

        var email = "email";
        var password = "password";

        var expectedUser = CreateMockUser(email, PasswordHasher.HashPassword(password));

        _mockUserRepository
            .Setup(s => s.GetUserByEmail(email))
            .ReturnsAsync(users.Find(u => u.Email == email)!);

        _mockUserRepository
           .Setup(s => s.AddUser(It.IsAny<User>()))
           .Callback((User u) => users.Add(u));

        await _oauthService.RegisterUser(expectedUser);
        var actualUser = users.Last();

        Assert.Equal(expectedUser.Id, actualUser.Id);
    }

    [Fact]
    public async Task TestRegisterExistent()
    {
        var email = "email";
        var password = "password";
        var expectedUser = CreateMockUser(email, PasswordHasher.HashPassword(password));

        expectedUser.Email = email;
        expectedUser.PasswordHashed = PasswordHasher.HashPassword(password);

        _mockUserRepository
            .Setup(s => s.GetUserByEmail(email))
            .ReturnsAsync(expectedUser);

        async Task Action() => await _oauthService.RegisterUser(expectedUser);

        await Assert.ThrowsAsync<UserRegisteredException>(Action);
    }

    [Fact]
    public async Task TestSignInUserAdmin()
    {
        var email = "email";
        var password = "password";

        var expectedUser = CreateMockUser();
        expectedUser.PasswordHashed = PasswordHasher.HashPassword(password);
        expectedUser.IsAdmin = true;

        _mockUserRepository
            .Setup(s => s.GetUserByEmail(email))
            .ReturnsAsync(expectedUser);

        var actualUser = await _oauthService.SignInUser(email, password);

        Assert.Equal(expectedUser.Id, actualUser.Id);
        Assert.Equal(expectedUser.Username, actualUser.Username);
        Assert.Equal(expectedUser.Email, actualUser.Email);
        Assert.True(actualUser.IsAdmin);
        Assert.True(actualUser.IsAuthorized);
    }

    [Fact]
    public async Task TestSignInUserNonadmin()
    {
        var email = "email";
        var password = "password";

        var expectedUser = CreateMockUser();
        expectedUser.PasswordHashed = PasswordHasher.HashPassword(password);

        _mockUserRepository
            .Setup(s => s.GetUserByEmail(email))
            .ReturnsAsync(expectedUser);

        var actualUser = await _oauthService.SignInUser(email, password);

        Assert.Equal(expectedUser.Id, actualUser.Id);
        Assert.Equal(expectedUser.Username, actualUser.Username);
        Assert.Equal(expectedUser.Email, actualUser.Email);
        Assert.False(actualUser.IsAdmin);
        Assert.True(actualUser.IsAuthorized);
    }

    [Fact]
    public async Task TestSignInNonexistent()
    {
        var email = "_email_";
        var password = "_password_";

        var expectedUser = CreateMockUser("email", PasswordHasher.HashPassword("password"));

        _mockUserRepository
            .Setup(s => s.GetUserByEmail(expectedUser.Email))
            .ReturnsAsync(expectedUser);

        async Task Action() => await _oauthService.SignInUser(email, password);

        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    [Fact]
    public async Task TestSignOut()
    {
        var users = CreateMockUsers();
        var expectedUser = users[2];
        expectedUser.IsAuthorized = true;

        _mockUserRepository
            .Setup(s => s.GetUserById(expectedUser.Id))
            .ReturnsAsync(expectedUser);

        await _oauthService.SignOutUser(expectedUser);
        var user = _mockUserRepository.Object.GetUserById(expectedUser.Id).Result;

        Assert.False(user!.IsAuthorized);
    }

    [Fact]
    public async Task TestSignOutNonexistent()
    {
        var expectedUser = CreateMockUser();
        var otherUser = CreateMockUser("another@email", "another_password");

        _mockUserRepository
            .Setup(s => s.GetUserById(otherUser.Id))
            .ReturnsAsync(otherUser);

        async Task Action() => await _oauthService.SignOutUser(expectedUser);

        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    private static User CreateMockUser(string email = "email", string passwd = "password")
    {
        return new(Guid.NewGuid(), Guid.Empty, "usrname", email, PasswordHasher.HashPassword(passwd), false, false);
    }

    private static List<User> CreateMockUsers()
    {
        return
        [
            new(Guid.NewGuid(), Guid.Empty, "user1", "u1@mail.ru", PasswordHasher.HashPassword("passwd1"), true, false),
            new(Guid.NewGuid(), Guid.Empty, "user2", "u2@mail.ru", PasswordHasher.HashPassword("passwd2"), false, false),
            new(Guid.NewGuid(), Guid.Empty, "user3", "u3@mail.ru", PasswordHasher.HashPassword("passwd3"), false, true),
            new(Guid.NewGuid(), Guid.Empty, "user4", "u4@mail.ru", PasswordHasher.HashPassword("passwd4"), true, true)
        ];
    }
}