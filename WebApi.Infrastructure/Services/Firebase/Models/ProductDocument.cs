using Google.Cloud.Firestore;

namespace WebApi.Infrastructure.Services.Firebase.Models;
[FirestoreData]
public class ProductDocument
{
    [FirestoreProperty]
    public string Name { get; set; }
}