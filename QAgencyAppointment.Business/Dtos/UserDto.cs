namespace QAgencyAppointment.Business.Dtos;

public record class UserDto
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}
