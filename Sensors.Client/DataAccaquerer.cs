using System.Linq;

public class DataEntry {
    public long Temperature { get; set;}
    public long Pressure { get; set;}
    public long Humidity { get; set;}
    public long CO { get; set;}
    public long NO2 { get; set;}
    public long SO2 { get; set;}
}

public interface IDataAccaquerer
{
    Task<DataEntry> GetEntryAsync();
}

public class FakeDataAccaquerer : IDataAccaquerer
{
    private readonly DateTime _dateTime;
    private readonly string _fakeDataPath;
    public string[]? data;
    public FakeDataAccaquerer(DateTime dateTime, string fakeDataPath)
    {
        _dateTime = dateTime;
        _fakeDataPath = fakeDataPath;
    }
    public async Task<DataEntry> GetEntryAsync()
    {
        if (data is null)
            data = (await System.IO.File.ReadAllLinesAsync(_fakeDataPath)).Skip(1).ToArray();
        var index = (int)(DateTime.Now - _dateTime).TotalSeconds % data.Length;
        var entry = data[index].Split(',', StringSplitOptions.None);
        return new DataEntry
        {
            Temperature = safeIntParse(entry[0]),
            Pressure = safeIntParse(entry[1]),
            Humidity = safeIntParse(entry[2]),
            CO = safeIntParse(entry[3]),
            NO2 = safeIntParse(entry[4]),
            SO2 = safeIntParse(entry[5])
        };
        long safeIntParse(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return long.MinValue;
            return long.TryParse(s, out long a) ? a : long.MinValue;
        }
    }

}