using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2v2
{
    public class ChangeLogObhect
    {
        public string LogType { get; set; }
        public int LogObjId { get; set; }
        public string LogObjectType { get; set; }
        public static int LogCountNew { get; set; }
        public static int LogCountChng { get; set; }
        public string LogPrevState { get; set; }
        public string LogCurState { get; set; }
        public static string UpdateStatus = "Обновление базы прошло успешно";


    }
}