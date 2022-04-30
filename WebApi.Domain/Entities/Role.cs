using WebApi.Domain.Contracts;

namespace WebApi.Domain.Entities;

public class Role : AuditableEntity<Guid>
{
    public string Name { get; set; }
    public List<User> Users { get; set; }
}