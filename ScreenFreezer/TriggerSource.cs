using System;

namespace ScreenFreezer
{
    public class TriggerSource
    {
        public Trigger Trigger { get; private set; }
        public int Interval { get; init; }
        public Action FinalAction { get; init; }

        public TriggerSource(bool pullImmed = true)
            => CreateNewTrigger(pullImmed);

        public void CreateNewTrigger(bool pullImmed = true)
        {
            Trigger?.Dispose();
            Trigger = new Trigger(Interval);

            Trigger.Result += result =>
            {
                if (result) FinalAction?.Invoke();
            };

            if (pullImmed) Pull();
        }

        public void Pull()
            => Trigger.Pull();
        
        public void Vanish()
            => CreateNewTrigger(false);
    }
}
