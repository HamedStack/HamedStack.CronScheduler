namespace HamedStack.CronScheduler;

/// <summary>
/// Defines a scheduler that executes a task based on a specified CRON expression.
/// </summary>
public interface ICronScheduler
{
    /// <summary>
    /// Starts the scheduler asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    Task StartAsync(CancellationToken cancellationToken = default);
}