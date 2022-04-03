using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Domain.Contracts;
using WebApi.Domain.Interfaces;

namespace WebApi.Domain
{
    public class Product : AuditableEntity<int>
    {
        public string Name { get; set; }
    }
}
