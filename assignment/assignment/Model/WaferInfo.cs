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
        public int dieNumIndex, displayValue;
        public List<Point> sampleTestPlan;
        public List<int> defectCount;
        public WaferInfo()
        {
            sampleTestPlan = new List<Point>();
            defectCount = new List<int>();
        }
    }
}
