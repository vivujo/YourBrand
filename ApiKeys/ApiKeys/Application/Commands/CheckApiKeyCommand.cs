
using YourBrand.ApiKeys.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.ApiKeys.Domain.Enums;

namespace YourBrand.ApiKeys.Application.Commands;

public record CheckApiKeyCommand(string ApiKey, string Origin, string ServiceSecret, string[] RequestedResources) : IRequest<ApiKeyResult>
{
    public class CheckApiKeyCommandHandler : IRequestHandler<CheckApiKeyCommand, ApiKeyResult>
    {
        private readonly IApiKeysContext context;

        public CheckApiKeyCommandHandler(IApiKeysContext context)
        {
            this.context = context;
        }

        public async Task<ApiKeyResult> Handle(CheckApiKeyCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Origin: " + request.Origin);

            var apiKey = await context.ApiKeys
                .Include(a => a.ApiKeyServices)
                .ThenInclude(a => a.Service)
                .Where(a => a.ApiKeyServices.Any(s => s.Service.Secret == request.ServiceSecret /* && s.Service.Url == request.Origin */))
                .FirstOrDefaultAsync(apiKey => apiKey.Key == request.ApiKey);

            return apiKey is null
                ? new ApiKeyResult(ApiKeyAuthCode.Unauthorized)
                : new ApiKeyResult(apiKey.Status switch
                {
                    ApiKeyStatus.Active => ApiKeyAuthCode.Authorized,
                    ApiKeyStatus.Expired => ApiKeyAuthCode.Expired,
                    ApiKeyStatus.Revoked => ApiKeyAuthCode.Revoked,
                    _ => throw new Exception()
                });
        }
    }
}

public record ApiKeyResult(ApiKeyAuthCode Status);

public enum ApiKeyAuthCode
{
    Authorized,
    Unauthorized,
    Expired,
    Revoked
}