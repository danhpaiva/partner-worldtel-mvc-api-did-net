using Partner.WorldTel.Did.Api.Models;

namespace Partner.WorldTel.Did.Api.Interface;

public interface IDidGeneratorService
{
    Task<InternationalDid> CreateFromE164NumberAsync(string e164Number, string createdBy = "system");
}
