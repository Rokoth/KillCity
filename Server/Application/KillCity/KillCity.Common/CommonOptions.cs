using System.Collections.Generic;

namespace KillCity.Common
{
    /// <summary>
    /// Класс хранения настроек
    /// </summary>
    public class CommonOptions
    {
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public ErrorNotifyOptions ErrorNotifyOptions { get; set; }
        public string UploadBasePath { get; set; }
    }
}