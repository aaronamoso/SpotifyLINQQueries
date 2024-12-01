using System;
using System.Collections.Generic;
using System.Linq;

namespace SpotifyLINQQueries
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the CSV file
            string filePath = "res/spotify-2023.csv"; // Insert into local bin for it to work, c# happens to delete them

            // Load songs
            var songs = Song.LoadSongs(filePath);

            if (songs.Count == 0)
            {
                Console.WriteLine("No songs loaded. Exiting.");
                return;
            }

            // Query 1: Filtering and Ordering
            // Filter songs and order them by stream count in descending
            var filteredAndOrdered = from song in songs
                                     where song.Streams > 1000000 // Filters songs with streams that are more than 1M
                                     orderby song.Streams descending // Ordering
                                     select song;
            Console.WriteLine("Query 1: Songs with more than 1M streams, ordered by streams:");

            foreach (var song in filteredAndOrdered)
                Console.WriteLine($"{song.Title} by {song.Artist} - {song.Streams} streams"); // Concat




            // Query 2: Grouping and Aggregation (Grouped by Release Year)
            // This query groups songs by year and take ave per yr
            var groupedAndAggregated = from song in songs
                                       group song by song.Year into yearGroup // Songs grouping
                                       select new
                                       {
                                           Year = yearGroup.Key, 
                                           TotalStreams = yearGroup.Sum(s => s.Streams), // sums
                                           AvgStreams = yearGroup.Average(s => s.Streams) // ave stream
                                       };
            Console.WriteLine("\nQuery 2: Total and average streams by release year:");
            foreach (var group in groupedAndAggregated)
                Console.WriteLine($"Year {group.Year}: Total = {group.TotalStreams}, Average = {group.AvgStreams}");






            // Query 3: Projection and Utility Methods
            // Project top 5 songs query
            var projected = songs
                .Where(song => song.Streams > 500000) // Filter songs with more than 500000
                .Select(song => new { song.Title, song.Artist }) // Takes the Title and Artist
                .Take(5); // Since we are taking the top 5, we take the first five on the list
            Console.WriteLine("\nQuery 3: Top 5 songs with more than 500K streams:");
            foreach (var song in projected)
                Console.WriteLine($"{song.Title} by {song.Artist}");








            // Query 4: Filtering, Ordering, and Aggregate Method (Highest Streamed Song)
            // Query to find the hghest stream in 2020
            var highestStreams = songs
                .Where(song => song.Year >= 2020) // Filter from 2020
                .OrderByDescending(song => song.Streams) // Orders them by desc
                .Aggregate((max, next) => next.Streams > max.Streams ? next : max); //  This line finds the highest/max stream
            Console.WriteLine($"\nQuery 4: Highest streamed song from 2020 onwards: {highestStreams.Title} by {highestStreams.Artist}");











            // Query 5: Grouping, Ordering, and Projection with Anonymous Type (Grouped by Artist)
            // Groups songs by artist and counts the number of songs per artist.
            var artistOrderProjection = from song in songs // location
                                        group song by song.Artist into artistGroup //grouping
                                        orderby artistGroup.Key // Orders by artist
                                        select new
                                        {
                                            Artist = artistGroup.Key, // group key is that artist name
                                            SongCount = artistGroup.Count() // counts # of songs per artist
                                        };
            Console.WriteLine("\nQuery 5: Song count by artist (ordered):");
            foreach (var artist in artistOrderProjection)
                Console.WriteLine($"{artist.Artist}: {artist.SongCount} songs");












            // Query 6: Grouping, Filtering, Ordering, Projection with Anonymous Type, and Aggregation (Artists with High Total Streams)
            // Groups songs by artist and filters those > 10M total streams and organize by artist name
            var complexQuery = from song in songs
                               group song by song.Artist into artistGroup
                               where artistGroup.Sum(s => s.Streams) > 10000000 // artist who has over 10m streams
                               orderby artistGroup.Key
                               select new
                               {
                                   Artist = artistGroup.Key,
                                   TotalStreams = artistGroup.Sum(s => s.Streams), // Sums artist's streams
                                   SongCount = artistGroup.Count() // artist song count
                               };
            Console.WriteLine("\nQuery 6: Artists with more than 10M total streams:");
            foreach (var result in complexQuery)
                Console.WriteLine($"{result.Artist}: {result.TotalStreams} streams across {result.SongCount} songs");
        }
    }
}
