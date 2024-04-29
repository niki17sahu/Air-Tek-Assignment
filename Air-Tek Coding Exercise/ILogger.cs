using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Tek_Coding_Exercise
{
    interface ILogger
    {
        void LogError(string message);
        void LogInformation(string message);
    }
}
