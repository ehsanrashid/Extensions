namespace System.Threading
{
    public class WorkItem
    {
        public WaitCallback Callback;
        public Object Parameter;
        
        public WorkItem(WaitCallback callback, Object parameter)
        {
            Parameter = parameter;
            Callback = callback;
        }
    }
}