using Domain.Core.Abstractions.Stream;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace Payment.Infrastructure.Models;

internal class IEventStreamBD
{
    public IEventStreamBD(IEventStream @event)
    {
        Event = @event;
        IdCorrelation = @event.IdCorrelation.ToString(); 
    }

    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }

    [DataMember]
    public MongoDB.Bson.BsonString? _t { get; set; } 

    public IEventStream Event { get; set; }

    public string IdCorrelation { get; init; }
}
