using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class DecisionEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId LoanId { get; set; }
        public DateTime Created { get; set; }
        public bool Aproved { get; set; }
        public string Decision { get; set; }
    }
}
