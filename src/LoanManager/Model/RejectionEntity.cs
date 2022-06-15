using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class RejectionEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId LoanId { get; set; }
        public ObjectId UserId { get; set; }
        public DateTime Created { get; set; }
        public LoanType Type { get; set; }
        public int Amount { get; set; }
        //Antar att man inte går in på delar av månader
        public int Duration { get; set; }

        public static implicit operator RejectionEntity(LoanApplicationEntity entity)
        {
            if(entity == null)
            {
                return null;
            }

            return new RejectionEntity()
            {
                LoanId = entity.Id,
                UserId = entity.UserId,
                Created = entity.Created,
                Amount = entity.Amount,
                Duration = entity.Duration,
                Type = entity.Type
            };
        }


        public static implicit operator LoanApplicationItem(RejectionEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new LoanApplicationItem()
            {
                Id = entity.LoanId.ToString(),
                UserId = entity.UserId.ToString(),
                Amount = entity.Amount,
                Duration = entity.Duration,
                Type = entity.Type
            };
        }
    }
}
