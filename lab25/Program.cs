using System;
using System.IO;

namespace lab25
{

// ---------------- LOGGER ----------------

interface ILogger
{
    void Log(string message);
}

class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine("[Console] " + message);
    }
}

class FileLogger : ILogger
{
    private string path = "log.txt";

    public void Log(string message)
    {
        File.AppendAllText(path, message + Environment.NewLine);
        Console.WriteLine("[FileLogger] записано у файл");
    }
}

// ---------------- FACTORY METHOD ----------------

abstract class LoggerFactory
{
    public abstract ILogger CreateLogger();
}

class ConsoleLoggerFactory : LoggerFactory
{
    public override ILogger CreateLogger()
    {
        return new ConsoleLogger();
    }
}

class FileLoggerFactory : LoggerFactory
{
    public override ILogger CreateLogger()
    {
        return new FileLogger();
    }
}

// ---------------- SINGLETON ----------------

class LoggerManager
{
    private static LoggerManager? instance;

    private LoggerFactory? factory;

    private LoggerManager() {}

    public static LoggerManager Instance
    {
        get
        {
            if (instance == null)
                instance = new LoggerManager();
            return instance;
        }
    }

    public void SetFactory(LoggerFactory factory)
    {
        this.factory = factory;
    }

    public ILogger GetLogger()
    {
        return factory!.CreateLogger();
    }
}

// ---------------- STRATEGY ----------------

interface IDataProcessorStrategy
{
    string Process(string data);
}

class EncryptDataStrategy : IDataProcessorStrategy
{
    public string Process(string data)
    {
        return "Encrypted(" + data + ")";
    }
}

class CompressDataStrategy : IDataProcessorStrategy
{
    public string Process(string data)
    {
        return "Compressed(" + data + ")";
    }
}

// ---------------- CONTEXT ----------------

class DataContext
{
    private IDataProcessorStrategy strategy;

    public DataContext(IDataProcessorStrategy strategy)
    {
        this.strategy = strategy;
    }

    public void SetStrategy(IDataProcessorStrategy strategy)
    {
        this.strategy = strategy;
    }

    public string ProcessData(string data)
    {
        return strategy.Process(data);
    }
}

// ---------------- OBSERVER ----------------

class DataPublisher
{
    public event Action<string>? DataProcessed;

    public void PublishDataProcessed(string data)
    {
        DataProcessed?.Invoke(data);
    }
}

class ProcessingLoggerObserver
{
    public void Subscribe(DataPublisher publisher)
    {
        publisher.DataProcessed += OnDataProcessed;
    }

    private void OnDataProcessed(string data)
    {
        var logger = LoggerManager.Instance.GetLogger();
        logger.Log("Observer отримав дані: " + data);
    }
}

// ---------------- MAIN ----------------

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===== СЦЕНАРІЙ 1: Повна інтеграція =====");

        LoggerManager.Instance.SetFactory(new ConsoleLoggerFactory());

        var context = new DataContext(new EncryptDataStrategy());

        var publisher = new DataPublisher();

        var observer = new ProcessingLoggerObserver();
        observer.Subscribe(publisher);

        string result = context.ProcessData("Hello");

        var logger = LoggerManager.Instance.GetLogger();
        logger.Log("Результат обробки: " + result);

        publisher.PublishDataProcessed(result);


        Console.WriteLine("\n===== СЦЕНАРІЙ 2: Зміна логера =====");

        LoggerManager.Instance.SetFactory(new FileLoggerFactory());

        result = context.ProcessData("Hello Again");

        logger = LoggerManager.Instance.GetLogger();
        logger.Log("Результат обробки: " + result);

        publisher.PublishDataProcessed(result);


        Console.WriteLine("\n===== СЦЕНАРІЙ 3: Зміна стратегії =====");

        context.SetStrategy(new CompressDataStrategy());

        result = context.ProcessData("Hello Strategy");

        logger = LoggerManager.Instance.GetLogger();
        logger.Log("Результат обробки: " + result);

        publisher.PublishDataProcessed(result);
    }
}

}