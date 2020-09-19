using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickTick1
{
    class GlobalVariables
    {
        private static String setNo = "";
        public static String SetNo
        {
            get { return setNo; }
            set { setNo = value; }
        }
        private static String idToEdit = "";
        public static String IdToEdit
        {
            get { return idToEdit; }
            set { idToEdit = value; }
        }
    }
}
