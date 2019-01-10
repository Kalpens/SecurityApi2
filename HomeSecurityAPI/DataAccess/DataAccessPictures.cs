using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSecurityAPI.Logic;
using HomeSecurityAPI.Models;

namespace HomeSecurityAPI.DataAccess
{
    public class DataAccessPictures
    {
        MongoClient _client;
        static IMongoDatabase _db;
        private ImageHandler imageHandler;
        public DataAccessPictures()
        {
            _client = new MongoClient("mongodb://Kristof:asdlol1@ds127704.mlab.com:27704/home-security");
            _db = _client.GetDatabase("home-security");
            imageHandler = new ImageHandler();
        }

        public async Task<Picture> Create(Picture p)
        {
            var dateTime = DateTime.Now;
            dateTime = dateTime.ToUniversalTime();
            BsonDocument picture = new BsonDocument {
                {"userID" , p.userID },
                {"Base64" , p.Base64},
                {"Timestamp" , dateTime },
                {"PictureName" , ""  }
            };
            p.Timestamp = dateTime;
            var collection = _db.GetCollection<BsonDocument>("Pictures");
            await collection.InsertOneAsync(picture);
            p.Id = picture[0].AsObjectId;

            p.PictureName = p.Timestamp.ToString("dd-MM-yyyy--") + p.Id;
            imageHandler.ConvertAndStore(p.Base64, p.PictureName);
            var updateResult = await Update(p);

            return p;
        }

        public async Task<List<Picture>> GetAllbyUserID(int id)
        {
            var col = _db.GetCollection<Picture>("Pictures");
            return await col.Find(pic => pic.userID == id).ToListAsync();
            
        }

        public async Task<Boolean> Update(Picture p)
        {
            var col = _db.GetCollection<Picture>("Pictures");
            var filter = Builders<Picture>.Filter.Eq(dbPicture => dbPicture.Id, p.Id);
            var update = Builders<Picture>.Update.Set(dbPicture => dbPicture.PictureName, p.PictureName);
            var result = await col.UpdateOneAsync(filter, update);

            if (result.MatchedCount >= 1)
                return true;
            return false;
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

        public async Task<List<Picture>> GetPictureByInterval(DateTime date1, DateTime date2)
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
