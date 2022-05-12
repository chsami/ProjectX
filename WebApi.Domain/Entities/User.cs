using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Domain.Contracts;

namespace WebApi.Domain.Entities
{
    public class User : AuditableEntity<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new string Id { get; set; }
        public string Email { get; set; }
        public virtual List<Role> Roles { get; set; }
        public virtual List<Tenant> Tenants { get; set; }
    }
}
