using Microsoft.Extensions.Logging;

namespace TemplateWebApiNet8.Logging
{
    public class CustomLogger : ILogger
    {
        private readonly string loggerName;
        private readonly CustomLoggerProviderConfiguration configuration;
        public static bool Arquivo { get; set; } = false;
        public CustomLogger(string loggerName, CustomLoggerProviderConfiguration configuration)
        {
            this.loggerName = loggerName;
            this.configuration = configuration;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"Log de Execução {logLevel}: {eventId} - {formatter(state, exception)}";
            if (Arquivo)
                SalvarLogNoArquivo(mensagem);
            else
                Console.WriteLine(mensagem);
        }

        private void SalvarLogNoArquivo(string mensagem)
        {
            string caminhoArquivo = Environment.CurrentDirectory + @$"\LOG-{DateTime.Now:yyyy-MM-dd}.txt";
            if (!File.Exists(caminhoArquivo))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
                File.Create(caminhoArquivo).Dispose();
            }

            using StreamWriter stream = new(caminhoArquivo, true);
            stream.WriteLine(mensagem);
            stream.Flush();

        }
    }
}
