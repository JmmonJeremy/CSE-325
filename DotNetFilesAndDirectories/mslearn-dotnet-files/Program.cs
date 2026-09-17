using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

var storeSalesFiles = FindStoreSaleFiles("stores");

foreach (var file in storeSalesFiles)
{
    Console.WriteLine(file);
}

IEnumerable<string> FindStoreSaleFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        // The file name will contain the full path, so only check the end of it
        if (file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

var salesSummary = GetSalesSummaryReport(salesFiles);

File.WriteAllText(Path.Combine(salesTotalDir, "summary.txt"), salesSummary);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        // The file name will contain the full path, so only check the end of it
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

string GetSalesSummaryReport(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    string filename = string.Empty;
    string fileList = string.Empty;
    foreach (var file in salesFiles)    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        if (data != null && data.Total != 0)
        {
            filename = $"{Path.GetRelativePath(currentDirectory, file)}";
            fileList += $" {filename}: {data.Total:C}\n";
        }
        salesTotal += data?.Total ?? 0;
    }

    return $"Sales Summary\n-----------------------------------\nTotal Sales: {salesTotal:C}\n\nDetails:\n{fileList}";
}

record SalesData (double Total);



