using System.IO;
using System.Text.Json;

namespace calculator_app
{
    public class CalculatorSettings
    {
        private const string SettingsFileName = "calculator_settings.json";
        
        public bool DigitGroupingEnabled { get; set; } = false;
        
        public static CalculatorSettings Load()
        {
            try
            {
                if (File.Exists(SettingsFileName))
                {
                    string json = File.ReadAllText(SettingsFileName);
                    return JsonSerializer.Deserialize<CalculatorSettings>(json) ?? new CalculatorSettings();
                }
            }
            catch
            {
                // Ignore errors and return default settings
            }
            
            return new CalculatorSettings();
        }
        
        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(this);
                File.WriteAllText(SettingsFileName, json);
            }
            catch
            {
                // Ignore save errors
            }
        }
    }
}