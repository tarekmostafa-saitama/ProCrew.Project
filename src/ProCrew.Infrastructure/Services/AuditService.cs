using Google.Cloud.Firestore;
using Mapster;
using ProCrew.Application;
using ProCrew.Application.Common.Exceptions;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Audits.Models;
using ProCrew.Infrastructure.Trails;

namespace ProCrew.Infrastructure.Services;

public class AuditService(FirestoreDb fireStoreDb) : IAuditService
{
    private const string CollectionName = "Audits";

    public async Task CreateAuditsAsync(List<Trail> trails)
    {
        var collection = GetCollection();
        foreach (var trail in trails) await collection.AddAsync(trail.Adapt<TrailStore>());
    }

    public async Task<List<Trail>> GetAllAuditsAsync()
    {
        var collection = GetCollection();
        var snapshot = await collection.GetSnapshotAsync();
        var trailStores =  snapshot.Documents.Select(s => s.ConvertTo<TrailStore>()).ToList();
        return trailStores.Adapt<List<Trail>>();
    }

    public async Task<Trail> GetAuditByIdAsync(string id)
    {
        var collection = GetCollection();
        var document = await collection.Document(id).GetSnapshotAsync();
        return document.Exists ? document.ConvertTo<TrailStore>().Adapt<Trail>() : throw new NotFoundException("Trial is not found");
    }

    private CollectionReference GetCollection()
    {
        return fireStoreDb.Collection(CollectionName);
    }
}