using System.IO;
using System.Text.Json; // permite serializarea si deserializarea obiectelor in format JSON

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
                    // citeste continutul fisierului
                    string json = File.ReadAllText(SettingsFileName);

                    // deserializare json in obiect de tip CalculatorSettings
                    return JsonSerializer.Deserialize<CalculatorSettings>(json) ?? new CalculatorSettings();
                    //unde ?? = daca rezultatul deserializarii este null, returneaza un obiect nou
                }
            }
            catch
            {
            }

            return new CalculatorSettings();
        }

        // metoda pentru salvarea setarilor in fisier
        public void Save()
        {
            try
            {
                // serializare obiect curent in format json
                string json = JsonSerializer.Serialize(this);

                // scriere json in fisier
                File.WriteAllText(SettingsFileName, json);
            }
            catch
            {
            }
        }
    }
}
