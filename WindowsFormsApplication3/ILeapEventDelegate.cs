using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
   public  interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }
}
