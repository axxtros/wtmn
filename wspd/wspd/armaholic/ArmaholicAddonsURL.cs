using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

namespace wspd.armaholic
{
    class ArmaholicResource
    {
        private String url;
        private wspd.commons.entity.ADDON_TYPES addonType;

        public ArmaholicResource(string url, ADDON_TYPES addonType)
        {
            this.Url = url;
            this.AddonType = addonType;
        }

        public string Url { get => url; set => url = value; }
        internal ADDON_TYPES AddonType { get => addonType; set => addonType = value; }
    }

    static class ArmaholicAddonsURL
    {

        public static List<ArmaholicResource> ARMAHOLIC_RESOURCES = new List<ArmaholicResource> {
            //addons
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_gear&s=title&w=asc&d=0", ADDON_TYPES.GEAR),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_packs&s=title&w=asc&d=0", ADDON_TYPES.PACKS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_misc&s=title&w=asc&d=0", ADDON_TYPES.MISCELLANEOUS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_objects&s=title&w=asc&d=0", ADDON_TYPES.OBJECTS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_replacements&s=title&w=asc&d=0", ADDON_TYPES.REPLACEMENT_PACKS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_soundmods&s=title&w=asc&d=0", ADDON_TYPES.SOUNDS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_islands&s=title&w=asc&d=0", ADDON_TYPES.TERRAIN),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_units&s=title&w=asc&d=0", ADDON_TYPES.UNITS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_weapons&s=title&w=asc&d=0", ADDON_TYPES.WEAPONS),
            ////vehicles
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_chopper&s=title&w=asc&d=0", ADDON_TYPES.CHOPPERS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_heavyarmor&s=title&w=asc&d=0", ADDON_TYPES.HEAVY_ARMOR),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_lightarmor&s=title&w=asc&d=0", ADDON_TYPES.LIGHT_ARMOR),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_packs&s=title&w=asc&d=0", ADDON_TYPES.VEHICLE_PACKS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_planes&s=title&w=asc&d=0", ADDON_TYPES.PLANES),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_sea&s=title&w=asc&d=0", ADDON_TYPES.SEA),
            new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_wheeled&s=title&w=asc&d=0", ADDON_TYPES.WHEELED)
        };

    }
}
