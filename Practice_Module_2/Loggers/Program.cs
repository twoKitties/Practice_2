using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggers
{
        class Program
    {
        static void Main(string[] args)
        {
            string message = "ERROR";
            MessageLogger dailyConsoleLogger = new MessageLogger(new DailyLogWriter(new ConsoleLogWritter()));
            dailyConsoleLogger.Write(message);
            MessageLogger fileLogger = new MessageLogger(new FileLogWritter());
            fileLogger.Write(message);
            MessageLogger dailyFileLogger = new MessageLogger(new DailyLogWriter(new FileLogWritter()));
            dailyFileLogger.Write(message);
            MessageLogger multiLogger = new MessageLogger(new ConsoleFileWriter(new ConsoleLogWritter(), new FileLogWritter()));
            multiLogger.Write(message);
            MessageLogger dailyMultiLogger = new MessageLogger(new DailyLogWriter(new ConsoleFileWriter(new ConsoleLogWritter(), new FileLogWritter())));
            dailyMultiLogger.Write(message);
        }
    }

    class MessageLogger
    {
        private ILogWriter _writer;

        public MessageLogger(ILogWriter writer)
        {
            _writer = writer;
        }

        public void Write(string message)
        {
            _writer.WriteError(message);
        }
    }

    interface ILogWriter
    {
        void WriteError(string message);
    }

    class ConsoleLogWritter : ILogWriter
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogWriter
    {
        public void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class ConsoleFileWriter : ILogWriter
    {
        private ConsoleLogWritter _consoleWriter;
        private FileLogWritter _fileWriter;

        public ConsoleFileWriter(ConsoleLogWritter consoleWriter, FileLogWritter fileWriter)
        {
            _consoleWriter = consoleWriter;
            _fileWriter = fileWriter;
        }

        public void WriteError(string message)
        {
            _consoleWriter.WriteError(message);
            _fileWriter.WriteError(message);
        }
    }

    class DailyLogWriter : ILogWriter
    {
        private ILogWriter _writer;

        public DailyLogWriter(ILogWriter writer)
        {
            _writer = writer;
        }

        public void WriteError(string message)
        {
            if ((int)DateTime.Now.DayOfWeek % 2 != 0)
            {
                _writer.WriteError(message);
            }
        }
    }
}
