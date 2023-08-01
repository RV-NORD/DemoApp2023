// See https://aka.ms/new-console-template for more information
using DemoApp.DAL.Entityes;
using System.Reflection;

Console.WriteLine("Hello, World!");
Worker w1 = new Worker { Id = 1, SurName = "Иванов", FirstName = "Иван", LastName = "Иванович", BirthDay = new DateOnly(2000, 10, 10), Pol = true };
Worker w2 = new Worker { Id = 1, SurName = "Ивановj", FirstName = "Иван5", LastName = "Иванович", BirthDay = new DateOnly(2000, 10, 10), Pol = true };

var type = w1.GetType();
Console.WriteLine($"{w1.Id} - {w1.SurName} - {w1.FirstName} - {w1.LastName} - {w1.BirthDay} - {w1.Pol}");
foreach (PropertyInfo prop in type.GetProperties().Where(p => p.CanWrite))
{
    Console.WriteLine($"оба объекта - {prop.Name} - {prop.PropertyType.Name} - {prop.GetValue(w1)} - {prop.GetValue(w2)}");
    if (prop is null || !prop.CanWrite || (prop.GetValue(w1).ToString() == prop.GetValue(w2).ToString())) continue;
    //if (prop.PropertyType.Name == "DateTime" && DateOnly.FromDateTime((DateTime)prop.GetValue(old_worker)) == DateOnly.FromDateTime((DateTime)prop.GetValue(worker))) continue;
    Console.WriteLine($"изменено - {prop.Name}");
    prop.SetValue(w1, prop.GetValue(w2));
}
Console.WriteLine($"{w1.Id} - {w1.SurName} - {w1.FirstName} - {w1.LastName} - {w1.BirthDay} - {w1.Pol}");
Console.WriteLine("нажмите эникей");
Console.ReadKey();