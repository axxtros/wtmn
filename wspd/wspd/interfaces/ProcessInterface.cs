using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

namespace wspd.interfaces
{    
    public interface ProcessInterface
    {
        List<AddonEntity> getAddonList();

        void startParsing(int selectedYear, int selectedMonth, int selectedDay);
    }
}
