using Opah.ReportService.Infrastructure.KeyCloak;
using System.Security.Claims;
using System.Text.Json;

namespace Opah.ReportService.Infrastructure.Test
{
    public class KeycloakRolesTransformationTests
    {
        private static ClaimsPrincipal CreatePrincipal(
            bool authenticated,
            string? realmAccessJson = null)
        {
            var identity = new ClaimsIdentity(
                authenticationType: authenticated ? "Bearer" : null
            );

            if (!string.IsNullOrEmpty(realmAccessJson))
            {
                identity.AddClaim(new Claim("realm_access", realmAccessJson));
            }

            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task TransformAsync_Should_Return_Principal_When_Not_Authenticated()
        {
            // Arrange
            var principal = CreatePrincipal(authenticated: false);
            var transformer = new KeycloakRolesTransformation();

            // Act
            var result = await transformer.TransformAsync(principal);

            // Assert
            Assert.Empty(result.Claims);
        }

        [Fact]
        public async Task TransformAsync_Should_Not_Add_Roles_When_RealmAccess_Is_Missing()
        {
            // Arrange
            var principal = CreatePrincipal(authenticated: true);
            var transformer = new KeycloakRolesTransformation();

            // Act
            var result = await transformer.TransformAsync(principal);

            // Assert
            Assert.Empty(result.FindAll(ClaimTypes.Role));
        }

        [Fact]
        public async Task TransformAsync_Should_Not_Add_Roles_When_RealmAccess_Is_Empty()
        {
            // Arrange
            var principal = CreatePrincipal(authenticated: true, realmAccessJson: "");
            var transformer = new KeycloakRolesTransformation();

            // Act
            var result = await transformer.TransformAsync(principal);

            // Assert
            Assert.Empty(result.FindAll(ClaimTypes.Role));
        }

        [Fact]
        public async Task TransformAsync_Should_Not_Add_Roles_When_Roles_Property_Does_Not_Exist()
        {
            // Arrange
            var realmAccess = JsonSerializer.Serialize(new
            {
                somethingElse = new[] { "admin" }
            });

            var principal = CreatePrincipal(true, realmAccess);
            var transformer = new KeycloakRolesTransformation();

            // Act
            var result = await transformer.TransformAsync(principal);

            // Assert
            Assert.Empty(result.FindAll(ClaimTypes.Role));
        }

        [Fact]
        public async Task TransformAsync_Should_Add_Roles_From_RealmAccess()
        {
            // Arrange
            var realmAccess = JsonSerializer.Serialize(new
            {
                roles = new[] { "admin", "report-viewer" }
            });

            var principal = CreatePrincipal(true, realmAccess);
            var transformer = new KeycloakRolesTransformation();

            // Act
            var result = await transformer.TransformAsync(principal);

            // Assert
            var roles = result
                .FindAll(ClaimTypes.Role)
                .Select(r => r.Value)
                .ToList();

            Assert.Contains("admin", roles);
            Assert.Contains("report-viewer", roles);
            Assert.Equal(2, roles.Count);
        }

        [Fact]
        public async Task TransformAsync_Should_Keep_Existing_Claims()
        {
            // Arrange
            var realmAccess = JsonSerializer.Serialize(new
            {
                roles = new[] { "admin" }
            });

            var identity = new ClaimsIdentity("Bearer");
            identity.AddClaim(new Claim(ClaimTypes.Name, "danilo"));
            identity.AddClaim(new Claim("realm_access", realmAccess));

            var principal = new ClaimsPrincipal(identity);
            var transformer = new KeycloakRolesTransformation();

            // Act
            var result = await transformer.TransformAsync(principal);

            // Assert
            Assert.Equal("danilo", result.Identity!.Name);
            Assert.Contains(result.Claims, c =>
                c.Type == ClaimTypes.Role && c.Value == "admin");
        }
    }
}
