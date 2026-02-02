using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Opah.TransactionService.Infrastructure.KeyCloak
{
    public class KeycloakRolesTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
                return Task.FromResult(principal);

            var realmAccess = identity.FindFirst("realm_access")?.Value;
            if (string.IsNullOrEmpty(realmAccess))
                return Task.FromResult(principal);

            using var doc = JsonDocument.Parse(realmAccess);

            if (doc.RootElement.TryGetProperty("roles", out var roles))
            {
                foreach (var role in roles.EnumerateArray())
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString()!));
                }
            }

            return Task.FromResult(principal);
        }
    }

}
