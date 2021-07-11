using BooksAPI1.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI1.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        MongoClient client;

        public BookService(IBookstoreDatabaseSettings settings)
        {
            client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);


            _books = database.GetCollection<Book>(settings.BooksCollectionName);

            var indexKeysDefinition = Builders<Book>.IndexKeys.Ascending(book => book.BookName);
            _books.Indexes.CreateOneAsync(new CreateIndexModel<Book>(indexKeysDefinition));

        }

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            try
            {
                _books.InsertOne(book);
                return book;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///Create a new book and increase the price of all the existing book and this book by 10%
        ///using transactions
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        //public async Task<Book> Create(Book book)
        //{
        //    using (var session = await client.StartSessionAsync())
        //    {
        //        // Begin transaction
        //        session.StartTransaction();
        //        try
        //        {
        //            // Insert the book data
        //            await _books.InsertOneAsync(session, book);

        //            var resultsBeforeUpdates = await _books.Find<Book>(session, Builders<Book>.Filter.Empty).ToListAsync();

        //            Console.WriteLine("Original Prices:\n");
        //            foreach (Book d in resultsBeforeUpdates)
        //            {
        //                Console.WriteLine(String.Format("Book Name: {0}\tPrice: {1:0.00}", d.BookName, d.Price));
        //            }
        //            // Increase all the prices by 10% for all books
        //            var update = new UpdateDefinitionBuilder<Book>().Mul<decimal>(r => r.Price, (decimal)1.1);
        //            await _books.UpdateManyAsync(session, Builders<Book>.Filter.Empty, update);
        //            await session.CommitTransactionAsync();
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Error writing to MongoDB: " + e.Message);
        //            await session.AbortTransactionAsync();
        //            return null;
        //        }
        //        Console.WriteLine("\n\nNew Prices (10% increase):\n");
        //        var resultsAfterCommit = await _books.Find<Book>(session, Builders<Book>.Filter.Empty).ToListAsync();
        //        foreach (Book book1 in resultsAfterCommit)
        //        {
        //            Console.WriteLine(String.Format("Product Name: {0}\tPrice: {1:0.00}", book1.BookName, book.Price));
        //        }
        //        return book;
        //    }
        //}

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}
