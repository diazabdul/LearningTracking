using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class CsvLoader
{
    public static List<T> LoadCsv<T>(string filePath) where T : new()
    {
        var result = new List<T>();

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return result;
        }

        var lines = File.ReadAllLines(filePath);

        if (lines.Length == 0)
        {
            Debug.LogError("CSV file is empty.");
            return result;
        }

        var headers = lines[0].Split(',');
        var properties = typeof(T).GetProperties();

        Debug.Log("CSV Headers: " + string.Join(", ", headers));

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            var obj = new T();

            for (int j = 0; j < headers.Length; j++)
            {
                var header = headers[j].Trim();
                var prop = properties.FirstOrDefault(p => p.Name.Equals(header, StringComparison.OrdinalIgnoreCase));

                if (prop != null && j < values.Length)
                {
                    var value = values[j].Trim();
                    try
                    {
                        var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                        prop.SetValue(obj, convertedValue);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error converting value '{value}' to type {prop.PropertyType} for property '{prop.Name}': {e.Message}");
                    }
                }
                else
                {
                    if (prop == null)
                    {
                        Debug.LogWarning($"Property '{header}' not found on type '{typeof(T).Name}'");
                    }
                }
            }

            result.Add(obj);
        }

        return result;
    }
}
