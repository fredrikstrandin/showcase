using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Model
{
    public class CustomerEntity
    {
        //Jag använder inte personnummer som id på grund av GDPR. Man vill inte skicka runt det på siten.
        [BsonId]
        public ObjectId Id { get; set; }
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public int MonthlyIncome { get; set; }

        public static implicit operator CustomerEntity(CustomerItem item)
        {
            if (item == null)
            {
                return null;
            }

            return new CustomerEntity()
            {
                PersonalNumber = item.PersonalNumber,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Street = item.Street,
                Zip = item.Zip,
                City = item.City,
                MonthlyIncome = item?.MonthlyIncome ?? 0
            };
        }

        public static implicit operator CustomerItem(CustomerEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CustomerItem(entity.Id.ToString(), entity.PersonalNumber, entity.FirstName, entity.LastName, entity.Email, entity.Street, entity.Zip, entity.City, entity?.MonthlyIncome ?? 0);
        }

        public static implicit operator CustomerEntity(CustomerCreateItem item)
        {
            if (item == null)
            {
                return null;
            }

            return new CustomerEntity()
            {
                PersonalNumber = item.PersonalNumber,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Hash = item.Hash,
                Salt = item.Salt,
                Street = item.Street,
                Zip = item.Zip,
                City = item.City,
                MonthlyIncome = item?.MonthlyIncome ?? 0
            };
        }

        public static implicit operator CustomerCreateItem(CustomerEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CustomerCreateItem(entity.PersonalNumber, entity.FirstName, entity.LastName, entity.Email, entity.Hash, entity.Salt, entity.Street, entity.Zip, entity.City, entity?.MonthlyIncome ?? 0);
        }
    }
}
