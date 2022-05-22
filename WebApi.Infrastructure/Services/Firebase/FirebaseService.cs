using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using WebApi.Infrastructure.Services.Firebase;
using Google.Cloud.Firestore;
using WebApi.Domain;
using WebApi.Infrastructure.Services.Firebase.Models;

namespace WebApi.Infrastructure.Services
{
    public class FirebaseService : IFireBaseService
    {
        private readonly FirestoreDb _db;
        public FirebaseService(IConfiguration configuration)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", configuration.GetSection("Firebase:Keyfile").Value);
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.GetApplicationDefault()
                });
            }
            _db = FirestoreDb.Create(configuration.GetSection("Firebase:ValidAudience").Value);
        }

        public async Task SetCustomUserClaimsAsync(string uid, IList<string> roles)
        {
            var claims = new Dictionary<string, object>();
            
            foreach (var role in roles)
            {
                claims.Add(role, true);
            }
            
            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
        }

        public async Task<UserRecord> CreateUser(string email, string password)
        {
            var result = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs()
            {
                Email = email,
                Password = password
            });

            return result;
        }

        public async Task SaveDocument(ProductDocument productDocument)
        {
            await _db.Collection("test").AddAsync(productDocument);
        }
    }
}
