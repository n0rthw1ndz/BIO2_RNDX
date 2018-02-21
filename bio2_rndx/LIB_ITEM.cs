﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace bio2_rndx
{

    // a static class to return all item based info
   public static class LIB_ITEM
    {

        public static Dictionary<byte, string> BIO2_ITEM_LUT = new Dictionary<byte, string>()
        {
            {0x00, "Nothing"},
            {0x01, "Knife"},
            {0x02, "H&K VP70"},
            {0x03, "Browning"},
            {0x04, "Custom Handgun" },
            {0x05, "Magnum"},
            {0x06, "Custom Magnum"},
            {0x07, "Shotgun" },
            {0x08, "Custom Shotgun" },
            {0x09, "G Launcher (G)" },
            {0x0A, "G Launcher (F)" },
            {0x0B, "G Launcher (A)" },
            {0x0C, "Bowgun" },
            {0x0D, "Colt S.A.A"},
            {0x0E, "Sparkshot" },
            {0x0F, "Ingram" },
            {0x10, "Flamethrower" },
            {0x11, "Rocket Launcher" },
            {0x12, "Gatling Gun"},
            {0x13, "Beretta M29FS" },
            {0x14, "Handgun Ammo" },
            {0x15, "Shotgun Ammo" },
            {0x16, "Magnum Ammo"},
            {0x17, "Flamer Fuel" },
            {0x18, "Grenade Rounds" },
            {0x19, "Flame Rounds" },
            {0x1A, "Acid Rounds" },
            {0x1B, "SMG ammo" },
            {0x1C, "Sparkshot ammo" },
            {0x1D, "Bowgun Ammo" },
            {0x1E, "Ink Ribbon" },
            {0x1F, "Small Key" },
            {0x20, "Handgun Parts" },
            {0x21, "Magnum Parts" },
            {0x22, "Shotgun Parts" },
            {0x23, "F-Aid Spray" },
            {0x24, "Antivirus Bomb" },
            {0x25, "Chemical ACw-32" },
            {0x26, "Green Herb" },
            {0x27, "Red Herb" },
            {0x28, "Blue Herb" },
            {0x29, "G + G Herb" },
            {0x2A, "G + R Herb" },
            {0x2B, "G + B Herb" },
            {0x2C, "G + G + G Herb" },
            {0x2D, "G + G + B Herb" },
            {0x2E, "G + R + B Herb" },
            {0x2F, "Lighter" },
            {0x30, "Lockpick" },
            {0x31, "Sherry Photo"},
            {0x32, "Valve Handle" },
            {0x33, "Red Jewel" },
            {0x34, "Red Card" },
            {0x35, "Blue Card" },
            {0x36, "Serpent Stone" },
            {0x37, "Jaguar Stone" },
            {0x38, "Jaguar Stone L" },
            {0x39, "Jaguar Stone R" },
            {0x3A, "Eagle Stone" },
            {0x3B, "Bishop Plug" },
            {0x3C, "Rook Plug" },
            {0x3D, "Knight Plug" },
            {0x3E, "King Plug" },
            {0x3F, "Weapon Storage key" },
            {0x40, "Detonator" },
            {0x41, "C4" },
            {0x42, "C4+ Detonator" },
            {0x43, "Crank" },
            {0x44, "Film A" },
            {0x45, "Film B" },
            {0x46, "Film C" },
            {0x47, "Unicorn Medal" },
            {0x48, "Eagle Medal" },
            {0x49, "Wolf Medal" },
            {0x4A, "Cog" },
            {0x4B, "Manhole opener" },
            {0x4C, "Main Fuse" },
            {0x4D, "Fuse Case" },
            {0x4E, "Vaccine" },
            {0x4F, "Vaccine Container" },
            {0x50, "Film D" },
            {0x51, "Base Vaccine" },
            {0x52, "G-Virus" },
            {0x53, "Base Vaccine (case only)" },
            {0x54, "Joint S Plug" },
            {0x55, "Joint N Plug" },
            {0x56, "Wire" },
            {0x57, "Ada's Photo" },
            {0x58, "Cabin Key" },
            {0x59, "Spade Key" },
            {0x5A, "Diamond Key" },
            {0x5B, "Heart Key" },
            {0x5C, "Club Key" },
            {0x5D, "Control Pannel Key (Down)" },
            {0x5E, "Control Pannel Key (Up)" },
            {0x5F, "Power Room Key" },
            {0x60, "MO Disk" },
            {0x61, "Umbrella KeyCard" },
            {0x62, "Master Key" },
            {0x63, "Platform Key"},
            {0x64, "Sparkshot Handle"},
            {0x65, "Ingram Stock"},
            {0x66, "Flamethrower Handle"},
            {0x67, "Rocket Launcher Base" },
            {0x68, "Gatling Base" }
            
        };



        /// <summary>
        /// STRING > ID
        /// </summary>
        public static Dictionary<string, byte> BIO2_ITEM_LUT_INVERSE = new Dictionary<string, byte>()
        {
            {"Nothing", 0x00},
            {"Knife", 0x01},
            {"H&K VP70", 0x02},
            {"Browning", 0x03},
            {"Custom Handgun" ,0x04},
            {"Magnum", 0x05},
            {"Custom Magnum", 0x06},
            {"Shotgun", 0x07},
            {"Custom Shotgun",0x08},
            {"G Launcher (G)", 0x09},
            {"G Launcher (F)", 0x0A},
            {"G Launcher (A)" ,0x0B},
            {"Bowgun", 0x0C},
            {"Colt S.A.A", 0x0D},
            {"Sparkshot", 0x0E},
            {"Ingram" , 0x0F},
            {"Flamethrower" , 0x10},
            {"Rocket Launcher",0x11},
            {"Gatling Gun",0x12},
            {"Beretta M29FS",0x13},
            {"Handgun Ammo", 0x14},
            {"Shotgun Ammo",0x15},
            {"Magnum Ammo", 0x16},
            {"Flamer Fuel", 0x17},
            {"Grenade Rounds" , 0x18},
            {"Flame Rounds" ,0x19},
            {"Acid Rounds" ,0x1A},
            {"SMG ammo", 0x1B},
            {"Sparkshot ammo",0x1C},
            {"Bowgun Ammo",0x1D },
            {"Ink Ribbon", 0x1E},
            {"Small Key", 0x1F},
            {"Handgun Parts", 0x20},
            {"Magnum Parts", 0x21},
            {"Shotgun Parts", 0x22},
            {"F-Aid Spray", 0x23},
            {"Antivirus Bomb", 0x24},
            {"Chemical ACw-32", 0x25},
            {"Green Herb", 0x26},
            {"Red Herb", 0x27},
            {"Blue Herb", 0x28},
            {"G + G Herb", 0x29},
            {"G + R Herb", 0x2A},
            {"G + B Herb", 0x2B},
            {"G + G + G Herb", 0x2C},
            {"G + G + B Herb", 0x2D},
            {"G + R + B Herb", 0x2E},
            {"Lighter", 0x2F},
            {"Lockpick", 0x30},
            {"Sherry Photo", 0x31},
            {"Valve Handle", 0x32},
            {"Red Jewel", 0x33},
            {"Red Card", 0x34},
            {"Blue Card", 0x35},
            {"Serpent Stone", 0x36},
            {"Jaguar Stone", 0x37},
            {"Jaguar Stone L", 0x38},
            {"Jaguar Stone R", 0x39},
            {"Eagle Stone", 0x3A},
            {"Bishop Plug", 0x3B},
            {"Rook Plug", 0x3C},
            {"Knight Plug", 0x3D},
            {"King Plug", 0x3E},
            {"Weapon Storage key", 0x3F},
            {"Detonator", 0x40},
            {"C4" , 0x41},
            {"C4+ Detonator", 0x42},
            {"Crank", 0x43},
            {"Film A", 0x44},
            {"Film B", 0x45},
            {"Film C", 0x46},
            {"Unicorn Medal",0x47},
            {"Eagle Medal",0x48},
            {"Wolf Medal", 0x49},
            {"Cog", 0x4A},
            {"Manhole opener", 0x4B},
            {"Main Fuse", 0x4C},
            {"Fuse Case",0x4D},
            {"Vaccine" ,0x4E},
            {"Vaccine Container",0x4F},
            {"Film D", 0x50},
            {"Base Vaccine" ,0x51},
            {"G-Virus", 0x52},
            {"Base Vaccine (case only)", 0x53},
            {"Joint S Plug", 0x54},
            {"Joint N Plug", 0x55},
            {"Wire" , 0x56},
            {"Ada's Photo", 0x57},
            {"Cabin Key" , 0x58},
            {"Spade Key", 0x59},
            {"Diamond Key", 0x5A},
            {"Heart Key", 0x5B},
            {"Club Key", 0x5C},
            {"Control Pannel Key (Down)", 0x5D},
            {"Control Pannel Key (Up)", 0x5E},
            {"Power Room Key",0x5F},
            { "MO Disk",0x60},
            {"Umbrella KeyCard", 0x61},
            {"Master Key", 0x62},
            {"Platform Key",0x63},
            {"Sparkshot Handle", 0x64},
            {"Ingram Stock", 0x65},
            {"Flamethrower Handle", 0x66},
            {"Rocket Launcher Base", 0x67},
            {"Gatling Base", 0x68}
            
        };

        public static Dictionary<byte, string> BIO3_ITEM_LUT = new Dictionary<byte, string>()
        {
            {0x00, "Nothing"},
            {0x01, "Combat Knife" },
            {0x02, "Sigpro SP"},
            {0x03, "Beretta M92F Handgun"},
            {0x04, "Shotgun Benelli M3S"},
            {0x05, "Smith & Wesson M629C"},
            {0x06, "Grenade Launcher (BURST)"},
            {0x07, "Grenade Launcher (FLAME)"},
            {0x08, "Grenade Launcher (ACID)" },
            {0x09, "Grenade Launcher (FREEZE)" },
            {0x0A, "M66 Rocket Launcher"},
            {0x0B, "Gatling Gun" },
            {0x0C, "Mine Thrower" },
            {0x0D, "STI EAGLE 6.0"},
            {0x0E, "M4A1 Assault Rifle(MANUAL)"},
            {0x0F, "M4A1 Assault Rifle(AUTO)"},
            {0x10, "Custom M37 Shotgun"},
            {0x11, "Sigpro Sp 2009 (Enhanced Ammo)"},
            {0x12, "Beretta M92F custom (Enhanced Ammo)"},
            {0x13, "Shotgun Benelli M3S (Enhanced Ammo)" },
            {0x14, "Mine Thrower(Enhanced Ammo)" },
            {0x15, "Handgun Bullets"},
            {0x16, "Magnum Rounds" },
            {0x17, "Shotgun Shells" },
            {0x18, "Grenade Rounds"},
            {0x19 ,"Flame Rounds"},
            {0x1A, "Acid Rounds" },
            {0x1B, "Freeze Rounds" },
            {0x1C, "Minethrower Rounds"},
            {0x1D, "Assault Rifle Clip" },
            {0x1E, "Enhanced Handgun Bullets"},
            {0x1F, "Enhanced Shotgun Shell" },
            {0x20, "First Aid Spray" },
            {0x21, "Green Herb"},
            {0x22, "Blue Herb" },
            {0x23, "Red Herb"},
            {0x24, "2x Green" },
            {0x25, "Green/Blue" },
            {0x26, "Green/Red"},
            {0x27, "3x Green"},
            {0x28, "Green/Green/Blue"},
            {0x29, "Green/Red/Blue"},
            {0x2A, "First Aid Spray Box"},
            {0x2B, "Square Crank"},
            {0x2C, "BOTU(RED)"},
            {0x2D, "BOTU(BLUE)"},
            {0x2E, "BOTU(GOLD)"},
            {0x2F, "Stars Card(Jill)"},
            {0x30, "GIGA OIL"},
            {0x31, "Battery"},
            {0x32, "Fire Hook"},
            {0x33, "Power Cable"},
            {0x34, "Fuse"},
            {0x35, "Unknown Broken Fire Hose"},
            {0x36, "Oil Additive"},
            {0x37, "Brad Vicker's Card Case"},
            {0x38, "Stars Card (BRAD)" },
            {0x39, "Machine Oil"},
            {0x3A, "Mixed Oil"},
            {0x3B, "Steel Chain"},
            {0x3C, "Wrench"},
            {0x3D, "Iron Pipe"},
            {0x3E, "Cylinder?"},
            {0x3F, "Fire Hose"},
            {0x40, "Tape Recorder" },
            {0x41, "Ligther Oil"},
            {0x42, "Lighter Case"},
            {0x43, "Ligther"},
            {0x44, "Green Gem"},
            {0x45, "Blue Gem"},
            {0x46, "Amber Ball"},
            {0x47, "Obsidian Ball"},
            {0x48, "Crystal Ball"},
            {0x49, "Demo Remote(No Batteries)"},
            {0x4A, "Demo Remote(Batteries)" },
            {0x4B, "Remote Batteries" },
            {0x4C, "Gold Gear"},
            {0x4D, "Silver Gear"},
            {0x4E, "Chronos Gear"},
            {0x4F, "Bronze Book"},
            {0x50, "Bronze Compass" },
            {0x51, "Vaccine Medium" },
            {0x52, "Vaccine Base" },
            {0x53, "Demo Sigpro_00" },
            {0x54, "Demo Sigpro_01" },
            {0x55, "Vaccine" },
            {0x56, "Demo_Sigpro_02" },
            {0x57, "Demo SIgpro_03" },
            {0x58, "Medium Base" },
            {0x59, "Eagle Parts A"},
            {0x5A, "Eagle Parts B" },
            {0x5B, "M37 Parts A" },
            {0x5C, "M37 Parts B" },
            {0x5D, "Demo Sigpro_04"},
            {0x5E, "Chronos Chain" },
            {0x5F, "Rusted Crank" },
            {0x60, "Card Key"},
            {0x61, "Gun Powder A" },
            {0x62, "Gun Powder B" },
            {0x63, "Gun Powder C" },
            {0x64, "Gun Powder AA" },
            {0x65, "Gun Powder BB" },
            {0x66, "Gun Powder AC" },
            {0x67, "Gun Powder BC" },
            {0x68, "Gun Powder CC" },
            {0x69, "Gun Powder AAA"},
            {0x6A, "Gun Powder AAB"},
            {0x6B, "Gun Powder BBA"},
            {0x6C, "Gun Powder BBB"},
            {0x6D, "Gun Powder CCC"},
            {0x6E, "Inf Ammo Case" },
            {0x6F, "Water Sample"},
            {0x70, "System Disk" },
            {0x71, "Dummy Key" },
            {0x72, "Lockpick" },
            {0x73, "Warehouse Key"},
            {0x74, "Sickroom Key" },
            {0x75, "Stars Key" },
            {0x76, "Beta Keyring" },
            {0x77, "Clock Tower Key (BEZEL)" },
            {0x78, "Clock Tower Key (WINDER)" },
            {0x79, "Chronos Key" },
            {0x7A, "Demo_Sigpro_05"},
            {0x7B, "Park Key(FRONT)" },
            {0x7C, "Park Key(GRAVEYARD)" },
            {0x7D, "Park Key(REAR)" },
            {0x7E, "Facility Key(No Barcode)" },
            {0x7F, "Facility Key(Barcode)" },
            {0x80, "Boutique Key" },
            {0x81, "Ink Ribbon" },
            {0x82, "Reloading Tool" },
            {0x83, "Game Instructions A"},
            {0x84, "Game Instructions B" },
            {0x85, "Game Instructions C" },
            {0x89, "Marvins Report"},
            {0x96, "Kendo Fax" },
            {0xA4, "Map of UpTown" },
            {0xA7, "Park Map" },
            {0xA9, "RPD Map" }
            
        };

        public static Dictionary<string, byte> BIO3_ITEM_LUT_INVERSE = new Dictionary<string, byte>()
        {
            {"Nothing",0x00},
            {"Combat Knife", 0x01 },
            {"Sigpro SP", 0x02},
            {"Beretta M92F Handgun", 0x03},
            {"Shotgun Benelli M3S", 0x04},
            {"Smith & Wesson M629C", 0x05},
            {"Grenade Launcher (BURST)", 0x06 },
            {"Grenade Launcher (FLAME)", 0x07 },
            {"Grenade Launcher (ACID)", 0x08  },
            {"Grenade Launcher (FREEZE)", 0x09  },
            {"M66 Rocket Launcher", 0x0A },
            {"Gatling Gun" , 0x0B },
            {"Mine Thrower", 0x0C  },
            {"STI EAGLE 6.0", 0x0D },
            {"M4A1 Assault Rifle(MANUAL)", 0x0E },
            {"M4A1 Assault Rifle(AUTO)", 0x0F },
            {"Custom M37 Shotgun", 0x10},
            {"Sigpro Sp 2009 (Enhanced Ammo)", 0x11},
            {"Beretta M92F custom (Enhanced Ammo)", 0x12},
            {"Shotgun Benelli M3S (Enhanced Ammo)", 0x13},
            {"Mine Thrower(Enhanced Ammo)", 0x14},
            {"Handgun Bullets", 0x15},
            {"Magnum Rounds", 0x16},
            {"Shotgun Shells", 0x17},
            {"Grenade Rounds", 0x18},
            {"Flame Rounds", 0x19},
            {"Acid Rounds", 0x1A},
            {"Freeze Rounds", 0x1B},
            {"Minethrower Rounds", 0x1C},
            {"Assault Rifle Clip", 0x1D},
            {"Enhanced Handgun Bullets", 0x1E},
            {"Enhanced Shotgun Shell", 0x1F},
            {"First Aid Spray", 0x20},
            {"Green Herb", 0x21},
            {"Blue Herb", 0x22},
            {"Red Herb", 0x23},
            {"2x Green", 0x24},
            {"Green/Blue", 0x25},
            {"Green/Red", 0x26},
            {"3x Green", 0x27 },
            {"Green/Green/Blue", 0x28},
            {"Green/Red/Blue", 0x29},
            {"First Aid Spray Box", 0x2A},
            {"Square Crank", 0x2B},
            {"BOTU(RED)", 0x2C},
            {"BOTU(BLUE)", 0x2D},
            {"BOTU(GOLD)", 0x2E},
            {"Stars Card(Jill)", 0x2F},
            {"GIGA OIL", 0x30},
            {"Battery", 0x31},
            {"Fire Hook", 0x32},
            {"Power Cable", 0x33},
            {"Fuse", 0x34},
            {"Unknown Broken Fire Hose", 0x35},
            {"Oil Additive", 0x36},
            {"Brad Vicker's Card Case", 0x37},
            {"Stars Card (BRAD)", 0x38},
            {"Machine Oil", 0x39},
            {"Mixed Oil", 0x3A},
            {"Steel Chain", 0x3B},
            {"Wrench", 0x3C},
            {"Iron Pipe", 0x3D},
            {"Cylinder?", 0x3E},
            {"Fire Hose", 0x3F},
            {"Tape Recorder", 0x40 },
            {"Ligther Oil", 0x41},
            {"Lighter Case", 0x42},
            {"Ligther", 0x43},
            {"Green Gem", 0x44},
            {"Blue Gem", 0x45},
            {"Amber Ball", 0x46},
            {"Obsidian Ball", 0x47},
            {"Crystal Ball", 0x48},
            {"Demo Remote(No Batteries)", 0x49},
            {"Demo Remote(Batteries)", 0x4A},
            {"Remote Batteries", 0x4B},
            {"Gold Gear", 0x4C},
            {"Silver Gear", 0x4D},
            {"Chronos Gear", 0x4E},
            {"Bronze Book", 0x4F},
            {"Bronze Compass", 0x50},
            {"Vaccine Medium", 0x51},
            {"Vaccine Base", 0x52},
            {"Demo Sigpro_00", 0x53},
            {"Demo Sigpro_01", 0x54},
            {"Vaccine", 0x55},
            {"Demo_Sigpro_02" , 0x56},
            {"Demo SIgpro_03" , 0x57},
            {"Medium Base" , 0x58},
            {"Eagle Parts A", 0x59},
            {"Eagle Parts B", 0x5A},
            {"M37 Parts A", 0x5B},
            {"M37 Parts B", 0x5C},
            {"Demo Sigpro_04", 0x5D},
            {"Chronos Chain", 0x5E},
            {"Rusted Crank", 0x5F},
            {"Card Key", 0x60},
            {"Gun Powder A", 0x61},
            {"Gun Powder B", 0x62},
            {"Gun Powder C", 0x63},
            {"Gun Powder AA", 0x64},
            { "Gun Powder BB", 0x65},
            {"Gun Powder AC", 0x66},
            {"Gun Powder BC", 0x67},
            {"Gun Powder CC", 0x68},
            {"Gun Powder AAA", 0x69},
            {"Gun Powder AAB", 0x6A},
            {"Gun Powder BBA", 0x6B},
            {"Gun Powder BBB", 0x6C},
            {"Gun Powder CCC", 0x6D},
            {"Inf Ammo Case", 0x6E},
            {"Water Sample", 0x6F},
            {"System Disk", 0x70},
            {"Dummy Key", 0x71},
            {"Lockpick", 0x72},
            {"Warehouse Key", 0x73 },
            {"Sickroom Key",0x74},
            {"Stars Key",0x75},
            {"Beta Keyring",0x76},
            {"Clock Tower Key (BEZEL)",0x77},
            {"Clock Tower Key (WINDER)", 0x78},
            {"Chronos Key" , 0x79},
            {"Demo_Sigpro_05", 0x7A },
            {"Park Key(FRONT)" , 0x7B },
            {"Park Key(GRAVEYARD)" , 0x7C},
            {"Park Key(REAR)" , 0x7D },
            {"Facility Key(No Barcode)" , 0x7E},
            {"Facility Key(Barcode)" , 0x7F},
            {"Boutique Key" , 0x80},
            {"Ink Ribbon" , 0x81},
            {"Reloading Tool", 0x82  },
            {"Game Instructions A", 0x83},
            {"Game Instructions B", 0x84},
            {"Game Instructions C", 0x85},
            {"Kendo Fax", 0x96},
            {"Map of Uptown", 0xA4},
            {"Park Map", 0xA7 },
            {"RPD Map", 0xA9 },


        };




    }
}
