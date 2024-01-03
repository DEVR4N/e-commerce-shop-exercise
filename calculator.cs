using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string filePath = "DEV-data.txt";

        do
        {
            Console.Write("Enter the target year: ");
            if (int.TryParse(Console.ReadLine(), out int targetYear))
            {
                try
                {
                    List<decimal> earnings = ReadEarningsFromFile(filePath, targetYear);

                    if (earnings.Count > 0)
                    {
                        decimal standardDeviation = CalcStandardDeviation(earnings);
                        Console.WriteLine($"Standard Deviation for {targetYear}: {standardDeviation:F2}");
                    }
                    else
                        Console.WriteLine($"No earnings data found for the year {targetYear}.");
                }
                catch (Exception ex)    
                {            
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

            }
            else
                Console.WriteLine("Invalid input for the target year.");

            Console.Write("Do you want to repeat? ('Y' / 'N') ");
        } while (Console.ReadLine()?.Trim().ToUpper() == "Y");
    }

    static List<decimal> ReadEarningsFromFile(string filePath, int targetYear)
    {
        List<decimal> earnings = new List<decimal>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] parts = line.Split("##");

                if (parts.Length == 2)
                {
                    string[] dateParts = parts[0].Split('/');
                    if (dateParts.Length == 3 && int.TryParse(dateParts[2], out int year))
                    {
                        if (year == targetYear)
                        {
                            if (decimal.TryParse(parts[1], out decimal revenue))
                                earnings.Add(revenue);
                        }
                    }
                }
            }
        }

        return earnings;
    }


     static decimal CalcStandardDeviation(List<decimal> data)
    {
        int n = data.Count;
        decimal mean = 0m;

        foreach (decimal value in data)
            mean += value;

        mean /= n;

        decimal sumSquaredDeviations = 0m;
        foreach (decimal value in data)
            sumSquaredDeviations += (value - mean) * (value - mean);

        decimal variance = sumSquaredDeviations / n;
        decimal standardDeviation = (decimal)Math.Sqrt((double)variance);

        return standardDeviation;
    } 
}
