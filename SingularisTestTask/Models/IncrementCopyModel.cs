using MongoDB.Bson.Serialization.Attributes;

namespace SingularisTestTask.Models;

public class IncrementCopyModel
{
    [BsonId]
    public string DestinationFolder { get; set; }

    public HashSet<string> Dirs { get; set; }
    public Dictionary<string, string> Files { get; set; }

    public IncrementCopyModel(string destinationFolder, HashSet<string> dirs,
        Dictionary<string, string> files)
    {
        DestinationFolder = destinationFolder;
        Dirs = dirs;
        Files = files;
    }
    public IncrementCopyModel(string destinationFolder)
    {
        DestinationFolder = destinationFolder;
        Dirs = new HashSet<string>();
        Files = new Dictionary<string, string>();
    }
}