using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendCRUD.Common
{
    public class Enum
    {
        public enum LogType
        {
            WebSite,
            ConsoleTask,
        }

        public enum EnumMessage
        {
            Succes,
            Error,
            Alert
        }
        public enum CallType
        {
            Get,
            Post,
            Put
        }

        public enum MemberType
        {
            E,
            C,
        }

        public enum ActionType
        {
            ADD,
            UPDATE,
        }
    }
}