using QAgencyAppointment.Business.Interface;
using NSubstitute;
using QAgencyAppointment.Business.Dtos;

namespace QAgencyAppointment.Test.ServiceTests;

public class UserServiceTest : IClassFixture<UserFixture>
{
    private readonly UserFixture _fixture;
    
    public UserServiceTest(UserFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async void AuthenticateTest()
    {
        string result = await _fixture.UserService.Authenticate(new() { Username = "", Password = "admin#12345" });
        Assert.Null(result);
        
        string result2 = await _fixture.UserService.Authenticate(new() { Username = "admin", Password = "" });
        Assert.Null(result2);
        
        string result3 = await _fixture.UserService.Authenticate(new() { Username = "admin", Password = "admin#12345" });
        Assert.NotNull(result3);
    }
} 

public class UserFixture : IDisposable
{
    public IUserService UserService { get; }

    public UserFixture()
    {
        UserService = Substitute.For<IUserService>();
        UserService.Authenticate(Arg.Is<LoginDto>(x => x.Username.Length == 0 || x.Password.Length == 0))
            .Returns(Task.FromResult<string?>(null));
        
        UserService.Authenticate(Arg.Is<LoginDto>(x => x.Username.Length > 0 && x.Password.Length > 0))
            .Returns(Task.FromResult<string?>(""));
    }

    public void Dispose()
    {
    }
}