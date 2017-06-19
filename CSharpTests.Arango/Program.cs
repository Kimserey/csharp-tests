using ArangoDB.Client;
using ArangoDB.Client.Query;
using System;
using System.Linq;
using System.Net;

namespace CSharpTests.Arango
{
    class Person
    {
        [DocumentProperty(Identifier = IdentifierType.Key)]
        public string Key;
        public string Name;
        public int Age;
    }

    class Program
    {
        static void Main(string[] args)
        {
            ArangoDatabase.ChangeSetting(s => {
                s.Credential = new NetworkCredential("root", "123456");
            });

            using (var db = new ArangoDatabase(url: "http://localhost:8529", database: "TestDatabase"))
            {
                ///////////////////// insert and update documents /////////////////////////

                var person = new Person { Name = "Kim", Age = 26 };

                // insert new document and creates 'Person' collection on the fly
                db.Insert<Person>(person);

                person.Age = 27;

                // partially updates person, only 'Age' attribute will be updated
                db.UpdateAsync<Person>(person).Wait();

                // returns 27
                db.Query<Person>()
                    .Where(p => AQL.Contains(p.Name, "raoof"))
                    .Select(p => p.Name)
                    .ForEachAsync(x => Console.WriteLine(x))
                    .Wait();

                db.Query<Person>()
                    .Join(db.Query<Person>(), (p1) => p1.Age, (p2) => p2.Age, (p1, p2) => new { Name = p2.Name })
                    .ForEachAsync(x => Console.WriteLine(x))
                    .Wait();

                /////////////////////// aql modification queries ////////////////////////////

                // remove products transactionally and return removed results
                //var removedResults = db.Query<Product>().Where(p => p.Quantity == 0).Remove().ToList();

                // insert products to backup collection
                //db.Query<Product>().Insert().In<ProductBackup>().Execute();

                ///////////////////////      simple join        /////////////////////////////

                //var query = from movie in db.Query<Movie>()
                //            from director in db.Query<Director>()
                //            where movie.DirectorKey == director.Key
                //            select new { movie.Title, director.Name };
            }
        }
    }
}