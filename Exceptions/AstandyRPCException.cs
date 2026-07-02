using System;

namespace Astandy.Exceptions
{
    public class AstandyRPCException : Exception
    {
        public int ErrorCode { get; }

        public AstandyRPCException(string message, int errorCode = -1) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}