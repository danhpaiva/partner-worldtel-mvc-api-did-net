using Microsoft.EntityFrameworkCore;
using Partner.WorldTel.Did.Api.Data;
using Partner.WorldTel.Did.Api.Enum;
using Partner.WorldTel.Did.Api.Interface;
using Partner.WorldTel.Did.Api.Models;
using System.Text.RegularExpressions;

namespace Partner.WorldTel.Did.Api.Service
{
    public class DidGeneratorService : IDidGeneratorService
    {
        private readonly AppDbContext _context;
        private static readonly Regex E164Regex = new(@"^\+(\d{1,3})(\d{1,5})(\d{4,10})$", RegexOptions.Compiled);

        public DidGeneratorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InternationalDid> CreateFromE164NumberAsync(string e164Number, string createdBy = "system")
        {
            if (string.IsNullOrWhiteSpace(e164Number))
                throw new ArgumentException("O número E.164 é obrigatório.", nameof(e164Number));

            var cleanNumber = Regex.Replace(e164Number, @"[^\d+]", "");
            if (!cleanNumber.StartsWith("+"))
                throw new ArgumentException("O número deve estar no formato E.164 (ex: +553122334455).");

            var match = E164Regex.Match(cleanNumber);
            if (!match.Success)
                throw new ArgumentException("Formato E.164 inválido. Use: +[código país][área][número local].");

            var countryCode = match.Groups[1].Value;
            var areaCode = match.Groups[2].Value;
            var localNumber = match.Groups[3].Value.PadRight(10, '0')[..10]; // Garante 10 dígitos

            if (!await IsCountryCodeValid(countryCode))
                throw new ArgumentException($"Código de país inválido: {countryCode}");

            if (!IsAreaCodeValid(areaCode))
                throw new ArgumentException($"Código de área inválido: {areaCode} (máx 5 dígitos).");

            if (!IsLocalNumberValid(localNumber))
                throw new ArgumentException($"Número local inválido: {localNumber} (4 a 10 dígitos).");

            var didId = GenerateDidId(countryCode, areaCode, localNumber);

            if (await _context.InternationalDid.AnyAsync(d => d.DidId == didId))
                throw new InvalidOperationException($"DID já existe: {didId}");

            var did = new InternationalDid
            {
                DidId = didId,
                CountryCode = countryCode,
                AreaCode = areaCode,
                LocalNumber = localNumber,
                Status = DidStatus.Pending,
                ActivationDate = null,
                BillingCycle = "Monthly",
                Currency = GetCurrencyForCountry(countryCode),
                MonthlyFee = CalculateMonthlyFee(countryCode, areaCode),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            _context.InternationalDid.Add(did);
            await _context.SaveChangesAsync();

            return did;
        }

        private static string GenerateDidId(string countryCode, string areaCode, string localNumber)
        {
            return $"WT-INT-{countryCode}{areaCode}{localNumber}";
        }

        private async Task<bool> IsCountryCodeValid(string countryCode)
        {
            var validCodes = new HashSet<string> { "1", "55", "44", "33", "49", "81", "86" };
            return validCodes.Contains(countryCode) || await Task.FromResult(true); // Extensível
        }

        private static bool IsAreaCodeValid(string areaCode)
            => areaCode.Length <= 5 && Regex.IsMatch(areaCode, @"^\d+$");

        private static bool IsLocalNumberValid(string localNumber)
            => localNumber.Length >= 4 && localNumber.Length <= 10 && Regex.IsMatch(localNumber, @"^\d+$");

        private static string GetCurrencyForCountry(string countryCode) => countryCode switch
        {
            "1" => "USD",
            "55" => "BRL",
            "44" => "GBP",
            "33" => "EUR",
            "49" => "EUR",
            _ => "USD"
        };

        private static decimal CalculateMonthlyFee(string countryCode, string areaCode)
        {
            var baseFee = countryCode switch
            {
                "1" => 1.99m,
                "55" => 49.90m,
                "44" => 2.50m,
                _ => 3.99m
            };

            var premiumArea = new[] { "11", "21", "31", "800", "900" };
            return premiumArea.Contains(areaCode) ? baseFee * 1.5m : baseFee;
        }
    }
}
