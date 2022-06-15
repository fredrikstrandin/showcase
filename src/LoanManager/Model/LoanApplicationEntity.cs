using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class LoanApplicationEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public DateTime Created { get; set; }
        public LoanType Type { get; set; }
        public int Amount { get; set; }
        //Antar att man inte går in på delar av månader
        public int Duration { get; set; }

        public static implicit operator LoanApplicationItem(LoanApplicationEntity entity)
        {
            if(entity == null)
            {
                return null;
            }

            return new LoanApplicationItem()
            {
                Id = entity.Id.ToString(),
                UserId = entity.UserId.ToString(),
                Amount = entity.Amount,
                Duration = entity.Duration,
                Type = entity.Type
            };
        }
    }
}
