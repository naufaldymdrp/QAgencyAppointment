namespace QAgencyAppointment.Business;

public record BusinessResult<T>(bool Success, T payload, string? BusinessError);
    