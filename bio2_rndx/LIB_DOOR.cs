using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bio2_rndx
{
    public static class LIB_DOOR
    {
        //SPADE KEY POSSIBLE DOOR OFFSETS
        // [0] Default Stars hall 
        // [1] Crow hall > helecopter
        // [2]
        // [3]
        // [4]
        // [5]
        // [6]
        // [7]
        // [8]
        // [9]
        // [10]
        //
        //
        public static int[] SPADE_OFFSETS = new int[] { 2591, 6663 };



        /// <summary>
        /// RDT INDEX / HEART_SPADE_DIAMOND_CLUB LOCK LIST
        /// </summary>
        public static Dictionary<int, int> HSDC_LOCKS = new Dictionary<int, int>()
        {
         //   {7, 2779}, // cabin (if cabin is != spade, u cant get diamond..)
           // {9, 6663}, // crow hall > helpecopter room
          //  {18, 10511}, // library > 3F door
          //  {19, 3405}, // blue coke > lib
            {20, 2591}, // original stars hall spot
            {34, 6459}, // licker hall > filing room
            {37, 3987}, // evidence room
            {41, 12119}, // east office heart key default door (warning this cant be club)
            {42, 7143}, // red hall > press conference
          //  {42, 7175}, // red hall > interogation can only have 1 or the other?
            {45, 6031}, // night watch room (club) default
            {47, 6367}, // basement autopsy (default club)
            
        };

        public static Dictionary<byte, string> LOCK_LUT = new Dictionary<byte, string>()
        {
            {0x59, "Spade"},
            {0x5A, "Diamond"},
            {0x5B, "Heart"},
            {0x5C, "Club"}
         
        };

        public static Dictionary<int, string> ROOM_LUT = new Dictionary<int, string>()
        {
            {20, "Stars Hall"},
            {34, "Filing Room"},
            {37, "Evidence Room"},
            {41, "East Office/Basement"},
            {42, "Red Hall"},
            {45, "Night Watch Room"},
            {47, "Basement Autopsy"},
        };


    }
}
