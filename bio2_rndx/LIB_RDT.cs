using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace bio2_rndx
{
    public class LIB_RDT
    {
        public static string[] Current_SCD_Lines;
        public static string Selected_ByteStr;

        public static Dictionary<int, string> LUT_RDT_OFFSET = new Dictionary<int, string>()
        {
            {0, "Sound Effect Attributes (*.SND)"},
            {1, "Sound Effect Header (*.VH)"},
            {2, "Sound Effect Bank (*.VB)"},
            {3, "Sound Effect Header (*.VH) (UNUSED - Trial Edition, Only)" },
            {4, "Sound Effect Bank (*.VB) (UNUSED - Trial Edition, Only)" },
            {5, "UNUSED (.OTA)" },
            {6, "Collision Boundaries for 3D Models (*.SCA)"},
            {7, "Camera Positions & Targets, pointers to Camera Sprites (*.RID)"},
            {8, "Camera Zones/Switches (*.RVD)"},
            {9, "Lighting for 3D Models (*.LIT)"},
            {10, "3D Model pointer array (*.MD1;*.TIM)"},
            {11, "Floor data (*.FLR)"},
            {12, "Block data (*.BLK)" },
            {13, "Event Text/Message data (Japanese) (*.MSG)"},
            {14, "Event Text/Message data (Other) (*.MSG)"},
            {15, "Camera Scroll Texture (*.TIM)"},
            {16, "Initialization Script(s) (*.SCD)"},
            {17, "Execution Script(s) (*.SCD)"},
            {18, "Effect Sprite ID List (*.ESP)"},
            {19, "Effect Sprite Data (*.EFF)"},
            {20,  "Effect Sprite Texture (*.TIM)"},
            {21, "3D Model Textures (*.TIM) (sizeof RDT, if nOmodel=0)"},
            {22, "Animation Data for 3D Models (*.RBJ)"},

        };

        public static List<Int32> OFFSET_LIST = new List<int>();





        public struct RDT_OBJ // needs to be as many as files
        {
            public string f_rdt;
            public int[] Header_Offsets; // ptrs to 23 elements, various fmts
            public int[] Item_Aot_Offsets;
            public int[] Emd_Offsets;


            public int Item_Aot_Count; // total items in cur rdt
            public int EMD_COUNT;
            public int Item_tKeys;
            public int Item_tCommon;
            public ITEM_DATA_OBJ[] ITEM_AOTS; // store these on a room to room basis?
            public CK_DATA_OBJ[] CK_OPS;
            public EM_SET_OBJ[] EM_DATA; // CUR RDT's Array of EM_SETS..


            public List<int> LIST_EMD_OFFS;
            public List<int> List_Aot_Offs; // try a list vs the array
            public List<int> List_CK_OFFS;

        }

        // structure for seed format output
        public struct SEED_HEADER_OBJ
        {
            public byte Leon_Claire; // 0/1
            public byte G_Shuffle; // 0/1 hunk/tofu/a/b flips
            public byte E_Shuffle; // 0,1 enemy shuffle, on, off
            public byte P_Shuffle; // 0,1, on/off
            public Int16 Item_Count;
        }


        public struct SEED_ENTRY_OBJ
        {
            public string rdt_id;
            public byte count_per_room;
            public int[] Offsets;
            public ITEM_DATA_OBJ[] Item;

        }

        public struct SEED_ENT_OBJ
        {
            public string rdt_id;
            public byte count_per_room;
            public int Offset;
            public ITEM_DATA_OBJ ITEM;

        }

        public struct RDT_HEADER_OBJ
        {
            public byte nSPrite;  /* unknown */
            public byte nCut;     /* Amount of Camera arrays */
            public byte noModel; /* Amount of Object 3D models */
            public byte nItem;   /* Amount of Item 3D models (UNUSED) */
            public byte nDoor;   /* unknown */
            public byte nRoom_At; /* unknown */
            public byte Reverb_lv; /* unknown */
            public byte nSprite_Max; /* Sum total of individual Camera Sprites/Masks */

            public int[] nOffsets; //
            public int[] Item_Aot_Offsets;

            public string f_rdt;
            public int Item_Aot_Count; // total items in cur rdt
            public int Item_tKeys;
            public int Item_tCommon;

        }

        /// <summary>
        /// Holds current RDT's SCD_DATA INFO..
        /// </summary>
        public struct SCD_HEADER_OBJ
        {
            public Int32 O_SCDMAIN;
            public Int32 O_SCD_SUB;

            public Int16 nMain;
            public Int16 nSub; // divide these by 2 to get true scd length

            public Int32 SCDMAIN_SZ; // total size of main scd block
            public Int32 SCDSUB_SZ; // total size of sub scd block, which will be broken apart based on count

            public Int16[] O_SCD_SUBS; // array of pointers to 
            public int[] SZ_SDS;
            public int SUB_SCD_PTR_T; // total of all scd ptrs

        }


        /// <summary>
        ///  struct for holding / transfering selected em_set opcode data
        /// </summary>
        public struct EM_SET_OBJ
        {
            public byte _opdummy;
            public byte _ubyte00;
            public byte _emIndex;
            public byte _emdID;
            public byte _emPose;
            public short _AnimFlag00;
            public short _ushort00;
            public byte _SND;
            public byte _TEX;
            public byte _emFlag;
            public short _posx;
            public short _posy;
            public short _posz;
            public short _posr;
            public short _ushort01;
            public short _ushort02;

        }



        public struct ITEM_DATA_OBJ
        {
            public byte op;
            public byte aot;
            public byte id;
            public byte type;
            public byte floor;
            public byte super;
            public Int16 x;
            public Int16 y;
            public UInt16 w;
            public UInt16 d;
            public byte item;
            public byte amount;
            public Int16 flag;
            public byte md1;
            public byte ani;


        }






        public struct MD1_MODEL_OBJ
        {
            public byte op;
            public byte md1; // md1 ptr for rdt

        }


        //0x06
        public struct IFEL_CK_OBJ
        {
            public byte opcode;
            public byte dummy;
            public short blk_len; // length of block
        }


        public struct CK_DATA_OBJ
        {
            public byte op;
            public byte bit_array;
            public byte num;
            public byte val;

        }




        /// <summary>
        /// struct for holding current rdt's message data
        /// </summary>
        public struct MSG_BLK_OBJ
        {
            public Int16[] Msg_Ptrs;
            public Int16 Total;
            public string[] Msgs;

        }

        public struct MsgAot_Obj
        {
            public byte[] Msg_Data;
            public string bytestr;
            public string Msg;
            public int msg_sz;
        }




        // MISC STRUCTS FOR EXTRA TROLLING
        public struct SAFE_COMBO_OBJ
        {
            public byte[] SafeArray;

        }


        public struct TORCH_COMBO_OBJ
        {
            public Int16[] AOT_X;
            public Int16[] ESPR_X;

        }


        public enum ItemClass
        {
            BLUECARD,
            FILM_KEYS,
            SUIT_KEYS,
            OBJ_KEYS,
            SEWER_KEYS,
            LAB_KEYS

        }
        
        public struct BOX_POS_OBJ
        {


            public Int16 box1_1X;
            public Int16 box1_1Z;
            public Int16 box1_1R;

            public Int16 box2_1X;
            public Int16 box2_1Z;
            public Int16 box2_1R;

            public Int16 box3_1X;
            public Int16 box3_1Z;
            public Int16 box3_1R;

        }
        // for trolling pushable statue positions
        public struct STATUE_POS_OBJ
        {
            public Int16 statue_1X;
            public Int16 statue_1Z;
            public Int16 statue_1R;

            public Int16 statue_2X;
            public Int16 statue_2Z;
            public Int16 statue_2R;

            
        }

        

        // return number of total common items
        public int Return_Total_Common(ITEM_DATA_OBJ[] ITEM_AOT)
        {
            int n = 0;

            for (int i = 0; i < ITEM_AOT.Length; i++)
            {
                if (ITEM_AOT[i].item >= 0x00 && ITEM_AOT[i].item <= 0x2E)
                {
                    n++;
                }
            }

            return n;
        }

        // return number of total Key items
        public int Return_Total_KeyMisc(ITEM_DATA_OBJ[] ITEM_AOT)
        {
            int n = 0;
            for (int i = 0; i < ITEM_AOT.Length; i++)
            {
                if (ITEM_AOT[i].item >= 0x2E && ITEM_AOT[i].item <= 0x6F)
                {
                    n++;
                }

            }
            return n;
        }



        /// <summary>
        /// Collects index's of flagged items
        /// </summary>
        /// <param name="TargetIndexs">Index list</param>
        /// <param name="Item">ITEM ID</param>
        /// <param name="indexer">incrementer</param>
        /// <param name="Modifier">0 = RPD || 1 = SEWER || 2 = LAB || 3 = SUIT KEYS </param>
        public static void IndexBuilder(List<int> TargetIndexs, byte Item, int indexer, byte modifier)
        {



            if (modifier == 0)
            {

                switch (Item)
                {

                    case 0x1F:
                    case 0x32:
                    case 0x33:
                    case 0x34:
                    case 0x43:

                    case 0x47:
                    case 0x4A:
                    case 0x4B:

                    case 0x53:


                    case 0x36:
                    case 0x37:
                    case 0x38:
                    case 0x39:
                    case 0x3A:


                    case 0x3B: // plugs
                    case 0x3C:
                    case 0x3D:
                    case 0x3E:



                        //  case 0x48:
                        // case 0x49:

                        //case 0x59:
                        //case 0x5A:
                        //case 0x5B: // keys, sewer stuff
                        //case 0x5C:
                        //  case 0x5D:
                        //   case 0x60:
                        //  case 0x61:




                        TargetIndexs.Add(indexer - 1);
                        //    Console.WriteLine(indexer - 1 + " : " + LIB_ITEM.BIO2_ITEM_LUT[Item]);

                        break;
                }

            }


            if (modifier == 1) // sewer
            {

                switch (Item)
                {

                    case 0x3F: // weapon storage
                    case 0x48: // eagle
                    case 0x49: // wolf
                    case 0x5D: // panel key



                        TargetIndexs.Add(indexer - 1);

                        //    Console.WriteLine(indexer - 1 + " : " + LIB_ITEM.BIO2_ITEM_LUT[Item]);

                        break;
                }

            }


            if (modifier == 2) // lab
            {

                switch (Item)
                {
                    case 0x4C:
                    case 0x54:
                    case 0x55:
                    case 0x60:
                    case 0x61:
                    case 0x63:

                        TargetIndexs.Add(indexer - 1);

                        //   Console.WriteLine(indexer - 1 + " : " + LIB_ITEM.BIO2_ITEM_LUT[Item]);

                        break;
                }

            }


            if (modifier == 3)
            {
                switch (Item)
                {
                    case 0x59:
                    case 0x5A:
                    case 0x5B:
                    case 0x5C:

                        TargetIndexs.Add(indexer - 1);

                        //    Console.WriteLine(indexer - 1 + " : " + LIB_ITEM.BIO2_ITEM_LUT[Item]);


                        break;

                }
            }

        }

        /// <summary>
        /// Kills the possibility of swapping with key items
        /// </summary>
        /// <param name="TargetIndes"></param>
        /// <param name="Return_Shuffle"></param>
        public static void ListCleaner(List<int> TargetIndes, List<LIB_RDT.ITEM_DATA_OBJ> Return_Shuffle)
        {
            // will store index's to clean
            List<int> Cleaner = new List<int>();

            for (int i = 0; i < TargetIndes.Count; i++)
            {
                if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[i].item))
                {
                    Cleaner.Add(i);
                }
            }

            // remove flagged indices from list =]
            foreach (int i in Cleaner)
            { //Console.WriteLine("removing[" + i + "] - " + LIB_ITEM.BIO2_KEY_LUT_LA[Return_Shuffle[i].item]); 
                TargetIndes.Remove(i);
            }

        }


        public static void ListTransfer(List<int> TargetList, List<int> TransferList, int numRead, ItemClass Class)
        {

            for (int i = numRead; i < TargetList.Count; i++)
            {
                TransferList.Add(TargetList[i]);
                //  Console.WriteLine("Adding " + TargetList[i] + " to " + Class.ToString() + "LIST");
            }


        }




        public static void Integrity_Check(List<LIB_RDT.ITEM_DATA_OBJ> Return_Shuffle, byte Debug_Flag)
        {

            Random R_Swap = new Random();

            int[] DiamondErrors = new int[] { 51, 52, 34, 35, 36, 61, 63, 64, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99,100, 101, 102, 104, 107, 108, 109, 110, 111, 112, 113, 114, 115, 120, 121, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134 };
            int[] SpadeErrors = new int[] { 38, 39, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100,101, 102, 104, 107, 108, 109, 110, 111, 112, 113, 114, 115, 120, 121, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134 };
            int[] ClubErrors = new int[] { 67, 68, 59, 60, 69,75, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99,100, 101, 102,103, 104, 107, 108, 109, 110, 111, 112, 113, 114, 115, 120, 121, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134 };
            int[] HeartErrors = new int[] { 65, 66, 75, 76, 71, 72, 73, 74, 77, 78, 79, 80, 81, 82 };
            int[] ValveErrors = new int[] { 10, 11, 12, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100,101, 102,103, 104, 107, 108, 109, 110, 111, 112, 113, 114, 115, 120, 121, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134 };
            int[] RedCardErrors = new int[] { 71, 72, 73, 74, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100,101, 103,102, 104, 107, 108, 109, 110, 111, 112, 113, 114, 115, 120, 121, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134 };

            int[] SmallKeyErrors = new int[] { 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99,100, 101, 102, 103,104, 107, 108, 109, 110, 111, 112, 113, 114, 115, 120, 121, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134 };

            int[] MainFuseErrors = new int[] { 126, 127, 132, 133, 134, 136, 138 };


            int[] ControlPanelKeyErrors = new int[] { 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 128, 127, 129, 130, 131, 132, 133, 134, 136, 137, 138, 139, 140, 141 };

            int[] WolfMedalErrors = new int[] { 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 128, 127, 129, 130, 131, 132, 133, 134, 136, 137, 138, 139, 140, 141 };
            int[] EagleMedalErrors = new int[] { 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 128, 127, 129, 130, 131, 132, 133, 134, 136, 137, 138, 139, 140, 141 };



            int[] UmbrellaCardErrors = new int[] { 127,128,129};


            int voidRoll = 0;
            int InkIdx = 0;

            List<int> NonKeyIdx = new List<int>();

            List<int> KeyIdx = new List<int>();

            List<int> WeaponIdx = new List<int>();



            int[] PuzzleReturns = new int[] { 30, 47, 48, 49, 50, 33, 11, 58, 13, 17, 59, 71, 72, 73, 74, 19 };

            bool Diamond = false;
            bool Spade = false;
            bool Club = false;
            bool Heart = false;
            bool Valve = false;
            bool RedCard = false;
            bool filmA = false;
            bool filmB = false;
            bool filmC = false;
            bool filmD = false;

            int MoDiskIDX = 0;
            int MainFuseIDX = 0;


            // films should not be on any of these because films return these items

            int[] FilmErrors = new int[] { 10, 11, 12, 51, 52, 34, 35, 36, 61, 63, 64, 38, 39, 40, 67, 68, 59, 60, 75, 65, 66, 76, 71, 73, 74, 77, 78, 79, 80, 81, 82, 30, 47, 48, 49, 50, 33, 19, 11, 58, 13, 17, 59, 72, 74, 74 };


            // check mysterious void index
            //if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[0].item))
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("ERROR KEY DETECTED in void index [" + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[0].item] + "]");
            //}
            //else {
            //    Console.ForegroundColor = ConsoleColor.Green;
            //    Console.WriteLine("Void Index is Clean [" + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[0].item] + "]");
            //}



            for (int p = 0; p < PuzzleReturns.Length; p++)
            {
                if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[PuzzleReturns[p]].item))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                if (Debug_Flag == 1)
                {



                    Console.WriteLine(PuzzleReturns[p] + " " + LIB_ITEM.BIO2_PUZZLE_LUT[(byte)PuzzleReturns[p]] + " [" + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[PuzzleReturns[p]].item] + "]");
                    //  Console.WriteLine(PuzzleReturns[p] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[p].item]);
                }
            }



            //Console.ForegroundColor = ConsoleColor.Cyan;
            //for (int i = 0; i < Return_Shuffle.Count; i++)
            //{
            //    if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[i].item))
            //    {
            //        Console.WriteLine(i + ") " + LIB_ITEM.BIO2_KEY_LUT_LA[Return_Shuffle[i].item]);
            //    }
            //}


            for (int i = 0; i < Return_Shuffle.Count; i++)
            {

                if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[i].item)) { KeyIdx.Add(i); }
                if (LIB_ITEM.BIO2_COMMON_LUT_LA.ContainsKey(Return_Shuffle[i].item)) { NonKeyIdx.Add(i); }
                if (Return_Shuffle[i].item == 0x02 || Return_Shuffle[i].item == 0x05 || Return_Shuffle[i].item == 0x07 || Return_Shuffle[i].item == 0x10)
                {
                    WeaponIdx.Add(i);
                }

                //if (Return_Shuffle[i].item == 0x1E)
                //{
                //    InkIdx = i;
                //}

                //if (i == 142)
                //{
                //    Return_Shuffle[i] = Return_Shuffle[InkIdx];
                //}

            }


            // handle the void index
            voidRoll = NonKeyIdx.First(x => x > 45 && Return_Shuffle[x].item == 0x1E);
            int PlatFormKeyIdx = 141;
            //   voidRoll = Return_Shuffle.IndexOf(Return_Shuffle.Where(x => Return_Shuffle[x].item == 0x1E));


            if (Debug_Flag == 1)
            {
                Console.WriteLine("VOID INDEX [" + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[0].item] + " > " + voidRoll + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[voidRoll].item]);
            }
            LIB_RDT.ITEM_DATA_OBJ VOID_ITEM = Return_Shuffle[0];
            Return_Shuffle[voidRoll] = VOID_ITEM;





            //Console.ForegroundColor = ConsoleColor.DarkBlue;

            // for all scanned weapons indexs
            for (int i = 0; i < WeaponIdx.Count; i++)
            {
                //Console.WriteLine("\n\nNo Weapons found below 20");

                // if shotgun > 45
                if (WeaponIdx[i] > 45 && Return_Shuffle[WeaponIdx[i]].item == 0x07)
                {

                    byte FOL = (byte)R_Swap.Next(0, 4);
                    int result = 0;

                    // find first result greater then 15 but less then 45
                    if (FOL == 0) { result = NonKeyIdx.First(x => x < 45 && x >= 12); }
                    // find last result non key idx < 45
                    if (FOL == 1) { result = NonKeyIdx.Last(x => x < 45 && x >= 12); }

                    if (FOL == 2) { result = NonKeyIdx.Last(x => x < 45 && x >= 15); }

                    if (FOL == 3) { result = NonKeyIdx.First(x => x < 45 && x >= 15); }


                    if (Debug_Flag == 1)
                    {
                        Console.WriteLine("Shotgun result == " + result);
                    }
                    //clone an item from the new roll
                    LIB_RDT.ITEM_DATA_OBJ OLD_ITEM = Return_Shuffle[result];

                    // newly rolled spot = shotgun
                    Return_Shuffle[result] = Return_Shuffle[WeaponIdx[i]];

                    // shotgun = old item
                    Return_Shuffle[WeaponIdx[i]] = OLD_ITEM;

                    NonKeyIdx.Remove(result);

                    break;

                }

            }


            for (int i = 0; i < Return_Shuffle.Count; i++)
            {

                if (Return_Shuffle[i].item == 0x60) { MoDiskIDX = i; }
                if (Return_Shuffle[i].item == 0x4C) { MainFuseIDX = i; }


                for (int x = 0; x < DiamondErrors.Length; x++)
                {
                    if (Return_Shuffle[i].item == 0x5A && i == DiamondErrors[x])
                    {
                        Diamond = false;

                        if (Debug_Flag == 1) { Console.WriteLine("Diamond Error"); }

                    }
                    //else {

                    //    Diamond = true;
                    //}
                }

                for (int y = 0; y < SpadeErrors.Length; y++)
                {
                    if (Return_Shuffle[i].item == 0x59 && i == SpadeErrors[y])
                    {
                        Spade = false;
                        if (Debug_Flag == 1) { Console.WriteLine("Spade Error"); }
                    }
                    //else {
                    //    Spade = true;
                    //}
                }

                for (int j = 0; j < ClubErrors.Length; j++)
                {
                    if (Return_Shuffle[i].item == 0x5C && i == ClubErrors[j])
                    {
                        Club = false;
                        if (Debug_Flag == 1) { Console.WriteLine("Club Error"); }
                    }
                    //else {
                    //    Club = true;
                    //}

                }

                for (int t = 0; t < HeartErrors.Length; t++)
                {

                    if (Return_Shuffle[i].item == 0x5B && i == HeartErrors[t])
                    {
                        Heart = false;
                        if (Debug_Flag == 1) { Console.WriteLine("Heart Error"); }
                    }
                    //else {
                    //    Heart = true;
                    //}
                }


                // films
                for (int f = 0; f < FilmErrors.Length; f++)
                {

                    if (Return_Shuffle[i].item == 0x44 && i == FilmErrors[f])
                    {
                        filmA = false;
                        if (Debug_Flag == 1) { Console.WriteLine("Film A Error"); }


                        // new result = first non key ietm that is not a film error
                        int result = NonKeyIdx.First(x => x != FilmErrors[f] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1) { Console.WriteLine("FILM A SWAP NOW " + result + " FROM " + i); }



                    }
                    //else {
                    //    filmA = true;
                    //}


                    if (Return_Shuffle[i].item == 0x45 && i == FilmErrors[f])
                    {

                        if (Debug_Flag == 1)
                        {
                            Console.WriteLine("Film B Error");
                        }



                        int result = NonKeyIdx.First(x => x != FilmErrors[f] && x > 0);

                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);


                        if (Debug_Flag == 1)
                        {
                            Console.WriteLine("FILM B SWAP NOW " + result + " FROM " + i);
                        }


                        filmB = false;

                    }
                    //else {
                    //    filmB = true;
                    //}

                    if (Return_Shuffle[i].item == 0x46 && i == FilmErrors[f])
                    {
                        if (Debug_Flag == 1)
                        {
                            Console.WriteLine("Film C Error");
                        }

                        int result = NonKeyIdx.First(x => x != FilmErrors[f] && x > 0);

                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1)
                        {
                            Console.WriteLine("FILM C SWAP NOW " + result + " FROM " + i);
                        }

                        filmC = false;

                    }
                    //else
                    //{
                    //    filmC = true;
                    //}


                    if (Return_Shuffle[i].item == 0x50 && i == FilmErrors[f])
                    {
                        Console.WriteLine("Film D Error");

                        int result = NonKeyIdx.First(x => x != FilmErrors[f] && x > 0);

                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1)
                        {
                            Console.WriteLine("FILM D SWAP NOW " + result + " FROM " + i);
                        }

                        filmD = false;

                    }
                    //else {
                    //    filmD = true;
                    //}

                }

                //valve
                for (int v = 0; v < ValveErrors.Length; v++)
                {
                    if (Return_Shuffle[i].item == 0x32 && i == ValveErrors[v])
                    {
                        Valve = false;
                        Console.WriteLine("Valve Error Error");
                        // set result swap index to whatever non key idx that isnt a valve error


                        int result = NonKeyIdx.First(x => x != ValveErrors[v] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1)
                        { Console.WriteLine("VALVE SWAP NOW " + result + " FROM " + i); }


                    }
                    //else
                    //{
                    //    Valve = true;
                    //}
                }


                // for every red card error value
                for (int r = 0; r < RedCardErrors.Length; r++)
                {
                    // if return shuffle item == red card and its one of the red card errors
                    if (Return_Shuffle[i].item == 0x34 && i == RedCardErrors[r])
                    {
                        int result = NonKeyIdx.First(x => x != RedCardErrors[r] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1)
                        {
                            Console.WriteLine("RED CARD SWAP NOW " + result + " FROM " + i);
                        }


                    }
                    //else
                    //{
                    //    RedCard = true;
                    //}
                }


                for (int s = 0; s < SmallKeyErrors.Length; s++)
                {
                    if (Return_Shuffle[i].item == 0x1F && i == SmallKeyErrors[s])
                    {
                        // int SwapRoll = R_Swap.Next(2, 31);

                        int result = NonKeyIdx.First(x => x != SmallKeyErrors[s] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1)
                        { Console.WriteLine("SMALL KEY SWAP NOW " + result + " FROM " + i); }

                    }


                }


                for (int r = 0; r < MainFuseErrors.Length; r++)
                {
                    // if return shuffle item == red card and its one of the red card errors
                    if (Return_Shuffle[i].item == 0x4C && i == MainFuseErrors[r])
                    {
                        int result = NonKeyIdx.First(x => x != MainFuseErrors[r] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1)
                        { Console.WriteLine("MAIN FUSE SWAP NOW " + result + " FROM " + i); }


                    }
                    //else
                    //{
                    //    RedCard = true;
                    //}
                }



                for (int r = 0; r < ControlPanelKeyErrors.Length; r++)
                {
                    // if return shuffle item == red card and its one of the red card errors
                    if (Return_Shuffle[i].item == 0x4C && i == ControlPanelKeyErrors[r])
                    {
                        int result = NonKeyIdx.First(x => x != ControlPanelKeyErrors[r] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1)
                        {
                            Console.WriteLine("CONTROL PANEL KEY SWAP NOW " + result + " FROM " + i);
                        }


                    }
                    //else
                    //{
                    //    RedCard = true;
                    //}
                }

                for (int r = 0; r < WolfMedalErrors.Length; r++)
                {
                    // if return shuffle item == red card and its one of the red card errors
                    if (Return_Shuffle[i].item == 0x49 && i == WolfMedalErrors[r])
                    {
                        int result = NonKeyIdx.First(x => x != WolfMedalErrors[r] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1)
                        { Console.WriteLine("WOLF MEDAL SWAP NOW " + result + " FROM " + i); }


                    }
                    //else
                    //{
                    //    RedCard = true;
                    //}
                }



                for (int r = 0; r < EagleMedalErrors.Length; r++)
                {
                    // if return shuffle item == red card and its one of the red card errors
                    if (Return_Shuffle[i].item == 0x49 && i == EagleMedalErrors[r])
                    {
                        int result = NonKeyIdx.First(x => x != EagleMedalErrors[r] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1) { Console.WriteLine("EAGLE MEDAL SWAP NOW " + result + " FROM " + i); }


                    }
                    //else
                    //{
                    //    RedCard = true;
                    //}
                }

                for (int r = 0; r < UmbrellaCardErrors.Length; r++)
                {
                    // if return shuffle item == red card and its one of the red card errors
                    if (Return_Shuffle[i].item == 0x61 && i == UmbrellaCardErrors[r])
                    {
                        int result = NonKeyIdx.First(x => x != UmbrellaCardErrors[r] && x > 0);


                        // new instance of item at above result
                        LIB_RDT.ITEM_DATA_OBJ NewSpot = Return_Shuffle[result];

                        // new result spot is now equal film
                        Return_Shuffle[result] = Return_Shuffle[i];

                        // bad film spot is now new result item
                        Return_Shuffle[i] = NewSpot;

                        // new spot is now equal bad 

                        NonKeyIdx.Remove(result);

                        if (Debug_Flag == 1) { Console.WriteLine("UMBRELLA CARD KEY SWAP NOW " + result + " FROM " + i); }


                    }
                    //else
                    //{
                    //    RedCard = true;
                    //}
                }

            }



            //if (Spade == true && Heart == true && Diamond == true && Club == true && Valve == true && RedCard == true && filmA == true && filmB == true && filmC == true && filmD == true)
            //{
            //    Console.ForegroundColor = ConsoleColor.Green;
            //    Console.WriteLine("SEED PASSED INTEGRITY CHECk");
            //}
            //else {
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("SEED FAILED INTEGRITY CHECk");

            //    //  re run app?
            //    //string[] args = new string[] {"-l", "-em_on", "-gs_on", "-pz_on" };

            //    //Program.Main(args);

            //}

        }





        public static void Prompt_Swap(List<LIB_RDT.ITEM_DATA_OBJ> Return_Shuffle)
        {

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n\n -------------------ITEM LIST CONTROL OPTIONS ----------------------\n");
            Console.WriteLine("1) Swap X Over Ink Ribbons");
            Console.WriteLine("2) Swap X Over First Aid Sprays");
            Console.WriteLine("3) Swap X Over all instances of X");
            Console.WriteLine("\n PRESS ANY OTHER KEY TO SKIP");


            ConsoleKeyInfo opt = Console.ReadKey();

            if (opt.Key == ConsoleKey.D1)
            {

                Console.WriteLine("OPTION 1");

                foreach (KeyValuePair<byte, string> ItemEntry in LIB_ITEM.BIO2_ITEM_LUT)
                {
                    Console.WriteLine(ItemEntry.Key + "] " + ItemEntry.Value);
                }


                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Select above item value to write over Ink Ribbons\n");

                string Iselect = Console.ReadLine();

                Console.WriteLine(LIB_ITEM.BIO2_ITEM_LUT[byte.Parse(Iselect)] + " Selected");

                Console.WriteLine("Select Desired Quantity\n");

                string QuanSelect = Console.ReadLine();

                LIB_RDT.ITEM_DATA_OBJ newItem = new LIB_RDT.ITEM_DATA_OBJ();

                newItem.item = byte.Parse(Iselect);
                newItem.amount = byte.Parse(QuanSelect);

                for (int i = 0; i < Return_Shuffle.Count; i++)
                {

                    if (Return_Shuffle[i].item == 0x1E)
                    {
                        Return_Shuffle[i] = newItem;

                    }

                }

                Console.WriteLine("ALL INKS HAVE BEEN SWAPPED WITH " + LIB_ITEM.BIO2_ITEM_LUT[byte.Parse(Iselect)]);

                //Console.ForegroundColor = ConsoleColor.White;

                //for (int i = 0; i < Return_Shuffle.Count; i++)
                //{
                //    Console.WriteLine(i + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[i].item]);

                //}

                //foreach (LIB_RDT.ITEM_DATA_OBJ Item in Return_Shuffle.Where(x => x.item == 0x1E))
                //{
                // Return_Shuffle

                //}

            }

            if (opt.Key == ConsoleKey.D2)
            {

                Console.WriteLine("OPTION 2");
                foreach (KeyValuePair<byte, string> ItemEntry in LIB_ITEM.BIO2_ITEM_LUT)
                {
                    Console.WriteLine(ItemEntry.Key + "] " + ItemEntry.Value);
                }


                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Select above item value to write over First Aid Sprays\n");

                string Iselect = Console.ReadLine();

                Console.WriteLine(LIB_ITEM.BIO2_ITEM_LUT[byte.Parse(Iselect)] + " Selected");

                Console.WriteLine(" Select Desired Quantity\n");

                string QuanSelect = Console.ReadLine();

                LIB_RDT.ITEM_DATA_OBJ newItem = new LIB_RDT.ITEM_DATA_OBJ();

                newItem.item = byte.Parse(Iselect);
                newItem.amount = byte.Parse(QuanSelect);

                for (int i = 0; i < Return_Shuffle.Count; i++)
                {
                    if (Return_Shuffle[i].item == 0x23)
                    {
                        Return_Shuffle[i] = newItem;
                    }
                }

                Console.WriteLine("ALL FAS HAVE BEEN SWAPPED WITH " + LIB_ITEM.BIO2_ITEM_LUT[byte.Parse(Iselect)]);
            }


            if (opt.Key == ConsoleKey.D3)
            {

                Console.WriteLine("OPTION 3");
                foreach (KeyValuePair<byte, string> ItemEntry in LIB_ITEM.BIO2_ITEM_LUT)
                {
                    Console.WriteLine(ItemEntry.Key + "] " + ItemEntry.Value);
                }


                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Choose Item to Overwrite (All occurences)\n");

                string Iselect00 = Console.ReadLine();

                Console.WriteLine(LIB_ITEM.BIO2_ITEM_LUT[byte.Parse(Iselect00)] + "Selected");

                Console.WriteLine(" Select Replacement Item\n");

                string Iselect01 = Console.ReadLine();

                Console.WriteLine(LIB_ITEM.BIO2_ITEM_LUT[byte.Parse(Iselect01)] + "Selected ");

                LIB_RDT.ITEM_DATA_OBJ newItem = new LIB_RDT.ITEM_DATA_OBJ();



                Console.WriteLine("Set Replacement Item Quantity \n");
                string SelQuan = Console.ReadLine();


                newItem.item = byte.Parse(Iselect01);
                newItem.amount = byte.Parse(SelQuan);


                for (int i = 0; i < Return_Shuffle.Count; i++)
                {
                    if (Return_Shuffle[i].item == byte.Parse(Iselect00))
                    {
                        Return_Shuffle[i] = newItem;

                    }
                }

            }

        }

        // ########################### EXCLUDE LIST ###########################
        ///
        ///                        ROOMA0E0.RDT [AV BOMB]
        //ROOMA110.RDT[AV BOMB]
        //ROOMB060.RDT [AV BOMB]
        //ROOMB0D0.RDT [AV BOMB]

        //ROOMB100.RDT[AV BOMB]

        //ROOMC010.RDT[AV BOMB]


        //ROOMD090.RDT[ROCKET 10]

        //ROOMF030.RDT[RED HERB x2]
        //ROOMA060.RDT[RED HERB x2 ?] ?? parking lot?

        /// 
        /// 
        /// #####################################################################
        /// <summary>
        /// Reads Item Aots using rdt relative count
        /// you should make clones of these to only store A/B/EX3 or all?
        /// </summary>
        /// <param name="bufferx"></param>
        /// <param name="enforcer"></param>

        public void Read_Item_Aots(FileStream fs, BinaryReader br, int z, ITEM_DATA_OBJ[] ITEM_AOT, List<ITEM_DATA_OBJ> AllItems, byte DEBUG_MODE)
        {
            // read struct
            ITEM_AOT[z].op = br.ReadByte(); //1
            ITEM_AOT[z].aot = br.ReadByte(); // 2
            ITEM_AOT[z].id = br.ReadByte(); // 3
            ITEM_AOT[z].type = br.ReadByte(); // 4
            ITEM_AOT[z].floor = br.ReadByte(); // 5
            ITEM_AOT[z].super = br.ReadByte(); // 6
            ITEM_AOT[z].x = br.ReadInt16(); // 8
            ITEM_AOT[z].y = br.ReadInt16(); // 10
            ITEM_AOT[z].w = br.ReadUInt16(); //12
            ITEM_AOT[z].d = br.ReadUInt16(); // 14
            ITEM_AOT[z].item = br.ReadByte(); // 16
            fs.Seek(+1, SeekOrigin.Current);
            ITEM_AOT[z].amount = br.ReadByte(); //18
            fs.Seek(+1, SeekOrigin.Current);
            ITEM_AOT[z].flag = br.ReadInt16(); // 20
            ITEM_AOT[z].md1 = br.ReadByte(); // 21
            ITEM_AOT[z].ani = br.ReadByte(); // 22
            Console.ForegroundColor = ConsoleColor.Green;

            // if cur item id is in table
            if (LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(ITEM_AOT[z].item))
            {
                if (DEBUG_MODE == 1)
                {
                    Console.WriteLine("Item ID:[0x" + ITEM_AOT[z].item.ToString("X") + "]" + LIB_ITEM.BIO2_ITEM_LUT[ITEM_AOT[z].item] + "\\" + ITEM_AOT[z].amount);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(" Quantity: " + ITEM_AOT[z].amount + " MD1: " + ITEM_AOT[z].md1);
                }

            }
            else
            {
                if (DEBUG_MODE == 1)
                {
                    Console.WriteLine("**[NOTE] : **" + ITEM_AOT[z].item.ToString("X"));
                }
            }

            //Item_Aots.Add(ITEM_AOT[z]); // store all item aots
            //  bool alreadyExists = AllItems.Any(x => x.item == 0x5c);

            //  if (!alreadyExists && ITEM_AOT[z].item == 0x5C)
            // {
            AllItems.Add(ITEM_AOT[z]);
            // }


        }

        public void Read_EM_SETS(FileStream fs, BinaryReader br, int t, EM_SET_OBJ[] EM_SET)
        {

            EM_SET[t]._opdummy = br.ReadByte();
            EM_SET[t]._emIndex = br.ReadByte();
            EM_SET[t]._emdID = br.ReadByte();
            EM_SET[t]._emPose = br.ReadByte();
            EM_SET[t]._AnimFlag00 = br.ReadInt16();
            EM_SET[t]._ushort00 = br.ReadInt16();
            EM_SET[t]._SND = br.ReadByte();
            EM_SET[t]._TEX = br.ReadByte();
            EM_SET[t]._emFlag = br.ReadByte();
            EM_SET[t]._posx = br.ReadInt16();
            EM_SET[t]._posz = br.ReadInt16();
            EM_SET[t]._posy = br.ReadInt16();
            EM_SET[t]._posr = br.ReadInt16();
            EM_SET[t]._ushort01 = br.ReadInt16();
            EM_SET[t]._ushort02 = br.ReadInt16();

        }



        /// <summary>
        /// dump seed to binary format
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bw"></param>
        /// <param name="room_item_count"></param>
        /// <param name="item_id"></param>
        /// <param name="quantity"></param>
        /// <param name="anim"></param>
        public static void Dump_Seed(SEED_ENT_OBJ[] SEED_ENTRIES, SEED_HEADER_OBJ SEED_HEADER)
        {

            string seed_dir = AppDomain.CurrentDomain.BaseDirectory + "\\Seeds\\";
            string output = string.Empty;



            //  SEED_HEADER.Item_Count = (Int16)total_items;
            SEED_HEADER.Leon_Claire = 0;
            SEED_HEADER.E_Shuffle = 0;
            SEED_HEADER.P_Shuffle = 0;
            SEED_HEADER.G_Shuffle = 0;


            // create dir if not there
            if (!Directory.Exists(seed_dir))
            {
                Directory.CreateDirectory(seed_dir);
            }

            // calculate total num of seeds/fil;es
            int seed_count = Directory.GetFiles(seed_dir).Length;


            // generated output
            output = "BIO2_LA_Seed_" + seed_count;

            // if current genereaqted file output does note xist, open streams, write data
            if (!File.Exists(seed_dir + output))
            {
                using (FileStream fs = new FileStream(seed_dir + output + ".isd", FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        fs.Seek(0, SeekOrigin.Begin);

                        // write small header
                        bw.Write(SEED_HEADER.Item_Count);
                        //bw.Write(SEED_HEADER.Leon_Claire);

                        for (int i = 0; i < SEED_ENTRIES.Length; i++)
                        {
                            // Console.WriteLine(SEED_ENTRIES[i].rdt_id);
                            if (SEED_ENTRIES[i].rdt_id != null)
                            {

                                bw.Write(SEED_ENTRIES[i].rdt_id);
                                //  bw.Write(SEED_ENTRIES[i].count_per_room);
                                bw.Write(SEED_ENTRIES[i].Offset);
                                bw.Write(SEED_ENTRIES[i].ITEM.item);
                                bw.Write(SEED_ENTRIES[i].ITEM.amount);
                                bw.Write(SEED_ENTRIES[i].ITEM.ani);

                            }
                            //for (int y = 0; y < SEED_ENTRIES[i].count_per_room; y++)
                            //{
                            //    //  Console.WriteLine(SEED_ENTRIES[i].Item[y].item + ":"+ SEED_ENTRIES[i].Item[y].amount);

                            //        bw.Write((byte)y);
                            //        bw.Write(SEED_ENTRIES[i].Offsets[y]);
                            //        bw.Write(SEED_ENTRIES[i].Item[y].item);
                            //        bw.Write(SEED_ENTRIES[i].Item[y].amount);
                            //        bw.Write(SEED_ENTRIES[i].Item[y].ani);


                            //}

                        }



                    }




                }


            }








        }


        /// <summary>
        /// Scans scd buffer using difficulty flags
        /// </summary>
        /// <param name="scd_buffer"> The buffer to scan</param>
        /// <param name="RDT_DATA"> Where the CK OFfsets are stored</param>
        /// <param name="rdt_index">Current relative file index</param>
        /// <param name="DFC_FLAG">THe difficulty flag</param>
        /// <param name="SCD_MAIN_OFF">Array of SCD MAIN OFFS, Just pass relative index</param>
        public void ARRANGE_FILTER(byte[] scd_buffer, RDT_OBJ[] RDT_DATA, int rdt_index, int[] SCD_MAIN_OFF)
        {

            LIB_RDT.CK_DATA_OBJ TEMP_CK = new LIB_RDT.CK_DATA_OBJ();
            int CK_COUNT = 0;

            // if DFC FLAG == 0, easy ..
            // if DFC FLAG == 1, Normal..

            for (int x = 0; x < scd_buffer.Length; x++)
            {
                if (x != scd_buffer.Length)
                {
                    // if op ck
                    if (scd_buffer[x] == 0x21)
                    {
                        // if op ck confirm
                        if (scd_buffer[x + 3] == 0x01 || scd_buffer[x + 3] == 0x00)
                        {

                            TEMP_CK.op = scd_buffer[x];
                            TEMP_CK.bit_array = scd_buffer[x + 1];
                            TEMP_CK.num = scd_buffer[x + 2];
                            TEMP_CK.val = scd_buffer[x + 3];

                            CK_COUNT += 1;

                            RDT_DATA[rdt_index].List_CK_OFFS.Add(x + SCD_MAIN_OFF[rdt_index]);

                            // if diff chk
                            if (TEMP_CK.bit_array == 0x00 && TEMP_CK.num == 0x19)
                            {
                                // normal
                                if (TEMP_CK.val == 0x00)
                                {

                                    // calculate if len
                                    short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                    // calculate else location
                                    int else_loc = x + ifblkLen - 4;

                                    while (x != else_loc)
                                    {
                                        x++;
                                    }

                                    if (x == else_loc && scd_buffer[else_loc] == 0x07)
                                    {
                                        // get else length
                                        short els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[else_loc + 2], scd_buffer[else_loc + 3] }, 0);

                                        // skip else
                                        x = else_loc + els_blk_len;
                                    }


                                }
                                else if (TEMP_CK.val == 0x01)
                                {
                                    short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                    // calculate else location
                                    int else_loc = x + ifblkLen - 4;
                                    x = else_loc;
                                }



                            }


                        }

                    }

                }

            }

        }


        public void SCENARIO_FILTER(byte[] scd_buffer, RDT_OBJ[] RDT_DATA, int rdt_index, int[] SCD_MAIN_OFF)
        {

            LIB_RDT.CK_DATA_OBJ TEMP_CK = new LIB_RDT.CK_DATA_OBJ();
            int CK_COUNT = 0;

            for (int x = 0; x < scd_buffer.Length; x++)
            {
                if (x != scd_buffer.Length)
                {
                    // if op ck
                    if (scd_buffer[x] == 0x21)
                    {
                        // if op ck confirm
                        if (scd_buffer[x + 3] == 0x01 || scd_buffer[x + 3] == 0x00)
                        {

                            TEMP_CK.op = scd_buffer[x];
                            TEMP_CK.bit_array = scd_buffer[x + 1];
                            TEMP_CK.num = scd_buffer[x + 2];
                            TEMP_CK.val = scd_buffer[x + 3];

                            CK_COUNT += 1;

                            RDT_DATA[rdt_index].List_CK_OFFS.Add(x + SCD_MAIN_OFF[rdt_index]);

                            if (TEMP_CK.bit_array == 0x01 && TEMP_CK.num == 0x01)
                            {

                                // A SCENARIO
                                if (TEMP_CK.val == 0x00)
                                {

                                    // calculate if len
                                    short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                    // calculate else location
                                    int else_loc = x + ifblkLen - 4;

                                    while (x != else_loc)
                                    {

                                        x++;


                                    }

                                    if (x == else_loc && scd_buffer[else_loc] == 0x07)
                                    {

                                        // get else length
                                        short els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[else_loc + 2], scd_buffer[else_loc + 3] }, 0);

                                        // skip else
                                        x = else_loc + els_blk_len;

                                    }


                                }


                            }


                        }

                    }

                }

            }


        }

    }
}







