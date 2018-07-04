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
        /// <summary>
        /// Ha az adott weboldalt dátum alapján kell felolvasni, akkor ezt a metódust kell meghívni.
        /// </summary>
        /// <param name="selectedYear"></param>
        /// <param name="selectedMonth"></param>
        /// <param name="selectedDay"></param>
        void startParsing(int selectedYear, int selectedMonth, int selectedDay);

        /// <summary>
        /// Ha az adott weboldalt oldalszám alapján kell felolvasni, akkor ezt a metódust kell meghívni.
        /// </summary>
        /// <param name="pageNumber">Eddig az oldalig.</param>
        void startParsing(int pageNumber);

        /// <summary>
        /// Ebbe a listába jönnek vissza a kapott addon-ok.
        /// </summary>
        /// <returns></returns>
        List<AddonEntity> getAddonList();        

        /// <summary>
        /// A parser neve, nincs jelentősége, ez jelenik meg a fejlécben címként az űrlapon.
        /// </summary>
        /// <returns></returns>
        string getParserName();

        /// <summary>
        /// Az addonok file neve.
        /// </summary>
        /// <returns></returns>
        string getFileName();
    }
}
