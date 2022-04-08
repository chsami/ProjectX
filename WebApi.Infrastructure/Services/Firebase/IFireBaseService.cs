using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Infrastructure.Services.Firebase.Models;

namespace WebApi.Infrastructure.Services.Firebase
{
    public interface IFireBaseService
    {
        Task<UserRecord> CreateUser(string email, string password);
        Task SaveDocument(ProductDocument productDocument);

    }
}
