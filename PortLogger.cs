1.git checkout - b feature / port - matrix1.git checkout - b feature / port - matrix
user @DESKTOP-73K8GJ9 MINGW64 ~/Desktop/практика Т/Pracktuchna №2_Матвійчук Денис (feature/student-lab-grades)
$ git checkout -b feature/port-matrix
Switched to a new branch 'feature/port-matrix'

user @DESKTOP-73K8GJ9 MINGW64 ~/Desktop/практика Т/Pracktuchna №2_Матвійчук Денис (feature/port-matrix)
$ git checkout -b feature/stringbuilder-logger
Switched to a new branch 'feature/stringbuilder-logger'

user @DESKTOP-73K8GJ9 MINGW64 ~/Desktop/практика Т/Pracktuchna №2_Матвійчук Денис (feature/stringbuilder-logger)
$
$ using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Praktychna1
{
    public class PortLogger
    {
        // Поле для накопичення логів
        private readonly StringBuilder _logHistory = new StringBuilder();
        // Метод для запису операції
        public void Log(int portNumber, string operation, string status)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // Використовуємо AppendFormat замість конкатенації (+)
            _logHistory.AppendFormat("[{0}] Порт №{1}: {2} | Статус: {3}\n",
                timestamp, portNumber, operation, status);

        }
        // Метод для отримання всього логу
        public string GetFullLog()
        {
            if (_logHistory.Length == 0)
                return "Історія операцій порожня.";

            return _logHistory.ToString();
        }
        // Очищення логів
        public void Clear() => _logHistory.Clear();
    }
}
Оновлення коду в PortMatrix.cs:
public string GetMatrixStatus()
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("=== Поточний стан апаратних портів ===");
    for (int i = 0; i < 16; i++)
    {
        for (int j = 0; j < 16; j++)
        {
            // Використовуємо Append для формування рядка матриці
            sb.Append(_matrix[i, j].IsOpen ? "[O]" : "[.]");
        }
        sb.AppendLine(); // Перехід на новий

