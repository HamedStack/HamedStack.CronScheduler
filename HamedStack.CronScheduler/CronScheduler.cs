using NCrontab;

namespace HamedStack.CronScheduler;

/// <summary>
/// Represents a scheduler that triggers an action based on a CRON expression.
/// </summary>
public class CronScheduler : ICronScheduler
{
    private readonly string _cronExpression;
    private readonly Action _taskToRun;

    /// <summary>
    /// Initializes a new instance of the <see cref="CronScheduler"/> class.
    /// </summary>
    /// <param name="cronExpression">The CRON expression that defines when the task should be executed.</param>
    /// <param name="taskToRun">The action to execute on each occurrence defined by the CRON expression.</param>
    public CronScheduler(string cronExpression, Action taskToRun)
    {
        _cronExpression = cronExpression;
        _taskToRun = taskToRun;
    }

    /// <summary>
    /// Starts executing the action based on the specified CRON schedule.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation of executing the scheduled action.</returns>
    /// <remarks>
    /// The method calculates the next occurrence based on the current time and the specified CRON expression.
    /// It then waits asynchronously until the next scheduled time before executing the action.
    /// If the execution is delayed and the calculated wait time is negative, the action is executed immediately.
    /// This process repeats until the cancellation token is triggered.
    /// </remarks>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var schedule = CrontabSchedule.Parse(_cronExpression);

        while (!cancellationToken.IsCancellationRequested)
        {
            var nextRun = schedule.GetNextOccurrence(DateTime.Now);
            var delay = nextRun - DateTime.Now;

            if (delay.TotalMilliseconds > 0)
            {
                await Task.Delay(delay, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested) continue;
            try
            {
                _taskToRun();
            }
            catch
            {
                // Exceptions are ignored to ensure continuous operation.
                // Consider logging or handling exceptions as necessary.
            }
        }
    }
}
