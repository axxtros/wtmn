using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wspd.commons.entity
{
    public enum ADDON_TYPES {
        NONE = 0,

        //armaholic
        GEAR = 1,
        PACKS = 2,
        MISCELLANEOUS = 3,
        OBJECTS = 4,
        REPLACEMENT_PACKS = 5,
        SOUNDS = 6,
        TERRAIN = 7,
        UNITS = 8,
        WEAPONS = 9,
        CHOPPERS = 10,
        HEAVY_ARMOR = 11,
        LIGHT_ARMOR = 12,
        VEHICLE_PACKS = 13,
        PLANES = 14,
        SEA = 15,
        WHEELED = 16,

        //ets2.lt
        TRUCK_MOD = 17,
        TRAILER_MOD = 18,
        INTERIOR_MOD = 19,
        INTERIOR_ADDON = 20,
        PARTS_TUNING_MOD = 21,
        AI_TRAFFIC = 22,
        SOUND_MOD = 23,
        TRUCK_SKIN = 24,
        COMBO_SKIN_PACKS = 25,
        MAPS = 26,
        CARS = 27,
        OTHERS = 28,
        NOT_DEFINED = 29       
    };

    public class AddonEntity
    {        
        private ADDON_TYPES type;        
        private int page;
        private String name;
        private string addonURL;
        private int year;
        private int month;
        private int day;
        private int listIndex;

        public AddonEntity()
        {
            //NOP..
        }

        public AddonEntity(ADDON_TYPES type)
        {
            this.Type = type;
        }

        public AddonEntity(ADDON_TYPES type, int page) : this(type)
        {
            this.page = page;
        }

        public ADDON_TYPES Type { get => type; set => type = value; }
        public int Page { get => page; set => page = value; }
        public string Name { get => name.Trim(); set => name = value; }
        public int Year { get => year; set => year = value; }
        public int Month { get => month; set => month = value; }
        public int Day { get => day; set => day = value; }
        public int ListIndex { get => listIndex; set => listIndex = value; }
        public string AddonURL { get => addonURL.Trim(); set => addonURL = value; }
    }
}
