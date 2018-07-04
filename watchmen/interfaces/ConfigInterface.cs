using System.Windows.Media;
using wspd.commons.entity;
using wspd.interfaces;

namespace watchmen.interfaces
{
    public enum PAGE_PARSER_TYPES
    {
        DATE_BASED = 1,
        PAGE_BASED = 2
    }

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

        PAGE_PARSER_TYPES getParsingType();
    }
}
