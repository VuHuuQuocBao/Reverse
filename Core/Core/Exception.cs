namespace Core.Core
{
    public class ReturnException : Exception
    {
        public object value;
        public ReturnException(object valueParm)
        {
            value = valueParm;
        }   
    }
}
