namespace SpendWise.SharedKernel.Storage;

public record FileResponse(
    Stream Stream,
    string ContentType,
    string FileName);
