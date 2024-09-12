using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

public static class CsvUtility
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<T> Read<T>(string file) where T : new()
    {
        var list = new List<T>();
        TextAsset data = Resources.Load(file) as TextAsset;

        if (data == null)
        {
            Debug.LogError($"File not found: {file}");
            return list;
        }

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var obj = new T();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j].Trim(TRIM_CHARS).Replace("\\", "");
                var prop = properties.FirstOrDefault(p => p.Name.Equals(header[j], StringComparison.OrdinalIgnoreCase));
                if (prop != null)
                {
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
            }
            list.Add(obj);
        }
        return list;
    }

    public static void Save<T>(string filePath, List<T> data)
    {
        if (data == null || data.Count == 0)
        {
            Debug.LogError("No data to save.");
            return;
        }

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var headers = properties.Select(p => p.Name).ToArray();

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(string.Join(",", headers));

            foreach (var item in data)
            {
                if (item == null)
                {
                    Debug.LogError("Item in data list is null.");
                    continue;
                }

                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item, null);
                    return value != null ? value.ToString() : string.Empty;
                }).ToArray();

                writer.WriteLine(string.Join(",", values));
            }
        }

        Debug.Log("Data saved to " + filePath);
    }
}
