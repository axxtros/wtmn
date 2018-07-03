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

    public static class AddonTypeConverter
    {
        /// <summary>
        /// Visszaadja string alapján az addon típusát
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ADDON_TYPES GetType(string type)
        {
            switch (type)
            {
                //armaholic
                case "GEAR": return ADDON_TYPES.GEAR;
                case "PACKS": return ADDON_TYPES.PACKS;
                case "MISCELLANEOUS": return ADDON_TYPES.MISCELLANEOUS;
                case "OBJECTS": return ADDON_TYPES.OBJECTS;
                case "REPLACEMENT_PACKS": return ADDON_TYPES.REPLACEMENT_PACKS;
                case "SOUNDS": return ADDON_TYPES.SOUNDS;
                case "TERRAIN": return ADDON_TYPES.TERRAIN;
                case "UNITS": return ADDON_TYPES.UNITS;
                case "WEAPONS": return ADDON_TYPES.WEAPONS;
                case "CHOPPERS": return ADDON_TYPES.CHOPPERS;
                case "HEAVY_ARMOR": return ADDON_TYPES.HEAVY_ARMOR;
                case "LIGHT_ARMOR": return ADDON_TYPES.LIGHT_ARMOR;
                case "VEHICLE_PACKS": return ADDON_TYPES.VEHICLE_PACKS;
                case "PLANES": return ADDON_TYPES.PLANES;
                case "SEA": return ADDON_TYPES.SEA;
                case "WHEELED": return ADDON_TYPES.WHEELED;

                //ets2.lt                
                case "TRUCK_MOD": return ADDON_TYPES.TRUCK_MOD;
                case "TRAILER_MOD": return ADDON_TYPES.TRAILER_MOD;
                case "INTERIOR_MOD": return ADDON_TYPES.INTERIOR_MOD;
                case "INTERIOR_ADDON": return ADDON_TYPES.INTERIOR_ADDON;
                case "PARTS_TUNING_MOD": return ADDON_TYPES.PARTS_TUNING_MOD;
                case "AI_TRAFFIC": return ADDON_TYPES.AI_TRAFFIC;
                case "SOUND_MOD": return ADDON_TYPES.SOUND_MOD;
                case "TRUCK_SKIN": return ADDON_TYPES.TRUCK_SKIN;
                case "COMBO_SKIN_PACKS": return ADDON_TYPES.COMBO_SKIN_PACKS;
                case "MAPS": return ADDON_TYPES.MAPS;
                case "CARS": return ADDON_TYPES.CARS;
                case "OTHERS": return ADDON_TYPES.OTHERS;
                case "NOT_DEFINED": return ADDON_TYPES.NOT_DEFINED;

                default: return ADDON_TYPES.NONE;
            }
        }
    }

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

        #region getters/setters
        public ADDON_TYPES Type { get => type; set => type = value; }
        public int Page { get => page; set => page = value; }
        public string Name { get => name.Trim(); set => name = value; }
        public int Year { get => year; set => year = value; }
        public int Month { get => month; set => month = value; }
        public int Day { get => day; set => day = value; }
        public int ListIndex { get => listIndex; set => listIndex = value; }
        public string AddonURL { get => addonURL.Trim(); set => addonURL = value; }
        #endregion
    }
}
