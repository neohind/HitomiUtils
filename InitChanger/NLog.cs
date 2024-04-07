using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog
{

    public class Logger
    {
        internal void Debug(string sMessage)
        {
            Console.WriteLine(sMessage);
        }

        internal void Error(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public class LogManager
    {
        static public Logger GetCurrentClassLogger()
        {
            return new Logger();   
        }


    }
}
