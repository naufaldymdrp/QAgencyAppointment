namespace QAgencyAppointment.DataAccess;

public record RepositoryResult(bool Success, string? ErrorMessage = null, string? Detail = null);