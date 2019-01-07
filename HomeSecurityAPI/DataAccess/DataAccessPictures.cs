﻿using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSecurityAPI.Models;

namespace HomeSecurityAPI.DataAccess
{
    public class DataAccessPictures
    {
        MongoClient _client;
        static IMongoDatabase _db;
        public DataAccessPictures()
        {
            _client = new MongoClient("mongodb://Kristof:asdlol1@ds127704.mlab.com:27704/home-security");
            _db = _client.GetDatabase("home-security");
        }

        public async Task<Picture> Create(Picture p)
        {
            BsonDocument picture = new BsonDocument {
                {"userID" , p.userID },
                {"Base64" , p.Base64},
                {"Timestamp" , DateTime.Now }
            };
            p.Timestamp = DateTime.Now;
            var collection = _db.GetCollection<BsonDocument>("Pictures");
            await collection.InsertOneAsync(picture);
            p.Id = picture[0].AsObjectId;
            return p;
        }

        public async Task<List<Picture>> GetAllbyUserID(int id)
        {
            var col = _db.GetCollection<Picture>("Pictures");
            return await col.Find(pic => pic.userID == id).ToListAsync();
            
        }

        public async Task<Boolean> Delete(string objId)
        {
            var col = _db.GetCollection<Picture>("Pictures");
            var result = await col.DeleteOneAsync(p => p.Id == ObjectId.Parse(objId));
            if (result.DeletedCount >= 1)
                return true;
            return false;
        }
        public async Task<List<Picture>> GetPicturesByDate(DateTime date)
        {
            var dateStart = date.Date;
            var dateEnd = date.Date.AddDays(1);
            var col = _db.GetCollection<Picture>("Pictures");
            return await col.Find(pic => pic.Timestamp >= dateStart && pic.Timestamp <= dateEnd).ToListAsync();
        }

        public async Task<List<Picture>> GetPictureByIntervallum(DateTime date1, DateTime date2)
        {
            var col = _db.GetCollection<Picture>("Pictures");
            return await col.Find(pic =>
                    pic.Timestamp >= date1
                    &&
                    pic.Timestamp <= date2).ToListAsync();
        }
        //we need pictures within certain time frames
    }
}
