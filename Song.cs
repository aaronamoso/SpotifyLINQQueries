using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace SpotifyLINQQueries
{
    public class Song
    {
        // Properties matching the columns in the CSV
        [Name("track_name")]
        public string Title { get; set; }

        [Name("artist(s)_name")]
        public string Artist { get; set; }

        [Name("streams")]
        public long Streams { get; set; } // since we are dealing w a lot of numsc

        [Name("released_year")]
        public int Year { get; set; }

        [Name("in_spotify_playlists")]
        public string Genre { get; set; } // Adjust if Genre is represented differently

        // Method to load songs from the CSV
        public static List<Song> LoadSongs(string filePath)
        {
            var songs = new List<Song>();

            try
            {
                var lines = File.ReadAllLines(filePath).Skip(1); // Skip header row

                foreach (var line in lines) // Iterate each line of the excel file
                {
                    var columns = line.Split(',');

                    // the song object
                    songs.Add(new Song
                    {
                        Title = columns[0], // Where each category is located in the csv
                        Artist = columns[1],
                        Streams = int.Parse(columns[8]), // Parsing to avoid error for NaN
                        Year = int.Parse(columns[3])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading songs: {ex.Message}");
            }

            return songs;
        }
    }
}
