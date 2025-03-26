using System;
using System.IO;

namespace Hoteles.UTL
{
    public static class SimpleLogger
    {
        // Define la ruta del archivo de log como estática y constante.
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app.log");

        // Constructor estático para asegurar la creación del directorio.
        static SimpleLogger()
        {
            // Asegurarse de que el directorio existe
            string directory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        // Método estático para registrar errores.
        public static void LogError(string message, Exception ex)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now} - Error: {message}");
                    if (ex != null)
                    {
                        writer.WriteLine($"Exception Message: {ex.Message}");
                        writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                    }
                }
            }
            catch
            {
                // Opcional: manejo de errores al intentar escribir en el log
            }
        }

        // Método estático para registrar advertencias.
        public static void LogWarning(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now} - Info: {message}");
                }
            }
            catch
            {
                // Opcional: manejo de errores al intentar escribir en el log
            }
        }
    }
}
