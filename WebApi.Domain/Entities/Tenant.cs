using WebApi.Domain.Contracts;

namespace WebApi.Domain.Entities;

public class Tenant : AuditableEntity<Guid>
{
    public string Name { get; set; }
    public string Domain { get; set; }
    public string Country { get; set; }
    public List<User> Users { get; set; }
}
