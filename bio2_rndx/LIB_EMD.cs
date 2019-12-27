using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bio2_rndx
{
    public static class LIB_EMD
    {
        public static Dictionary<byte, string> BIO2_EMD_LUT = new Dictionary<byte, string>()
        {// pose 18, status 5
           
            {0x10, "RPD COP"},
            {0x11, "Brad"},
            {0x12, "Zombie Variations"},

            {0x13, "Misty"},

            {0x15, "Lab Coat00"},
            {0x16, "Lab Coat01"},
            {0x17, "Naked"},
            {0x18, "Zombie (Civ)"},


            {0x1E, "Zombie Variant00"},
            {0x1F, "Zombie Variant00"},

            {0x20,  "Cerebus(Dog)"},
            {0x21, "Crow"},

            {0x22, "Regular Licker"},
            {0x23, "Alligator" },
            {0x24, "Super Licker" },
            {0x25,  "Spider"},
            {0x26,  "Spider mesh?"},
            {0x27,  "G-Parasite"},
            {0x28,  "G-Parasite"},
            {0x29,  "Cockrach"},

            {0x2A, "Tyrant Normal"},
            {0x2B, "Tyrant Normal"},
            {0x2C, "Tyrant Normal"},
            {0x2D, "Arms"},
            {0x2E, "Ivy?"},
            {0x2F, "Green Ivy"},

            {0x30,  "Birkin-1"},
            {0x31,  "Birkin-2"},

            {0x33, "Birkin 3rd form"},
            {0x34, "Birking 4th form"},
            {0x36, "Birkin Final"},
            {0x39,  "Ivy purple"},
            {0x3A, "Giant Moth" },
            {0x3C, "Tyrant Normal"},
            {0x3D, "Tyrant Normal"},
            {0x3E, "Tyrant Normal"},


            {0x40, "Chief Irons"},
            {0x41, "Ada Wong"},
            {0x43, "Ada Wong (Dead)"},
            {0x44, "Ben"},
            {0x45, "Ben"},
            {0x46, "Sherry Birkin"},
            {0x4A, "Marvin"},
        
          


        };

        /// <summary>
        /// store randomized emd data for log out..
        /// </summary>
        public struct EMD_OUT_OBJ {
            public string fname;
            public string roomname;
            public string enemy_name;
        }
        


    }
}
