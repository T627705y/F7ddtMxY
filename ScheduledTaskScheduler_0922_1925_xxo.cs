// 代码生成时间: 2025-09-22 19:25:18
 * at a specified interval.
# 扩展功能模块
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MauiApp
{
# TODO: 优化性能
    public class ScheduledTaskScheduler
    {
        private readonly ConcurrentDictionary<string, (Action Task, CancellationTokenSource CancellationTokenSource, TimeSpan Interval)> tasks =
            new ConcurrentDictionary<string, (Action, CancellationTokenSource, TimeSpan)>();
# 添加错误处理
        private readonly List<CancellationTokenSource> allCancellationTokens = new List<CancellationTokenSource>();

        /// <summary>
        /// Schedules a task to run at a specified interval.
        /// </summary>
# TODO: 优化性能
        /// <param name="taskName">The name of the task.</param>
# 扩展功能模块
        /// <param name="task">The task to be executed.</param>
        /// <param name="interval">The interval at which the task will be executed.</param>
        public void ScheduleTask(string taskName, Action task, TimeSpan interval)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                throw new ArgumentException("Task name cannot be null or whitespace.", nameof(taskName));
            }

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than zero.");
            }

            var cts = new CancellationTokenSource();
            var taskInfo = (task, cts, interval);
            tasks[taskName] = taskInfo;
            allCancellationTokens.Add(cts);

            Task.Run(async () =>
            {
                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        task.Invoke();
                        await Task.Delay(interval, cts.Token);
                    }
                }
# 扩展功能模块
                catch (OperationCanceledException)
                {
                    // Task was cancelled
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Task {taskName} failed with exception: {ex.Message}");
                }
# NOTE: 重要实现细节
            }, cts.Token);
        }

        /// <summary>
        /// Stops a scheduled task.
# 改进用户体验
        /// </summary>
# 添加错误处理
        /// <param name="taskName">The name of the task to stop.</param>
        public void StopTask(string taskName)
        {
            if (!tasks.TryRemove(taskName, out var taskInfo))
            {
# 改进用户体验
                Debug.WriteLine($"Task {taskName} not found.");
# NOTE: 重要实现细节
                return;
            }

            taskInfo.CancellationTokenSource.Cancel();
            allCancellationTokens.Remove(taskInfo.CancellationTokenSource);
        }

        /// <summary>
# 增强安全性
        /// Stops all scheduled tasks.
        /// </summary>
        public void StopAllTasks()
        {
            foreach (var cts in allCancellationTokens)
            {
                cts.Cancel();
            }
            tasks.Clear();
            allCancellationTokens.Clear();
        }
    }
}
