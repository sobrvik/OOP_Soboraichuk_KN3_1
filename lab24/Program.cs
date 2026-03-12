using System;
using System.Collections.Generic;

public interface INumericOperationStrategy
{
    string OperationName { get; }
    double Execute(double value);
}

public class SquareOperationStrategy : INumericOperationStrategy
{
    public string OperationName => "Square";

    public double Execute(double value)
    {
        return value * value;
    }
}

public class CubeOperationStrategy : INumericOperationStrategy
{
    public string OperationName => "Cube";

    public double Execute(double value)
    {
        return value * value * value;
    }
}

public class SquareRootOperationStrategy : INumericOperationStrategy
{
    public string OperationName => "Square Root";

    public double Execute(double value)
    {
        if (value < 0)
            throw new ArgumentException("Неможливо знайти корінь з від'ємного числа");

        return Math.Sqrt(value);
    }
}

public class NumericProcessor
{
    private INumericOperationStrategy _strategy;

    public NumericProcessor(INumericOperationStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(INumericOperationStrategy strategy)
    {
        _strategy = strategy;
    }

    public double Process(double input)
    {
        return _strategy.Execute(input);
    }

    public string GetOperationName()
    {
        return _strategy.OperationName;
    }
}

public class ResultPublisher
{
    public event Action<double, string>? ResultCalculated;

    public void PublishResult(double result, string operationName)
    {
        ResultCalculated?.Invoke(result, operationName);
    }
}

public class ConsoleLoggerObserver
{
    public void OnResultCalculated(double result, string operationName)
    {
        Console.WriteLine($"Операція: {operationName}, результат: {result}");
    }
}

public class HistoryLoggerObserver
{
    public List<string> History = new();

    public void OnResultCalculated(double result, string operationName)
    {
        History.Add($"{operationName}: {result}");
    }
}

public class ThresholdNotifierObserver
{
    private double threshold;

    public ThresholdNotifierObserver(double threshold)
    {
        this.threshold = threshold;
    }

    public void OnResultCalculated(double result, string operationName)
    {
        if (result > threshold)
        {
            Console.WriteLine($"Увага! Результат {result} перевищує поріг {threshold}");
        }
    }
}

internal class NewBaseType
{
    static void Main()
    {
        var processor = new NumericProcessor(new SquareOperationStrategy());
        var publisher = new ResultPublisher();

        var consoleLogger = new ConsoleLoggerObserver();
        var historyLogger = new HistoryLoggerObserver();
        var thresholdNotifier = new ThresholdNotifierObserver(20);

        publisher.ResultCalculated += consoleLogger.OnResultCalculated;
        publisher.ResultCalculated += historyLogger.OnResultCalculated;
        publisher.ResultCalculated += thresholdNotifier.OnResultCalculated;

        double[] numbers = { 4, 3, 25 };

        processor.SetStrategy(new SquareOperationStrategy());
        Process(numbers[0], processor, publisher);

        processor.SetStrategy(new CubeOperationStrategy());
        Process(numbers[1], processor, publisher);

        processor.SetStrategy(new SquareRootOperationStrategy());
        Process(numbers[2], processor, publisher);

        Console.WriteLine("\nІсторія:");

        foreach (var item in historyLogger.History)
        {
            Console.WriteLine(item);
        }
    }

    static void Process(double input, NumericProcessor processor, ResultPublisher publisher)
    {
        double result = processor.Process(input);
        publisher.PublishResult(result, processor.GetOperationName());
    }
}
