﻿using CustomerManager.Model;
using MongoDB.Driver;

namespace CustomerManager.DataContexts
{
    public interface IMongoDBContext
    {
        IMongoDatabase _database { get; set; }
        IMongoCollection<UserEntity> Customers {  get; }
    }
}