﻿using HomeSecurityAPI.Interfaces;
using HomeSecurityAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeSecurityAPI.Services
{
    public class UserService : IUserService
    {
        
        private MongoClient _client;
        private static IMongoDatabase _db;
 

        public UserService()
        {
            _client = new MongoClient("mongodb://Kristof:asdlol1@ds127704.mlab.com:27704/home-security");
            _db = _client.GetDatabase("home-security");
        }

        public async Task<User> Authenticate(User u)
        {

            var collection = _db.GetCollection<User>("Users");
            // needs rebuild for MongoDB
            var user = await collection.Find(x => x.Username == u.Username).SingleOrDefaultAsync();
            if (user == null)
                throw new InvalidDataException("User Not Found");


            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(u, user.Password, u.Password);
            if (result == PasswordVerificationResult.Failed) throw new AuthenticationException("User failed to log in");

            return user;
        }

        public async Task<User> GetbyUsername(string username)
        {
            var col = _db.GetCollection<User>("Users");
            var result = await col.Find(user => user.Username == username).SingleAsync();
            return result;
        }

        public async Task<List<User>> GetAll()
        {
            var col = _db.GetCollection<User>("Users");
            var lst = await col.Find(_ => true).ToListAsync();

            return lst;
        }

        public async Task<User> Create(User u)
        {
            var hasher = new PasswordHasher<User>();
            u.Password = hasher.HashPassword(u, u.Password);
            //values cant be null
            BsonDocument user = new BsonDocument { 
                {"FirstName" , u.FirstName},
                {"LastName" , u.LastName},
                {"Username" , u.Username},
                {"Password" , u.Password}
            };

            var col = _db.GetCollection<BsonDocument>("Users");
            await col.InsertOneAsync(user);
            return u;
        }

        public async Task<User> Update(List<User> u)
        {
            var oldUser = u.ElementAt(0);
            var newUser = u.ElementAt(1);

            BsonDocument user = new BsonDocument {
                {"FirstName" , newUser.FirstName},
                {"LastName" , newUser.LastName},
                {"Username" , newUser.Username},
                {"Password" , oldUser.Password}
            };

            var col = _db.GetCollection<BsonDocument>("Users");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter;

            filter = builder.Eq("Username", oldUser.Username);
            await col.ReplaceOneAsync(filter, user);

            return newUser;
        }


        public async Task<Boolean> Delete(string username)
        {
            var col = _db.GetCollection<User>("Users");
            var result = await col.DeleteOneAsync(p => p.Username == username);
            if (result.DeletedCount == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //neeed delete update

    }
}
