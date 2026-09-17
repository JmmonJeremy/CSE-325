// See https://aka.ms/new-console-template for more information
var time = DateTime.Now;
var christmas = new DateTime(DateTime.Now.Year, 12, 25);
var daysUntilChristmas = (christmas.Date - time.Date).Days;
Console.WriteLine($"Hello, World!");
Console.WriteLine($"The current time is {time}");
Console.WriteLine($"The number of days until the next Christmas is {daysUntilChristmas}");