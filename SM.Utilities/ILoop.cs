using System;
using System.Threading;

namespace SM.Utilities
{
    public delegate void ValueMethodInvoker<T>(T param);

    /// <summary>
    /// Interface for managing looping methods.  Implementations include <see cref="ThreadLoop"/> which utilises
    /// a persistent <see cref="Thread"/>, and <see cref="TimerLoop"/> which uses a <see cref="Timer"/>.
    /// </summary>
    public interface ILoop : IDisposable
    {
        /// <summary>
        /// Raised for any unhandled exceptions caught from the loop method.
        /// </summary>
        event ValueMethodInvoker<Exception> Error;

        /// <summary>
        /// Gets and sets a value stating whether the loop should exit if the loop method raises an
        /// unhandled exception.  The <see cref="Error"/> event will be raised regardless of this property's
        /// value.
        /// </summary>
        bool EndLoopOnError { get; set; }

        /// <summary>
        /// Gets and sets a value stating whether the loop should sleep after an unhandled exception occurs in
        /// the loop method.  The <see cref="Error"/> event will be raised regardless of this property's
        /// value.
        /// </summary>
        bool SleepOnError { get; set; }

        /// <summary>
        /// Starts the loop's execution, calling the constructor's loop method in an endless loop until
        /// <see cref="Stop"/> is called.
        /// </summary>
        /// <param name="runLoopImmediately">Causes the loop to wait it's period once before executing.
        /// If false, the loop will be executed immediately upon calling this method.</param>
        void Start(bool runLoopImmediately);

        /// <summary>
        /// Stops the loop.  This call blocks until the loop has terminated.  If the client action
        /// (loop method) takes a while to complete, this method will also be slow to return.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets a name that identifies this loop.
        /// </summary>
        string LoopName { get; }

        TimeSpan SleepPeriod { get; }

        bool IsRunning { get; }

        void ChangeSleepPeriod(TimeSpan sleepPeriod, bool runLoopImmediately);
    }

    public delegate void LoopMethod();
}