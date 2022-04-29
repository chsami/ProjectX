using WebApi.Domain.Contracts;

namespace WebApi.Domain.Entities;

public class Tenant : Entity<Guid>
{
    public string Name { get; set; }
    public List<User> Users { get; set; }
}
