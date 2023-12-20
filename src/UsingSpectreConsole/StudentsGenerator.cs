// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace UsingSpectreDotConsole;

/// <summary>
/// StudentsGenerator.
/// </summary>
public static class StudentsGenerator
{
    private static readonly JsonSerializerOptions _writeOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Gets the hostels.
    /// </summary>
    /// <value>
    /// The hostels.
    /// </value>
    public static string[] Hostels { get; }
        = ["Lincoln", "Louisa", "Laurent", "George", "Kennedy"];

    /// <summary>
    /// Generates the students.
    /// </summary>
    /// <returns>A collection of Student.</returns>
    public static List<Student> GenerateStudents() => new()
        {
            new Student { Id = 1, FirstName = "Julie", LastName = "Matthew", Age = 19, Hostel = Hostels[0] },
            new Student { Id = 2, FirstName = "Michael", LastName = "Taylor", Age = 23, Hostel = Hostels[3] },
            new Student { Id = 3, FirstName = "Joe", LastName = "Hardy", Age = 27, Hostel = Hostels[2] },
            new Student { Id = 4, FirstName = "Sabrina", LastName = "Azulon", Age = 18, Hostel = Hostels[3] },
            new Student { Id = 5, FirstName = "Hunter", LastName = "Cyril", Age = 19, Hostel = Hostels[4] },
        };

    /// <summary>
    /// Converts the students to json.
    /// </summary>
    /// <param name="students">The students.</param>
    /// <returns>A string.</returns>
    public static string ConvertStudentsToJson(List<Student> students) =>
        JsonSerializer.Serialize(students, _writeOptions);

    /// <summary>
    /// Streams the students from database.
    /// </summary>
    /// <returns>A collection of Student.</returns>
    public static IEnumerable<Student> StreamStudentsFromDatabase()
    {
        var students = GenerateStudents();

        for (var i = 0; i < students.Count; i++)
        {
            yield return students[i];
            Thread.Sleep(1000);
        }
    }
}
