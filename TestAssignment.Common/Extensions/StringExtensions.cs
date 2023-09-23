using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using TestAssignment.Common.Entities;
using Constants = TestAssignment.Common.Config.Constants;

namespace TestAssignment.Common.Extensions;

/// <summary>
/// Provides extension methods for working with string-related datas and calculations.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Parses a string containing movie data into a list of Movie objects.
    /// </summary>
    /// <param name="data">The input string containing movie data.</param>
    /// <returns>A list of Movie objects parsed from the input data.</returns>
    public static List<Movie> ParseMovieDatas(this string data)
    {
        // Define a regular expression pattern to match the format
        string pattern = Constants.DATA_MOVIES_PATTERN;
        
        // Create a list to store parsed movie data
        List<Movie> movies = new List<Movie>();

        // Use Regex.Matches to find all matches in the input data
        MatchCollection matches = Regex.Matches(data, pattern);

        foreach (Match match in matches)
        {
            if (match.Groups.Count >= 5)
            {
                if (int.TryParse(match.Groups[1].Value, out int id) &&
                    int.TryParse(match.Groups[3].Value, out int year))
                {
                    string title = match.Groups[2].Value;
                    string genres = match.Groups[4].Value;
                    // Create a Movie object and add it to the list
                    Movie movie = new Movie
                    {
                        Id = id,
                        Title = title,
                        Year = year,
                        Genres = genres.Split('|')
                    };
                    movies.Add(movie);
                }
                else
                {
                    // parsing errors here, log and skip the invalid datas
                    Console.WriteLine($"Line '{match.Index}' is invalid, and will be ignored.");
                }
            }
            else
            {
                // Handle incomplete matches, log and skip the invalid datas
                Console.WriteLine($"Line '{match.Index}' Contains incomplete datas, and will be ignored.");
            }
        }

        return movies;
    }
}