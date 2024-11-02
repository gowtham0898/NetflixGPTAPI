using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NetflixGPT.infra.DB.Entities
{
    public class MovieEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }


        [BsonElement("releaseYear")]
        public int ReleaseYear { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("overview")]
        public string? Overview { get; set; }

        [BsonElement("videoUrl")]
        public string? VideoUrl { get; set; }

        [BsonElement("photoUrl")]
        public string? PhotoUrl { get; set; }

        [BsonElement("ids")]
        public MovieIds Ids { get; set; }
    }

    public class MovieIds
    {
        [BsonElement("trakt")]
        public int Trakt { get; set; }

        [BsonElement("slug")]
        public string Slug { get; set; }

        [BsonElement("imdb")]
        public string Imdb { get; set; }

        [BsonElement("tmdb")]
        public int Tmdb { get; set; }
    }
}

