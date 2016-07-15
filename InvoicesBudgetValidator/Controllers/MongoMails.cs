using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicesBudgetValidator.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace InvoicesBudgetValidator.Controllers
{
    class MongoMails
    {
        string defaultmail = null;

        public string getResponseMail(Default_Mail mail)
        {
                string mongo = ConfigurationManager.AppSettings["Mongo"].ToString();
                string db = ConfigurationManager.AppSettings["DB"].ToString();
                string coll = ConfigurationManager.AppSettings["Collection"].ToString();
                var client = new MongoClient(mongo);
                var database = client.GetDatabase(db);
                var collection = database.GetCollection<BsonDocument>(coll);

                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq("rfc", mail.rfc) & builder.Eq("company_id", mail.company_id);
                var emails = collection.Find(filter)
                    .ToListAsync()
                    .Result;

                if (emails.Count > 0)
                {
                    string json = emails.ToJson()//.Replace("ObjectId(", "").Replace("),", ",")
                        .Remove(0, 1);
                    json = json.Remove(json.Length - 1);
                    var x = BsonSerializer.Deserialize<Default_Mail>(emails.First().ToBsonDocument());
                    defaultmail = x.default_email;
                }

                return defaultmail;
        }
    }
}
