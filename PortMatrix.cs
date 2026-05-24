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
        sb.AppendLine(); // Перехід на новий рядок
    }

    return sb.ToString();
}
