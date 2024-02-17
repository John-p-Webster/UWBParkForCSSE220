namespace UWBPark.src.ServerSide
{
    using System.Timers;
    public static class SystemTimer
    {
        private static System.Timers.Timer aTimer;

        // Define a delegate that matches the signature of the method to be called in the other class
        public delegate void TimerElapsedDelegate();

        // Define an event based on the delegate
        public static event TimerElapsedDelegate TimerElapsed;

        public static void SetTimer(double interval = 500)
        {
            // Create a timer with a .5 second interval.
            aTimer = new System.Timers.Timer(interval);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // Invoke the event
            TimerElapsed?.Invoke();
        }
}
}
