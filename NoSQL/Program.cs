using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NoSQL
{
	public class Program
	{
		private static IMongoCollection<Book> bookCollection;

		static void Main(string[] args)
		{
			var client = new MongoClient();
			IMongoDatabase db = client.GetDatabase("Library");

			bookCollection = ConnectAndCreateMongoCollection(db);

			/*2. Найдите книги с количеством экземпляров больше единицы.       
            
             a. Покажите в результате только название книги.
             b. Отсортируйте книги по названию.
             c. Ограничьте количество возвращаемых книг тремя.
             d. Подсчитайте количество таких книг.
             */
			Console.WriteLine($"Task 2.\n");

			var sortedCollection = bookCollection.Find(book => book.Count > 1).SortBy(book => book.Name);

			Console.WriteLine($"Count of books with count > 1: {sortedCollection.CountDocuments()}.\n");
			Console.WriteLine($"First 3 books from the list with count > 1:");
			sortedCollection.Limit(3).ToList().ForEach(book => Console.WriteLine(book.Name));

			// 3. Найдите книгу с макимальным/минимальным количеством (count).
			Console.WriteLine($"\nTask 3.\n");
			var minBook = bookCollection.Find(book => true).SortBy(book => book.Count).Limit(1).FirstOrDefault();
			var maxBook = bookCollection.Find(book => true).SortByDescending(book => book.Count).Limit(1).FirstOrDefault();

			Console.WriteLine($"Book with min count (count - {minBook.Count}): {minBook.Name};\nBook with max count (count {maxBook.Count}): {maxBook.Name}.");

			// 4. Найдите список авторов(каждый автор должен быть в списке один раз).
			Console.WriteLine($"\nTask 4.\n");
			Console.WriteLine($"List of Authors:\n");
			bookCollection.Find(book => !String.IsNullOrEmpty(book.Author)).ToList().Select(book => book.Author)
				.Distinct().ToList().ForEach(author => Console.WriteLine(author));

			// 5. Выберите книги без авторов.

			Console.WriteLine($"\nTask 5.\n");
			Console.WriteLine($"List of Books without authors:\n");
			bookCollection.Find(book => String.IsNullOrEmpty(book.Author)).ToList()
				.ForEach(book => Console.WriteLine(book.Name));

			// 6. Увеличьте количество экземпляров каждой книги на единицу.

			Console.WriteLine($"\nTask 6.\n");

			bookCollection.Find(book => true).ToList().ForEach(x => Console.WriteLine($"Count before increment :{x.Count}"));

			bookCollection.UpdateMany(Builders<Book>.Filter.Empty,
				Builders<Book>.Update.Inc("Count", 1));

			bookCollection.Find(book => true).ToList().ForEach(x => Console.WriteLine($"Count after increment :{x.Count}"));

			// 7. Добавьте дополнительный жанр “favority” всем книгам с жанром “fantasy” 
			//(последующие запуски запроса не должны дублировать жанр “favority”).

			Console.WriteLine($"\nTask 7.\n");

			var filter = Builders<Book>.Filter.Where(x => x.Genre.Contains("fantasy") && !x.Genre.Contains("favority"));
			bookCollection.UpdateMany(filter,
			   Builders<Book>.Update.Set(x => x.Genre[1], "favority"));

			bookCollection.Find(book => true).ToList().ForEach(x => x.Genre.ForEach(Console.WriteLine));

			// 8. Удалите книги с количеством экземпляров меньше трех.

			Console.WriteLine($"\nTask 8.\n");

			var deleteFilter = Builders<Book>.Filter.Where(x => x.Count < 3);
			bookCollection.DeleteMany(deleteFilter);

			Console.WriteLine($"\nList after delete of books where count < 3:\n");
			bookCollection.Find(book => true).ToList().ForEach(x => Console.WriteLine(x.Name));

			// 9.Удалите все книги.

			Console.WriteLine($"\nTask 9.\n");

			bookCollection.DeleteMany(Builders<Book>.Filter.Empty);
			Console.WriteLine($"Count of books: {bookCollection.CountDocuments(Builders<Book>.Filter.Empty)}.\n");

			DropCollection(db, "Books");
			Console.ReadLine();
		}

		private static IMongoCollection<Book> ConnectAndCreateMongoCollection(IMongoDatabase db)
		{

			var booksCollection = db.GetCollection<Book>("Books");

			var listOfBooks = FillInBookData();

			booksCollection.InsertMany(listOfBooks);

			var collection = db.GetCollection<Book>("Books");

			return collection;
		}

		private static List<Book> FillInBookData()
		{
			var listOfBooks = new List<Book>()
			{
				new Book
				{
					Name = "Hobbit" ,
					Author = "Tolkien",
					Count = 5 ,
					Genre = new List<string>{ "fantasy" },
					Year = new DateTime(2014)
				},
				new Book
				{
					Name = "Lord of the rings" ,
					Author = "Tolkien",
					Count = 3 ,
					Genre = new List<string>{ "fantasy" },
					Year = new DateTime(2015)
				},
				new Book
				{
					Name = "Kolobok" ,
					Count = 10 ,
					Genre = new List<string>{ "kids" },
					Year = new DateTime(2000)
				},
				new Book
				{
					Name = "Repka",
					Count = 11,
					Genre = new List<string>{ "kids" },
					Year = new DateTime(2000)
				},
				new Book
				{
					Name = "Dyadya Stiopa",
					Author = "Mihalkov",
					Count = 1,
					Genre = new List<string>{ "kids" },
					Year = new DateTime(2001)
				}
			};

			return listOfBooks;
		}

		private static void DropCollection(IMongoDatabase db, string collectionName)
		{
			db.DropCollection(collectionName);
		}
	}
}
