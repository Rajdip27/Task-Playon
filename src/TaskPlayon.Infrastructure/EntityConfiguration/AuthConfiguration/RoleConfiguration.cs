using TaskPlayon.Domain.Model.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace TaskPlayon.Infrastructure.EntityConfiguration.AuthConfiguration;

public class RoleConfiguration :IEntityTypeConfiguration<IdentityModel.Role>
{
    public void Configure(EntityTypeBuilder<IdentityModel.Role> builder)
    {
        builder.HasData(new IdentityModel.Role
        {
            Id = 1,
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR",
            Description = "ADMINISTRATOR",

        }, new IdentityModel.Role
        {
            Id = 2,
            Name = "Employee",
            NormalizedName = "EMPLOYEE",
            Description = "ADMINISTRATOR",
        }, new IdentityModel.Role
        {
            Id = 3,
            Name = "Trainer",
            NormalizedName = "TRAINER",
            Description = "ADMINISTRATOR",
        }, new IdentityModel.Role
        {
            Id = 4,
            Name = "Student",
            NormalizedName = "STUDENT",
            Description = "ADMINISTRATOR",
        });
    }
}
