using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace App.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; private set; }
        
        [BsonElement("firstName")]
        public string FirstName { get; private set; }
        
        [BsonElement("surname")]
        public string Surname { get; private set; }
                
        [BsonElement("age")]
        public int Age { get; private set; }

        public User(string firstName, string surname, int age)
        {
            this.FirstName = firstName;
            this.Surname = surname;
            this.Age = age;
        }
    }
}