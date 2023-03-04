using Newtonsoft.Json;

namespace Utils.Saves
{
    public abstract class FileConfiguration
    {
        private readonly string _file;

        public FileConfiguration(string path)
        {
            _file = path;
        }
        public void SaveToFile(string file, bool formattable = true) 
        {
            var json = JsonConvert.SerializeObject(this, formattable ? Formatting.Indented : Formatting.None);
            File.WriteAllText(file, json);
        }
        public void SaveToFile(bool formattable = true)
        {
            var json = JsonConvert.SerializeObject(this, formattable ? Formatting.Indented : Formatting.None);
            File.WriteAllText(_file, json);
        }
        public void LoadFromFile(string file)
        {
            if (!File.Exists(file)) throw new FileConfigurationException("file not found");
            JsonConvert.PopulateObject(File.ReadAllText(file), this);
        }
        public void LoadFromFile()
        {
            if (!File.Exists(_file)) throw new FileConfigurationException("file not found");
            JsonConvert.PopulateObject(File.ReadAllText(_file), this);
        }
        public bool LoadFromFileIfExists(string file)
        {
            if (File.Exists(file))
            {
                JsonConvert.PopulateObject(File.ReadAllText(file), this);
                return true;
            }
            return false;
        }
        public bool LoadFromFileIfExists()
        {
            if (File.Exists(_file))
            {
                JsonConvert.PopulateObject(File.ReadAllText(_file), this);
                return true;
            }
            return false;
        }
    }
}
