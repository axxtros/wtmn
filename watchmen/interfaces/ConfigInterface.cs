using System.Drawing;
using wspd.commons.entity;
using wspd.interfaces;

namespace watchmen.interfaces
{
    interface ConfigInterface
    {
        /// <summary>
        /// Az addon lehetséges típusa
        /// </summary>
        /// <returns></returns>
        ADDON_TYPES[] getAddonTypes();

        /// <summary>
        /// A parser feldolgozó objetuma.
        /// </summary>
        /// <returns></returns>
        ProcessInterface getProcess();

        /// <summary>
        /// Header szín 1.
        /// </summary>
        /// <returns></returns>
        Color getFirstColor();

        /// <summary>
        /// Header szín 2.
        /// </summary>
        /// <returns></returns>
        Color getSecondColor();
    }
}
