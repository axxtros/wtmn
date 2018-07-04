using System.Windows.Media;
using watchmen.interfaces;
using wspd.commons.entity;
using wspd.interfaces;
using wspd.parsers;

namespace watchmen.processconfigs
{
    public class ArmaholicConfig : interfaces.ConfigInterface
    {        

        public ADDON_TYPES[] getAddonTypes()
        {
            ADDON_TYPES[] addonTypes = {
                ADDON_TYPES.GEAR,
                ADDON_TYPES.PACKS,
                ADDON_TYPES.MISCELLANEOUS,
                ADDON_TYPES.OBJECTS,
                ADDON_TYPES.REPLACEMENT_PACKS,
                ADDON_TYPES.SOUNDS,
                ADDON_TYPES.TERRAIN,
                ADDON_TYPES.UNITS,
                ADDON_TYPES.WEAPONS,
                ADDON_TYPES.CHOPPERS,
                ADDON_TYPES.HEAVY_ARMOR,
                ADDON_TYPES.LIGHT_ARMOR,
                ADDON_TYPES.VEHICLE_PACKS,
                ADDON_TYPES.PLANES,
                ADDON_TYPES.SEA,
                ADDON_TYPES.WHEELED
            };
            return addonTypes;
        }

        public Color getFirstColor()
        {
            return Color.FromArgb(255, 227, 225, 214);
        }        

        public Color getSecondColor()
        {
            return Color.FromArgb(255, 136, 119, 93);
        }

        public ProcessInterface getProcess()
        {
            return new ArmaholicParserProcess();
        }

        public PAGE_PARSER_TYPES getParsingType()
        {
            return PAGE_PARSER_TYPES.DATE_BASED;
        }
    }
}
