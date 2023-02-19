using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snitch.Core.Utilities
{
    public class SnitchEventHandler
    {
        public TimeSpan TimerDelay { get; set; }
        public delegate void TimeDelegate();
        public event TimeDelegate TimeHasCome;
        public delegate void EventDelegate();
        public event EventDelegate Happened;
        public void HappenedEvent()
        {
            if (Happened != null)
            {
                Happened();
            }
        }
        public void HappenedEvent(object O)
        {
            if (Happened != null)
            {
                Happened();
            }
        }
        public void TimerEvent()
        {
            TimeHasCome();
        }
        public void TimerEvent(object O)
        {
            TimeHasCome();
        }

        TimerCallback TimerCallback;
        Timer Timer;
        public void StartTimer()
        {
            TimerCallback = new TimerCallback(TimerEvent);
            Timer = new Timer(TimerCallback, null, TimerDelay, Timeout.InfiniteTimeSpan);
        }
        public void StopTimer()
        {
            Timer.Dispose();
        }

        public SnitchEventHandler() { }
    }
}
