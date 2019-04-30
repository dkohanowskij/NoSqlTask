using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace NoSQL
{
	/// <summary>
	/// Class contains Book data
	/// </summary>
	public class Book
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public ObjectId Id { get; set; }
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the author.
		/// </summary>
		/// <value>
		/// The author.
		/// </value>
		public string Author { get; set; }

		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int Count { get; set; }

		/// <summary>
		/// Gets or sets the genre.
		/// </summary>
		/// <value>
		/// The genre.
		/// </value>
		public List<string> Genre { get; set; }

		/// <summary>
		/// Gets or sets the year.
		/// </summary>
		/// <value>
		/// The year.
		/// </value>
		public DateTime Year { get; set; }
	}
}
