using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace assignment.Model
{    
    class WaferInfo
    {
        public string waferID, lotID, deviceID, fileTimestamp;
        public int dieNumIndex, defectCountIndex, displayValue;
        public List<Point> sampleTestPlan;
        public List<int> defectCount, dieNum;
        public WaferInfo()
        {
            sampleTestPlan = new List<Point>();
            defectCount = new List<int>();
            dieNum = new List<int>();
        }
    }
}
