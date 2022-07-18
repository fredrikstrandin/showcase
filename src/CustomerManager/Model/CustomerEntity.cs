using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Model
{
    public class UserEntity
    {
        //Jag använder inte personnummer som id på grund av GDPR. Man vill inte skicka runt det på siten.
        [BsonId]
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        
        public static implicit operator UserEntity(UserItem item)
        {
            if (item == null)
            {
                return null;
            }

            if(ObjectId.TryParse(item.Id, out ObjectId id))
            {
                return new UserEntity()
                {
                    Id = id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    Street = item.Street,
                    Zip = item.Zip,
                    City = item.City
                };
            }
            else
            {
                throw new ArgumentException("Id is not ObjectId");
            }
        }

        public static implicit operator UserItem(UserEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new UserItem(entity.Id.ToString(), entity.FirstName, entity.LastName, entity.Email, entity.Street, entity.Zip, entity.City);
        }
    }
}
