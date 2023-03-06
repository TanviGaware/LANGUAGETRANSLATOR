using System;
using System.Collections.Generic;
using System.Text;

namespace Automation.common
{
    /// <summary>
    /// Ths is the exception class for automation.
    /// </summary>
    public class AutomationException : ApplicationException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">message for user display.</param>
        public AutomationException(string message)
            : base(message)
        {
        }
    }
}
