/*
    Tips:
    Wanna create an array? Better to stick with Array list ( List<T> )
*/
// // Array list ( List<T> )
// var names = new List<string> { "Scott", "Ana", "Felipe" };
// // Add new item to array
// names.Add("David");
// names.Add("Maria");
// names.Add("Lory");
// // Recommend for displaying item of array
// foreach (var name in names)
// {
//     // String Interpolation
//     Console.WriteLine($"Hello, {name.ToUpper()}");
// }
// // Optional
// for (int i = 0; i < names.Count; i++)
// {
//     Console.WriteLine($"Hello, {names[i].ToUpper()}");
// }

// // Array of string
// // This array is fix
// var names = new string[] { "Scott", "Ana", "Felipe" };
// // Add new item to array
// names = [..names, "David", "Maria", "Lory"];
// foreach (var name in names)
// {
//     Console.WriteLine($"Hello, {name.ToUpper()}");
// }

// // Indexing
// // Set include and not include number of array
// // names[2] is included number and names[5] is not included number
// foreach (var name in names[2..5])
// {
//     Console.WriteLine($"Hello, {name.ToUpper()}");
// }
// Console.WriteLine(names[0]);
// Console.WriteLine(names[names.Count - 1]); // Old way to get last index of array
// Console.WriteLine(names[^1]); // New way to get last index of array

// // LINQ = Language Integrated Query
// // Specify the data source
// List<int> scores = [ 97, 92, 81, 60 ];

// // Define the query expression
// IEnumerable<int> scoreQuery =
//     from score in scores
//     where score > 80
//     select score;

// // Execute the query
// foreach (var i in scoreQuery)
// {
//     Console.WriteLine(i + " ");
// }

// List<int> scores = [ 97, 92, 81, 60 ];
// IEnumerable<string> scoreQuery =
//     from score in scores
//     where score > 80
//     select $"The score is {score}";
// // int count = scoreQuery.Count();
// // Console.WriteLine($"Count = {count}");
// foreach (var str in scoreQuery)
// {
//     Console.WriteLine(str);
// }

// // LINQ Method syntax vs Query
// // Specify the data source
// List<int> scores = [ 97, 92, 81, 60 ];

// // Define the query expression
// IEnumerable<int> scoreQuery =
//     from score in scores
//     where score > 80
//     orderby score descending
//     select score;

// // Method syntax
// var scoreQuery2 = scores.Where(s => s > 80).OrderByDescending(s => s);

// // Collection expression (Could replace ToList() method)
// List<int> myScores = [.. scoreQuery2]; // List<int> myScores = scoreQuery.ToList();

// // Execute the query
// foreach (var score in myScores)
// {
//     Console.WriteLine(score);
// }

// OOP
Console.WriteLine("Hello, OOP!");

var p1 = new Person("John", "Silver", new DateOnly(1990, 1, 1));
var p2 = new Person("Uni", "Sakawa", new DateOnly(1986, 2, 2));

p1.Pets.Add(new Dog("Tonny"));
p1.Pets.Add(new Dog("Jackie"));

p2.Pets.Add(new Cat("Snowie"));
// p2.Pets.Add(new Cat("Beyonce"));

List<Person> people = [p1 ,p2];

Console.WriteLine($"There are {people.Count} people in the list.");

foreach (var person in people)
{
    int countPet = person.Pets.Count;

    Console.WriteLine($"Hello, Mr/Mrs {person}.");
    Console.WriteLine($"Your birthdate is {person.Birthdate}. Is it correct?");
    Console.WriteLine($"Also, you have {(countPet > 1 ? countPet + " pets" : countPet + " pet")}.");

    foreach (var pet in person.Pets)
    {
        Console.WriteLine(pet);
    }
}

public class Person(string firstname, string lastname, DateOnly birthdate)
{
    public string Firstname { get; } = firstname; // new way to create getter and setter

    public string Lastname { get; } = lastname;

    public DateOnly Birthdate { get; set; } = birthdate;

    public List<Pet> Pets { get; } = [];

    public override string ToString()
    {
        return $"{Firstname} {Lastname}.";
    }
}

public abstract class Pet(string name)
{
    public string Name { get; } = name;

    public abstract string MakeNoise();

    public override string ToString()
    {
        return $"It is {Name} and it is a {GetType().Name} and it {MakeNoise()}";
    }
}

public class Cat(string name) : Pet(name)
{
    public override string MakeNoise() => "Meow";
}

public class Dog(string name) : Pet(name)
{
    public override string MakeNoise() => "Woah";
}
