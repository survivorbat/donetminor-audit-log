using System;

namespace MaartenH.Minor.Miffy.AuditLogging.Exceptions
{
    public class ListenerValidationException : Exception
    {
        public ListenerValidationException(string message) : base(message)
        {

        }
    }
}
