using Google.Cloud.Firestore;

namespace ProCrew.Infrastructure.Trails;

[FirestoreData]
public class TrailStore
{
    [FirestoreDocumentId] public string Id { get; set; }

    [FirestoreProperty] public string Type { get; set; }

    [FirestoreProperty] public string TableName { get; set; }

    [FirestoreProperty] public DateTime DateTime { get; set; }

    [FirestoreProperty] public string OldValues { get; set; }

    [FirestoreProperty] public string NewValues { get; set; }

    [FirestoreProperty] public string AffectedColumns { get; set; }

    [FirestoreProperty] public string PrimaryKey { get; set; }
}