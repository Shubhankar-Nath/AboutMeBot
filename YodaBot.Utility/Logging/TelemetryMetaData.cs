
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaBot.Utility.Logging
{
    public class TelemetryMetaData
    {
        /// <summary>
        /// To return a Dictionary for addiional parameters in Telemetry Data
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="exceptionEvent"></param>
        public static Dictionary<string, string> BindExceptionEvent(string exceptionType, string exceptionEvent )
        {
            return new Dictionary<string, string>()
            {
                {exceptionType,exceptionEvent}
            };

        }

        public static Dictionary<string, string> BindExceptionEvent(string exceptionType, string exceptionEvent, string conversationID, string cardJson)
        {
            return new Dictionary<string, string>()
            {
                {exceptionType,exceptionEvent},
                {"ConversationID",conversationID },
                {"CardJson", cardJson}
            };

        }
    }
}
