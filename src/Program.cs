using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Caffeine.Models;

args = args ?? new string[0];

string filePath;
if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
{
    filePath = args[0];
}
else
{
    filePath = @"C:\my-coding-projects\yaml\caffeine.yaml";
}

try
{
    string yaml = LoadFile(filePath);
    IDeserializer deserializer = new DeserializerBuilder()
        .WithNamingConvention(PascalCaseNamingConvention.Instance)
        .Build();
    Dictionary<string, DailyTotal> dailyTotals = deserializer.Deserialize<Dictionary<string, DailyTotal>>(yaml);
    CalculateAverage(dailyTotals);
}
catch (FileNotFoundException)
{
    Console.WriteLine($"Error: Could not find file '{filePath}'");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

void CalculateAverage(Dictionary<string, DailyTotal> dailyTotals)
{
    var orderedData = dailyTotals
        .Select(kv => new
        {
            Date = DateTime.Parse(kv.Key),
            Total = kv.Value.Total
        })
        .OrderBy(item => item.Date)
        .ToList();

    var lastFiveDays = orderedData.TakeLast(5).ToList();
    var today = orderedData.Last();

    double average = lastFiveDays.Average(d => d.Total);

    Console.WriteLine($"5-Day Average: {average:F2}");
    Console.WriteLine($"Today (or last time): {today.Total}");
}

string LoadFile(string fileName)
{
    if (string.IsNullOrEmpty(fileName))
    {
        throw new ArgumentNullException(nameof(fileName));
    }

    if (!File.Exists(fileName))
    {
        throw new FileNotFoundException($"File not found: {fileName}");
    }

    return File.ReadAllText(fileName);
}
