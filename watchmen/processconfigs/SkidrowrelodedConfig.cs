using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using watchmen.interfaces;
using wspd.commons.entity;
using wspd.interfaces;
using wspd.parsers;

namespace watchmen.processconfigs
{
    class SkidrowrelodedConfig : interfaces.ConfigInterface
    {
        public ADDON_TYPES[] getAddonTypes()
        {
            ADDON_TYPES[] addonTypes = {
                ADDON_TYPES.GAME
            };
            return addonTypes;
        }

        public Color getFirstColor()
        {
            return Color.FromArgb(255, 243, 243, 243);
        }

        public Color getSecondColor()
        {
            return Color.FromArgb(255, 243, 243, 243);
        }

        public PAGE_PARSER_TYPES getParsingType()
        {
            return PAGE_PARSER_TYPES.PAGE_BASED;
        }

        public ProcessInterface getProcess()
        {
            return new SkidrowrelodedParserProcess();
        }        
    }
}
