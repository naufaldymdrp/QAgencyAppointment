namespace QAgencyAppointment.DataAccess.Interfaces;

public interface ISeeding
{
    bool SeedRoles();
    bool SeedUsers();
    bool SeedUserRoles();
}