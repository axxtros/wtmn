using System.Windows.Media;
using watchmen.interfaces;
using wspd.commons.entity;
using wspd.interfaces;
using wspd.parsers;

namespace watchmen.processconfigs
{
    class Ets2LtConfig : interfaces.ConfigInterface
    {
        public ADDON_TYPES[] getAddonTypes()
        {
            ADDON_TYPES[] addonTypes = {
                ADDON_TYPES.TRUCK_MOD,
                ADDON_TYPES.TRAILER_MOD,
                ADDON_TYPES.INTERIOR_MOD,
                ADDON_TYPES.INTERIOR_ADDON,
                ADDON_TYPES.PARTS_TUNING_MOD,
                ADDON_TYPES.AI_TRAFFIC,
                ADDON_TYPES.SOUND_MOD,
                ADDON_TYPES.TRUCK_SKIN,
                ADDON_TYPES.COMBO_SKIN_PACKS,
                ADDON_TYPES.MAPS,
                ADDON_TYPES.CARS,
                ADDON_TYPES.OTHERS,
                ADDON_TYPES.NOT_DEFINED                
            };
            return addonTypes;
        }

        public Color getFirstColor()
        {
            return Color.FromArgb(255, 227, 225, 214);
        }

        public ProcessInterface getProcess()
        {
            return new Ets2ltParserProcess();
        }

        public Color getSecondColor()
        {
            return Color.FromArgb(255, 136, 119, 93);
        }

        public PAGE_PARSER_TYPES getParsingType()
        {
            return PAGE_PARSER_TYPES.PAGE_BASED;
        }
        
    }
}
