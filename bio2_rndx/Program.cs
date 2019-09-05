using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic;



using System.Data;



namespace bio2_rndx
{
    public class Program
    {


        public LIB_RDT RDT_FUNCTIONS = new LIB_RDT();


        public LIB_RDT.RDT_OBJ[] RDT_DATA = new LIB_RDT.RDT_OBJ[0]; // resize to count of rdt files        
        public LIB_RDT.ITEM_DATA_OBJ[] ALL_ITEM_AOT = new LIB_RDT.ITEM_DATA_OBJ[0]; // resize to amount of items found in rdt file?

        public List<int> Item_Offsets = new List<int>();
        public List<int> EMD_OFFSETS = new List<int>();

        public LIB_RDT.MD1_MODEL_OBJ[] MD1_MODEL = new LIB_RDT.MD1_MODEL_OBJ[0];
        public LIB_RDT.CK_DATA_OBJ[] CK_TEST = new LIB_RDT.CK_DATA_OBJ[0];


        public static Program prg = new Program();
        public static LIB_RDT.RDT_HEADER_OBJ RDT_HEADER = new LIB_RDT.RDT_HEADER_OBJ();

        public static LIB_RDT.SEED_HEADER_OBJ SEED_HEADER = new LIB_RDT.SEED_HEADER_OBJ();
        public static LIB_RDT.SEED_ENTRY_OBJ[] SEED_ENTRIES = new LIB_RDT.SEED_ENTRY_OBJ[0]; // resize later
        public static LIB_RDT.SEED_ENT_OBJ[] SEED_ENTRYS = new LIB_RDT.SEED_ENT_OBJ[0];

        public string Dir_Path = string.Empty;
        public string Config_Path = string.Empty;
        public string[] RDT_FILES = new string[0];
        public int[] SCD_MAIN_OFFS = new int[0];
        public int[] SCD_SUB_OFFS = new int[0];
        public int[] SCD_EOF = new int[0];


        public List<LIB_RDT.ITEM_DATA_OBJ> AllItems = new List<LIB_RDT.ITEM_DATA_OBJ>();
        public List<LIB_RDT.ITEM_DATA_OBJ> Shuffled_Items = new List<LIB_RDT.ITEM_DATA_OBJ>();

        public List<LIB_RDT.ITEM_DATA_OBJ> LA_KEYS = new List<LIB_RDT.ITEM_DATA_OBJ>();
        public List<LIB_RDT.ITEM_DATA_OBJ> CA_KEYS = new List<LIB_RDT.ITEM_DATA_OBJ>();


        
        public List<int> GLOBAL_ITEM_OFFS = new List<int>();
        public List<int> RDT_ITEM_TOTAL = new List<int>();

       public List<int> ItemBoxRoomList = new List<int> { 15, 31,38,68,69,73,89,103,133};
        public List<string> LogOut = new List<string>();



        public int t_Items; // total items scanned
        public int t_Cmn; // total items found 00 - 2E
        public int t_cmn_rdt; // rdt relative common count
        public int t_Key; // total keys found
        public static int t_count;
        public static int t_em_count;


        /// <summary>
        ///  Simply for scanning array/bit vals
        /// </summary>
        public string Scan_Op;
        public int Scan_Bit;
        public int Scan_Num;
        public int Scan_Val;



        // GAME RELATED BYTE FLAGS
        // #################################################################
        public static byte SCE_FLAG_A = 0; // SCENARIO A FLAG
        public static byte SCE_FLAG_B = 1; // SCENARIO B FLAG
        public static byte PL_FLAG_L = 0;  // LEON FLAG
        public static byte PL_FLAG_C = 1; // CLAIRE FLAG
        public static byte GM_FLAG_LC = 0; // LEON/CLAIRE FLAG
        public static byte GM_FLAG_HT = 1; // HUNK/TOFU FLAG
        public static byte DFC_FLAG_E = 0; // DIFFICULTY EASY FLAG
        public static byte DFC_FLAG_N = 1; // DIFFICULTY NORMAL FLAG

        public static byte ITEM_MODE = 0; // 0 == COMMON SHUFFLE ONLY, 1 = COMMON/KEY , 2 = KEYS ONLY?

        public static byte GAME_MODE = 0;

        public static byte DEBUG_MODE = 0; // 0 OFF || 1 ON
        public static byte ENEMY_MODE = 0;
        public static byte PUZZLE_MODE = 0;
        public static byte CUTSCENE_MODE = 0;
        public static byte ITEMBOX_MODE = 0;
        public static byte SCENEARIO_MODE = 0; /// HUNK / A / B ENEMY LAYOUTS
        public static byte HANDGUN_MODE = 0; // disable /enable hg on loadout..


        public enum ItemClass
        {
              BLUECARD,
              SUIT_KEYS,
              OBJ_KEYS,
              SEWER_KEYS,
              LAB_KEYS
              
        }

        



        int spade_dxt = 0;
        int heart_dxt = 0;
        int dmnd_dxt = 0;  // discard totals

        byte LockFlag = 0x90;

        // #################################################################


        /// <summary>
        /// Array of Blue Key Card Locations, (Start > Main Hall ) + BUS
        /// </summary>
    //    public int[] BlueKeyList = new int[] { 1, 2, 3, 4, 5, 29 };

        public List<int> BlueKeyList = new List<int>() { 1, 2, 3, 4, 5, 29 };




        /// <summary>
        /// 2 Lists for object keys?
        /// </summary>
        // currently being used for RPD locations
        //public int[] ObjectKeyList = new int[] { 6, 7, 8, 9, 10, 11,
        //    12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 32, 33, 34,
        //    35, 39, 43, 44, 45, 46,47,48,49,50,51,52,53,54,55,
        //    56, 57, 58, 59, 60, 61,62,63,64,65,66,72,73,74,75,76, 77,78};

        public List<int> ObjectKeyList = new List<int>(){ 6, 7, 8, 9, 10, 11,
            12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 32, 33, 34,
            35, 39, 43, 44, 45, 46,47,48,49,50,51,52,53,54,55,
            56, 57, 58, 59, 60, 61,62,63,64,65,66,72,73,74,75,76, 77,78};


        // includes rpd locations
        // public int[] SewerKeyList = new int[] {88,89,90,91,92,93,94,95,96,97,98,99,100};



        //  public int[] SuitKeyList = new int[] { 6, 7, 8, 9, 13, 14, 15, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 32, 33, 41, 42, 43, 44, 45, 46, 47, 50, 51, 52, 53, 54, 55 };

        //   public List<int> SuitKeyList = new List<int>() {13,17,43,51};

        public List<int> FilmKeyList = new List<int>() {6, 7, 8, 9, 10,
            12, 13, 14, 15, 16, 17, 18, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 32, 34,
            35, 39, 43, 44, 45, 46,51,52,53,54,55,
            56, 57, 58, 59, 60, 61,62,63,64,65,66,75,76, 77,78};

         public List<int> SuitKeyList = new List<int>() { 6, 7, 8, 9, 13, 14, 15, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 32, 33, 41, 42, 43, 44, 45, 46, 47,48,49, 50, 53, 54, 55 };
        //public List<int> PlugKeyList = new List<int>() { 6, 7, 8, 9, 13, 14, 15, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 32, 33, 41, 42, 43, 44, 45, 46, 47, 50, 51, 52, 53, 54, 55 };

        public List<int> SewerKeyList = new List<int>() { 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99}; // sewer entrace, to annette fight

        public List<int> LabKeyList = new List<int>() { 101, 102, 104, 107, 108, 109, 110, 111, 112, 113, 114, 115, 120, 121, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134 };



        // locations of rpd puzzle index's
      //  public List<int> RPDPuzzleList = new List<int>() { 13, 51, 30, 56, 17, 11, 43, 81 };
        public List<int> RPDPuzzleList = new List<int>() { 13, 51, 17, 43};

        // includes sewer and rpd locations
        // public int[] LabKeyList = new int[] { 101,102,107,108,109,110,111,112,113,114,115,120,121,125,126,127,128,129,130,131,132,133,134};


        //public List<int> ObjectKeyList2 = new List<int>() { 6, 7, 8, 9, 10, 11,
        //    12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 32, 33, 34,
        //    35, 39, 43, 44, 45, 46,47,48,49,50,51,52,53,54,55,
        //    56, 57, 58, 59, 60, 61,62,63,64,65,66,72,73,74,75,76,77,78,79};




        /// <summary>
        /// Array of Suit Key Locations
        /// </summary>



        //public List<int> SuitKeyList = new List<int>() { 6, 7, 8, 9, 10, 11,
        //    12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 32, 33, 34,
        //    35, 39, 43};



        // *********************************************************************************************** //
        //    BIO2_RNDX DCHAPS 2018, 
        //    ARGS[0] = -l || -c (leon claire)
        //    ARGS[1] = -em_on || -em_off (enemy shuffle)
        //    args[2] = -gs_on || -gs_off (game type shuffle, A/B/Hunk/Tofu)
        //    args[3] = pz_on ||  -pz_off (puzzle scramlbing on off) (safe,torch,final timer, etc)
        // *********************************************************************************************** //

        public static void Main(string[] args)
        {
            Console.BufferHeight = 32000;

            prg.Banner();


            

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Config.INI"))
            {


                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CONFIG INI FOUND");


                // open config and read dir path
                using (StreamReader Sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Config.INI"))
                {
                    prg.Dir_Path = Sr.ReadLine();
                    Console.WriteLine(prg.Dir_Path);
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("NO CONFIG INI FOUND!");
            }



            if (args != null)
            {
                if (args.Length == 1)
                {



                    
                    // leon argument // common/key shuffle
                    if (args[0] == "/l" || args[0] == "/L")
                    {


                        DEBUG_MODE = CmdParse.PromptDebug(DEBUG_MODE);
                        ENEMY_MODE = CmdParse.PromptEnemy(ENEMY_MODE);
                        PUZZLE_MODE = CmdParse.PromptPuzzle(PUZZLE_MODE);
                        ITEMBOX_MODE = CmdParse.PromptItemBox(ITEMBOX_MODE);
                        SCENEARIO_MODE = CmdParse.PromptLayout(SCENEARIO_MODE);
                        CUTSCENE_MODE = CmdParse.PromptCut(CUTSCENE_MODE);
                        HANDGUN_MODE = CmdParse.PromptHG(HANDGUN_MODE);

                        // set item mode to common only
                        ITEM_MODE = 1;
                        PL_FLAG_L = 1;

                        //if backup dir doesent exist
                        if (!Directory.Exists(prg.Dir_Path + "\\bk_rdt_L"))
                        {
                            CopyDir.Copy(prg.Dir_Path + "\\pl0\\Rdt", prg.Dir_Path + "\\bk_rdt_L", DEBUG_MODE);
                        }
                        else // it does copy backup into game folder
                        {
                            CopyDir.Copy(prg.Dir_Path + "\\bk_rdt_L", prg.Dir_Path + "\\pl0\\Rdt", DEBUG_MODE);
                        }

                             
                        // leon file path..
                        string[] rdt_files = Directory.GetFiles(prg.Dir_Path + "\\pl0\\Rdt");
                        int filter_num = 0;
                        t_count = 0;
                        t_em_count = 0;


                        //resize all structures to num of rdts
                        Array.Resize(ref prg.SCD_MAIN_OFFS, rdt_files.Length);
                        Array.Resize(ref prg.SCD_SUB_OFFS, rdt_files.Length);
                        Array.Resize(ref prg.SCD_EOF, rdt_files.Length);
                        Array.Resize(ref prg.RDT_DATA, rdt_files.Length);
              
                        Array.Resize(ref SEED_ENTRIES, rdt_files.Length);
                        Array.Resize(ref SEED_ENTRYS, 158);

                        Console.ForegroundColor = ConsoleColor.Green;

                        if (DEBUG_MODE == 1)
                        {
                            Console.WriteLine("TOTAL RDT's FOUND: " + rdt_files.Length + "\n");
                        }
                        

                        for (int i = 0; i < rdt_files.Length; i++)
                        {

                            if (DEBUG_MODE == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.WriteLine("\n[" + i.ToString() + "] " + "[" + rdt_files[i].Substring(rdt_files[i].Length - 12, 12) + "]");
                                Console.WriteLine("-------------------------------------------------------------");
                            }

                            char Fcheck = char.Parse(rdt_files[i].Substring(rdt_files[i].Length - 8, 1));

                            //Filtered Out Extreme Battle
                            if (Char.IsNumber(Fcheck))
                            {
                                filter_num++;
                                BIO2_LIB_RND.Item_Patch(rdt_files[i], i);
                                prg.PARSE_RDT(rdt_files[i], i); // mass parse rdt's store offsets
                                prg.PARSE_SCD_BUFFER(rdt_files[i], i); // mass parse rdt item/data/get counts
                            }
                            //prg.PARSE_RDT_ITEMS(rdt_files);

                        }



                        //Console.WriteLine("Total Items Scanned: " + prg.t_Items);
                        //Console.WriteLine("Total Common Items Scanned: " + prg.t_Cmn);
                        //Console.WriteLine("Total Keys: " + prg.t_Key);

                       

                        // shuffle read item list before re writing..
                        prg.ITEM_SHUFFLE(args,prg.AllItems, prg.Shuffled_Items);

                        
                        Console.ForegroundColor = ConsoleColor.White;
                        
                        Console.WriteLine(Figgle.FiggleFonts.CyberMedium.Render("\n\n\n SEED LOG"));

                      
                        LIB_RDT.Prompt_Swap(prg.Shuffled_Items);

                        Console.WriteLine("\n");



                      

                        // loop all files
                        for (int x = 0; x < rdt_files.Length; x++)
                        {
                           
                            BIO2_LIB_RND.Shuffle_CK(rdt_files[x], x, prg.RDT_DATA[x].Item_Aot_Count, ref t_count, prg.RDT_DATA, prg.Shuffled_Items, SEED_ENTRYS, prg.LogOut);
                            


                            /// ENABLE PUZZLES
                            if (PUZZLE_MODE == 1)
                            {

                                if (x == 12) { BIO2_LIB_RND.Statue_Shuffle(rdt_files[x]); }
                                if (x == 41) { BIO2_LIB_RND.Safe_Shuffle(rdt_files[x], x); }
                                if (x == 43) { BIO2_LIB_RND.Torch_Shuffle(rdt_files[x]); }
                                if (x == 119 || x == 123 || x == 121) { BIO2_LIB_RND.Timer_Shuffle(rdt_files[x], x); }
                            }



                            // enable cutscene mode
                            if (CUTSCENE_MODE == 1)
                            {
                                BIO2_LIB_RND.CUTSCENE_EM_SWAP(rdt_files[x], x);
                            }


                        
                            // enable em_set randomization for hunk/tofu =]


                            if (SCENEARIO_MODE == 1)
                            {

                                BIO2_LIB_RND.Shuffle_GTYPE(rdt_files[x], x);

                            }


                            if (ENEMY_MODE == 1)
                            {

                                


                                switch (x)
                                {

                                    case 0:
                                    case 2:
                                    //  case 3: no brad room
                                    //   case 5:
                                    case 6:
                                    case 7:
                                    //   case 9:
                                    case 10:
                                    case 15:
                                    //  case 19: // blue coke hall (buggy as fuck)
                                    case 20: // stars hallway
                                    case 24: // dumpster area
                                    case 25: // b4 bus
                                    case 26:
                                    case 27:
                                    //   case 32: // marvin (buggy as fuck)
                                    case 37: // dark room hall
                                    case 39:
                                    case 40:
                                    case 45: // yellow hall
                                    case 41:
                                    case 42:
                                    case 46: // night duty
                                    case 47:
                                    case 52: // parking lot
                                    case 58: // spider sewer
                                             // case 54: // kennel
                                    case 61:
                                    //  case 75: // sewer 00
                                    //  case 76: // sewer 01
                                    case 88: // train zombie basement00
                                    case 94: // train zombie basement 01
                                    case 102: // naked turntable
                                    case 105: // green locker
                                    case 107: // lab red power room
                                              //    case 110: // plant chute
                                    case 112: // mo disk room
                                    case 117: // vam room
                                    case 120: // END A/B HALL ;) 



                                        BIO2_LIB_RND.EMD_SWAP(rdt_files[x], x, prg.RDT_DATA[x].EMD_COUNT, ref t_em_count, prg.RDT_DATA);

                                        break;


                                }

                                //   BIO2_LIB_RND.DOOR_AOT_SWAP(rdt_files[x], x); 

                                switch (x)
                                {
                                    //    case 0: // starting room
                                    //              case 1:  // kendo room,breaks..
                                    //    case 2: // basketball court
                                    case 3: // outside rpd (wont work tho if they grab shit..)
                                    case 5: // parking lot B
                                            //    case 6: // cabin
                                            //    case 7: // valve zombies area
                                            //    case 10: // helecopter hallway
                                            //    case 15: // 2f save room
                                            //                   case 16: // 2f hall/ladder...
                                            //               case 19: // blue coke hallawy (gtype)
                                            //    case 20: // stars hallway
                                            //    case 24: // dumpster area
                                            //    case 25: //b4 bus, can crash because of states
                                            //    case 26: // bus
                                            //    case 27: // racoon city 3?? (after bus)
                                            //    case 29: // trial room
                                            //    case 37: // 1f stairs hall
                                            //    case 39: // evidence locker
                                            //    case 40: // blue hall
                                            //    case 41: // east office
                                            //    case 42:  // red hall
                                            //    case 46: // night duty
                                            //               case 51: // autopsy
                                            //    case 53: // cell block (hunk setup only)
                                            //    case 61: // sewer?
                                            //    case 88: // basement cooridor 01
                                            //    case 94: // basement cooridor 02
                                            //    case 102: // lab white area
                                            //    case 112: // VAM ROOM?
                                            //    case 117: // Birkin lab
                                            //                case 118: // mo disk hallway// lickers


                                        BIO2_LIB_RND.EMD_SHUFFLE(rdt_files[x], x, prg.RDT_DATA[x].EMD_COUNT, ref t_em_count, prg.RDT_DATA);
                                        break;

                                }

                            }

                            //if (x == 21)
                            //{
                            BIO2_LIB_RND.BANDAID_FIX(rdt_files[x], x);
                            //}
                        }


                        if (ITEMBOX_MODE == 1)
                        {

                            Random boxRoll = new Random();

                            // shuffle box list..
                            BIO2_LIB_RND.KFY_Shuffle(prg.ItemBoxRoomList, boxRoll);

                            // generate random count
                            int boxcount = boxRoll.Next(0, prg.ItemBoxRoomList.Count);

                            Console.WriteLine("DISABLING ITEM BOXES: (" + boxcount + ")" + "\n");

                            for (int i = 0; i < prg.ItemBoxRoomList.Count; i++)
                            {
                                if (i <= boxcount)
                                {
                                    BIO2_LIB_RND.ITEMBOX_DISABLE(rdt_files[i], boxcount, prg.ItemBoxRoomList, i);
                                }
                             }

                            
                        }

                        SEED_HEADER.Item_Count = (Int16)t_count;

                        // patch that ada throw between rooms shit
                        for (int j = 0; j < rdt_files.Length; j++)
                        {
                            if (j == 67)
                            {
                                BIO2_LIB_RND.ADA_THROW_PATCH(rdt_files[j], 62, 63, prg.RDT_DATA);
                            }
                            
                        }


                        if (HANDGUN_MODE == 1)
                        {

                            using (FileStream fs = new FileStream(prg.Dir_Path + "\\bio2.exe", FileMode.Open))
                            {
                                using (BinaryWriter bw = new BinaryWriter(fs))
                                {

                                    Int16 nullz = 0x00;

                                    fs.Seek(1311072, SeekOrigin.Begin);
                                    bw.Write(nullz);


                                }
                            }

                        }


                        else
                        {
                            using (FileStream fs = new FileStream(prg.Dir_Path + "\\bio2.exe", FileMode.Open))
                            {
                                using (BinaryWriter bw = new BinaryWriter(fs))
                                {

                                    byte hg_id = 0x02;
                                    byte quan = 0x12;

                                    fs.Seek(1311072, SeekOrigin.Begin);
                                    bw.Write(hg_id);
                                    bw.Write(quan);


                                }
                            }





                        }





                        Console.WriteLine("Dump Seed Log to App Directory?? Y or N \n\n");

                        ConsoleKeyInfo WriteKey = Console.ReadKey();
                        if (WriteKey.Key == ConsoleKey.Y) {

                            CmdParse.SeedLog(prg.LogOut);
                        }
                        

                        
                       

                        //  var tbl = ConsoleTableBuilder.From(table);

                        //   tbl.WithFormat(ConsoleTableBuilderFormat.Alternative).ExportAndWriteLine();


                      //  LIB_RDT.Dump_Seed(SEED_ENTRYS, SEED_HEADER);

                        //   table.TableName = "BIO2";
                        //    table.Prefix = "SEED_LOG";

                        //  table.WriteXml(new FileStream(AppDomain.CurrentDomain.BaseDirectory + "SEED_LA_" + String.Format("{0:X}", "test".GetHashCode()), FileMode.Create));
                        //  table.WriteXmlSchema(new FileStream(AppDomain.CurrentDomain.BaseDirectory + "SEED_LA(" + String.Format("{0:X}", "test".GetHashCode() + ")"), FileMode.OpenOrCreate));


                      


                        prg.Banner();

                        Console.ReadKey();
                        return;
                        
                    }


                    if (args[0] == "/c" || args[0] == "/C") {


                        Console.WriteLine("Nice try but good things come to those that wait smart guy !! =]\n");

                        Environment.Exit(0);


                    }


                    if (args[0] == "-c" && args[1] == "-em_on" && args[2] == "-gs_on" && args[3] == "-pz_on")
                    {



                        Console.WriteLine("CLAIRE A (ENEMY SHUFFLE ON)  (GAME TYPE SHUFFLE ON) (PUZZLE SHUFFLE ON)");

                        // set item mode to common only
                        ITEM_MODE = 1;
                        PL_FLAG_L = 1;

                        //if backup dir doesent exist
                        if (!Directory.Exists(prg.Dir_Path + "\\bk_rdt_C"))
                        {
                            CopyDir.Copy(prg.Dir_Path + "\\pl1\\Rdt", prg.Dir_Path + "\\bk_rdt_C", DEBUG_MODE);
                        }
                        else // it does copy backup into game folder
                        {
                            CopyDir.Copy(prg.Dir_Path + "\\bk_rdt_C", prg.Dir_Path + "\\pl1\\Rdt", DEBUG_MODE);
                        }


                        // leon file path..
                        string[] rdt_files = Directory.GetFiles(prg.Dir_Path + "\\pl1\\Rdt");
                        int filter_num = 0;
                        t_count = 0;
                        t_em_count = 0;

                        List<string> rdt_filez = new List<string>();
                        //resize all structures to num of rdts
                        Array.Resize(ref prg.SCD_MAIN_OFFS, rdt_files.Length);
                        Array.Resize(ref prg.SCD_SUB_OFFS, rdt_files.Length);
                        Array.Resize(ref prg.SCD_EOF, rdt_files.Length);
                        Array.Resize(ref prg.RDT_DATA, rdt_files.Length);

                  

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("TOTAL RDT's FOUND: " + rdt_files.Length + "\n");



                        for (int i = 0; i < rdt_files.Length; i++)
                        {

                            int qChk = int.Parse(rdt_files[i].Substring(rdt_files[i].Length - 5, 1));

                           

                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.WriteLine("\n[" + i.ToString() + "] " + "[" + rdt_files[i].Substring(rdt_files[i].Length - 12, 12) + "]");
                                Console.WriteLine("-------------------------------------------------------------");


                                char Fcheck = char.Parse(rdt_files[i].Substring(rdt_files[i].Length - 8, 1));
                                int claireCk = int.Parse(rdt_files[i].Substring(rdt_files[i].Length - 5, 1));

                                //Filtered Out Extreme Battle
                                if (Char.IsNumber(Fcheck))
                                {
                                    filter_num++;
                                 //   BIO2_LIB_RND.Item_Patch(rdt_files[i], i);
                                    prg.PARSE_RDT(rdt_files[i], i); // mass parse rdt's store offsets
                                    prg.PARSE_SCD_BUFFER(rdt_files[i], i); // mass parse rdt item/data/get counts
                                }
                                //prg.PARSE_RDT_ITEMS(rdt_files);
                            
                        }



                        //Console.WriteLine("Total Items Scanned: " + prg.t_Items);
                        //Console.WriteLine("Total Common Items Scanned: " + prg.t_Cmn);
                        //Console.WriteLine("Total Keys: " + prg.t_Key);



                        // shuffle read item list before re writing..
                        prg.ITEM_SHUFFLE(args, prg.AllItems, prg.Shuffled_Items);




                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine(Figgle.FiggleFonts.CyberMedium.Render("\n\n\n SEED LOG"));




                      //  LIB_RDT.Prompt_Swap(prg.Shuffled_Items);


                        // loop all files
                        for (int x = 0; x < rdt_files.Length; x++)
                        {
                            //Console.WriteLine(rdt_files[x]);

                            BIO2_LIB_RND.Shuffle_CK_Claire(rdt_files[x], x, prg.RDT_DATA[x].Item_Aot_Count, ref t_count, prg.RDT_DATA, prg.Shuffled_Items, SEED_ENTRYS);


                         //   if (x == 12) { BIO2_LIB_RND.Statue_Shuffle(rdt_files[x]); }
                        //    if (x == 41) { BIO2_LIB_RND.Safe_Shuffle(rdt_files[x], x); }
                        //    if (x == 43) { BIO2_LIB_RND.Torch_Shuffle(rdt_files[x]); }
                       //     if (x == 119 || x == 123 || x == 121) { BIO2_LIB_RND.Timer_Shuffle(rdt_files[x], x); }

                            //  BIO2_LIB_RND.CUTSCENE_EM_SWAP(rdt_files[x], x);



                            // enable em_set randomization for hunk/tofu =]
                       //     BIO2_LIB_RND.Shuffle_GTYPE(rdt_files[x], x);

                            switch (x)
                            {

                                case 0:
                                case 2:
                                //  case 3: no brad room
                                //   case 5:
                                case 6:
                                case 7:
                                //   case 9:
                                case 10:
                                case 15:
                                case 19: // blue coke hall
                                case 20: // stars hallway
                                case 24: // dumpster area
                                case 25: // b4 bus
                                case 26:
                                case 27:
                                case 32: // marvin
                                case 37: // dark room hall
                                case 39:
                                case 40:
                                case 45: // yellow hall
                                case 41:
                                case 42:
                                case 46: // night duty
                                case 47:
                                case 52: // parking lot
                                case 58: // spider sewer
                                         // case 54: // kennel
                                case 61:
                                //  case 75: // sewer 00
                                //  case 76: // sewer 01
                                case 88: // train zombie basement00
                                case 94: // train zombie basement 01
                                case 102: // naked turntable
                                case 105: // green locker
                                case 107: // lab red power room
                                          //    case 110: // plant chute
                                case 112: // mo disk room
                                case 117: // vam room
                                case 120: // END A/B HALL ;) 



                                    BIO2_LIB_RND.EMD_SWAP(rdt_files[x], x, prg.RDT_DATA[x].EMD_COUNT, ref t_em_count, prg.RDT_DATA);

                                    break;


                            }

                            //   BIO2_LIB_RND.DOOR_AOT_SWAP(rdt_files[x], x); 

                            switch (x)
                            {
                                //    case 0: // starting room
                                //              case 1:  // kendo room,breaks..
                                //    case 2: // basketball court
                                case 3: // outside rpd (wont work tho if they grab shit..)
                                case 5: // parking lot B
                                        //    case 6: // cabin
                                        //    case 7: // valve zombies area
                                        //    case 10: // helecopter hallway
                                        //    case 15: // 2f save room
                                        //                   case 16: // 2f hall/ladder...
                                        //               case 19: // blue coke hallawy (gtype)
                                        //    case 20: // stars hallway
                                        //    case 24: // dumpster area
                                        //    case 25: //b4 bus, can crash because of states
                                        //    case 26: // bus
                                        //    case 27: // racoon city 3?? (after bus)
                                        //    case 29: // trial room
                                        //    case 37: // 1f stairs hall
                                        //    case 39: // evidence locker
                                        //    case 40: // blue hall
                                        //    case 41: // east office
                                        //    case 42:  // red hall
                                        //    case 46: // night duty
                                        //               case 51: // autopsy
                                        //    case 53: // cell block (hunk setup only)
                                        //    case 61: // sewer?
                                        //    case 88: // basement cooridor 01
                                        //    case 94: // basement cooridor 02
                                        //    case 102: // lab white area
                                        //    case 112: // VAM ROOM?
                                        //    case 117: // Birkin lab
                                        //                case 118: // mo disk hallway// lickers


                                    BIO2_LIB_RND.EMD_SHUFFLE(rdt_files[x], x, prg.RDT_DATA[x].EMD_COUNT, ref t_em_count, prg.RDT_DATA);
                                    break;

                            }

                            //if (x == 21)
                            //{
                            BIO2_LIB_RND.BANDAID_FIX(rdt_files[x], x);
                            //}
                        }







                       
                          


                     //   SEED_HEADER.Item_Count = (Int16)t_count;

                        // patch that ada throw between rooms shit
                        //for (int j = 0; j < rdt_files.Length; j++)
                        //{
                        //    if (j == 67)
                        //    {
                        //        BIO2_LIB_RND.ADA_THROW_PATCH(rdt_files[j], 62, 63, prg.RDT_DATA);
                        //    }

                        //}


                        //  var tbl = ConsoleTableBuilder.From(table);

                        //   tbl.WithFormat(ConsoleTableBuilderFormat.Alternative).ExportAndWriteLine();


                        //  LIB_RDT.Dump_Seed(SEED_ENTRYS, SEED_HEADER);

                        //   table.TableName = "BIO2";
                        //    table.Prefix = "SEED_LOG";

                        //  table.WriteXml(new FileStream(AppDomain.CurrentDomain.BaseDirectory + "SEED_LA_" + String.Format("{0:X}", "test".GetHashCode()), FileMode.Create));
                        //  table.WriteXmlSchema(new FileStream(AppDomain.CurrentDomain.BaseDirectory + "SEED_LA(" + String.Format("{0:X}", "test".GetHashCode() + ")"), FileMode.OpenOrCreate));





                        prg.Banner();




                        Console.ReadKey();
                        return;

                    }


                  


                    // repair dirs
                    if (args[0] == "-repair")
                    {


                        if (!Directory.Exists(prg.Dir_Path + "\\bk_rdt_L"))
                        {

                            CopyDir.Copy(prg.Dir_Path + "\\pl0\\Rdt", prg.Dir_Path + "\\bk_rdt", DEBUG_MODE);
                        }
                        else // it does copy backup into game folder
                        {
                            CopyDir.Copy(prg.Dir_Path + "\\bk_rdt_l", prg.Dir_Path + "\\pl0\\Rdt", DEBUG_MODE);
                        }



                    }

                    if (args[0] == "-opscan") {

                        string[] rdt_files = Directory.GetFiles(prg.Dir_Path + "\\pl0\\Rdt");

                        
                        Console.WriteLine("Pick Scan op... CK || SET \n");
                        prg.Scan_Op = Console.ReadLine();

                        Console.WriteLine("Bit Array: 0x04, 0x05\n");
                        prg.Scan_Bit = Console.Read();

                        Console.WriteLine("Bit Num: 0x9C etc\n");
                        prg.Scan_Num = Console.Read();

                        Console.WriteLine("Bit Val: 0x00 0x01 (true\false)\n");
                        prg.Scan_Val = Console.Read();


                        if (prg.Scan_Op == "CK") {

                            for (int i = 0; i < rdt_files.Length; i++)
                            {
                                prg.ARRAY_SCAN(0x21, prg.Scan_Bit, prg.Scan_Num, prg.Scan_Val, rdt_files[i], i);
                            }
                        }
                        if (prg.Scan_Op == "SET") {

                            for (int i = 0; i < rdt_files.Length; i++)
                            {
                                prg.ARRAY_SCAN(0x22, prg.Scan_Bit, prg.Scan_Num, prg.Scan_Val, rdt_files[i], i);
                            }
                        }


 
                    }

                    //if (args[0] == "-opscan" && args[1] == "c" && args[2] == prg.Scan_Op.ToString() && args[3] == prg.Scan_Bit.ToString() && args[4] == prg.Scan_Num.ToString() && args[5] == prg.Scan_Val.ToString())
                    //{
                    //    string[] rdt_files = Directory.GetFiles(prg.Dir_Path + "\\pl1\\Rdt");

                    //    for (int i = 0; i < rdt_files.Length; i++)
                    //    {
                    //        prg.ARRAY_SCAN(prg.Scan_Op, prg.Scan_Bit, prg.Scan_Num, prg.Scan_Val, rdt_files[i], i);
                    //    }

                    //}

                
                   // Console.Read();

                }
                else
                {
                    prg.Help();
                   
                }

            }
            else
            {

                prg.Help();

            }



        }



        // resonisble for shuffling the filtered list of item data
        /// <summary>
        /// Shuffle List of Items
        /// </summary>
        /// <param name="AllItems"></param>
        /// <param name="Return_Shuffle"></param>
        /// <param name="MODE_FLG"></param>
        public void ITEM_SHUFFLE(string[] args, List<LIB_RDT.ITEM_DATA_OBJ> AllItems, List<LIB_RDT.ITEM_DATA_OBJ> Return_Shuffle)
        {


            Random r_item = new Random();
            Random r_index = new Random();
            
            int indexer = 0;
            int n = 0;

            int BlueKeycard_Idx = 0; // orig blue keycard idx (artifically set in ada box room)

           // these lists are for holding the original key position index's

          //  List<int> KeyIdx = new List<int>(); // Holds idx of original 5 keys
            List<int> KeySuit_Idx = new List<int>(); // holds idx of suited keys.. (
         //   List<int> Plug_Idx = new List<int>();

            List<int> KeyObjectIdx = new List<int>(); // holds idx of object based keys (crank,jewls,etcetc)
            List<int> RPD_Keys_Idx = new List<int>();
            List<int> Sewer_Keys_Idx = new List<int>();
            List<int> Lab_Keys_Idx = new List<int>();
            List<int> Film_Key_Idx = new List<int>();



            bool ClubKey = false;

            foreach (LIB_RDT.ITEM_DATA_OBJ ITEM in AllItems)
            {

                
                if (LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(ITEM.item))
                {
                    // if LEON COOMON/KEY MIX

                    if (args[0] == "/L" || args[0] == "/L")
                    {
                        // if LEON KEY oR LEON ITEM... FILTER IS STILL NESSECARILY REGARDLESS OF USING LEON COMMON LISTS BECAUSE HG BULLETS ARE SHARED BETWEEN LEON\CLAIRE!!!

                        // somehow includes notes in the shuffle?
                        //if (LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(ITEM.item))
                        //{
                        //    Return_Shuffle.Add(ITEM);
                        //}

                        if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(ITEM.item) || LIB_ITEM.BIO2_COMMON_LUT_LA.ContainsKey(ITEM.item))
                        {
                            Return_Shuffle.Add(ITEM);
                        }

                    }

                    if (args[0] == "/C" || args[0] == "/c")
                    {
                        // if LEON KEY oR LEON ITEM... FILTER IS STILL NESSECARILY REGARDLESS OF USING LEON COMMON LISTS BECAUSE HG BULLETS ARE SHARED BETWEEN LEON\CLAIRE!!!

                        // somehow includes notes in the shuffle?
                        //if (LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(ITEM.item))
                        //{
                        //    Return_Shuffle.Add(ITEM);
                        //}

                        if (LIB_ITEM.BIO2_COMMON_LUT_CA.ContainsKey(ITEM.item) || LIB_ITEM.BIO2_KEY_LUT_CA.ContainsKey(ITEM.item))
                        {
                            Return_Shuffle.Add(ITEM);
                        }

                    }

                }

             
                //if (LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(ITEM.item))
                //{
                   // Console.WriteLine("Item " + LIB_ITEM.BIO2_ITEM_LUT[ITEM.item]);
                //}

            
            }

            

                BIO2_LIB_RND.KFY_Shuffle(Return_Shuffle, r_item);


            // for every item in the shuffled list..
            foreach (LIB_RDT.ITEM_DATA_OBJ ITEM in Return_Shuffle)
            {
                indexer += 1; // craete an indexer...

                

                // some color coding debug garbage..
                if (ITEM.item >= 0x01 && ITEM.item <= 0x13)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(ITEM.item))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                

                       // if we dealing with keys on leon A
             

                    
                    ///////////// COLLECT INDEX's OF CERTAIN KEY ITEMS 

                    // if leon A
                    if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(ITEM.item))
                    {
                        // Random n_index = new Random();

                        // test with the reg keys...
                        // if(LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(ITEM.item))

                         //      case 0x44:
                         //case 0x45:
                         //case 0x46:
                        // IF SUIT KEYS FOUND COLLECT INDEXES
                        if (ITEM.item == 0x59 || ITEM.item == 0x5A || ITEM.item == 0x5B || ITEM.item == 0x5C)
                        {
                            KeySuit_Idx.Add(indexer - 1);
                          //  Console.WriteLine("] " + indexer);
                        }

                        if(ITEM.item == 0x44 || ITEM.item == 0x45 || ITEM.item == 0x46 || ITEM.item == 0x50)
                        {
                            Film_Key_Idx.Add(indexer - 1);
                        }

                        if (ITEM.item == 0x35)
                        {
                            BlueKeycard_Idx = indexer - 1;

                          //  Console.WriteLine(indexer + "!!");
                        }
                      

                        // COLLCET INDEX's of every flagged item
                        LIB_RDT.IndexBuilder(RPD_Keys_Idx, ITEM.item, indexer, 0);
                        LIB_RDT.IndexBuilder(Sewer_Keys_Idx, ITEM.item, indexer, 1);
                        LIB_RDT.IndexBuilder(Lab_Keys_Idx, ITEM.item, indexer, 2);
                      
                        
                    }






                
                //    
              //  Console.WriteLine(LIB_ITEM.BIO2_ITEM_LUT[ITEM.item] + "\\ " + ITEM.amount);





            }
            

            // if LEON/COMMON/KEY SHUFFLE
           
                BIO2_LIB_RND.KFY_Shuffle(prg.BlueKeyList, r_index); // randomly shuffle blue keylist indexs
               // BIO2_LIB_RND.KFY_Shuffle(prg.ObjectKeyList, r_index);  // randomly shuffle object key indexs
               // BIO2_LIB_RND.KFY_Shuffle(prg.SuitKeyList, r_index); // randomly shuffle suited key indexes
                

                

                //Console.WriteLine("BLUE KEY CARD LOCATION INDICES");
                //for (int i = 0; i < BlueKeyList.Count; i++)
                //{
                //    Console.Write(BlueKeyList[i] + ",");
                //}



                for (int i = 0; i < BlueKeyList.Count; i++)
                {
                    if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[i].item) || LIB_ITEM.BIO2_KEY_LUT_CA.ContainsKey(Return_Shuffle[i].item))
                    {
                       // Console.WriteLine("removing[" + i + "] - " + LIB_ITEM.BIO2_KEY_LUT_LA[Return_Shuffle[i].item]);
                        BlueKeyList.Remove(i);
                    }
                }


                // for all blue keycard possible index locations
                for (int i = 0; i < BlueKeyList.Count; i++)
                {
                    // set roll index to non key item to avoid out of range shuffle
                    //if (!LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[BlueKeyList[i]].item))
                    //{
                        LIB_RDT.ITEM_DATA_OBJ BLUECARD_SWAP = Return_Shuffle[BlueKeyList[i]];
                        Return_Shuffle[BlueKeyList[i]] = Return_Shuffle[BlueKeycard_Idx];
                        Return_Shuffle[BlueKeycard_Idx] = BLUECARD_SWAP;
                       // break;
                   // }

                  
                }



              
                // remove rolled blue key index from suit key index's
                SuitKeyList.Remove(BlueKeyList[0]);
                FilmKeyList.Remove(BlueKeyList[0]);

                LIB_RDT.ListTransfer(BlueKeyList, FilmKeyList, 1, LIB_RDT.ItemClass.FILM_KEYS);
                LIB_RDT.ListTransfer(BlueKeyList, SuitKeyList, 1, LIB_RDT.ItemClass.SUIT_KEYS);
                LIB_RDT.ListTransfer(BlueKeyList, ObjectKeyList, 1, LIB_RDT.ItemClass.OBJ_KEYS);
                LIB_RDT.ListTransfer(BlueKeyList, SewerKeyList, 1, LIB_RDT.ItemClass.SEWER_KEYS);
                LIB_RDT.ListTransfer(BlueKeyList, LabKeyList, 1, LIB_RDT.ItemClass.LAB_KEYS);

                // LIB_RDT.ListTransfer(BlueKeyList, ObjectKeyList, 1);

                Console.ForegroundColor = ConsoleColor.Blue;



            if (args[0] == "-l")
            {
                if (DEBUG_MODE == 1)
                {

                    Console.WriteLine("\n [" + BlueKeyList[0] + "] " + LIB_ITEM.BIO2_KEY_LUT_LA[Return_Shuffle[BlueKeyList[0]].item] + " > " + " [" + BlueKeycard_Idx + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[BlueKeycard_Idx].item] + "\n");
                }
            }
            if (args[0] == "-c")
            {
                //Console.WriteLine("\n [" + BlueKeyList[0] + "] " + LIB_ITEM.BIO2_KEY_LUT_CA[Return_Shuffle[BlueKeyList[0]].item] + " > " + " [" + BlueKeycard_Idx + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[BlueKeycard_Idx].item] + "\n");
            }



            // shuffle landing points
            BIO2_LIB_RND.KFY_Shuffle(ObjectKeyList, r_item);
                BIO2_LIB_RND.KFY_Shuffle(SewerKeyList, r_item);
                BIO2_LIB_RND.KFY_Shuffle(LabKeyList, r_item);
                BIO2_LIB_RND.KFY_Shuffle(SuitKeyList, r_item);
                BIO2_LIB_RND.KFY_Shuffle(FilmKeyList, r_item);



            





                Console.ForegroundColor = ConsoleColor.Yellow;
                // Console.WriteLine("\n");
                //  Console.WriteLine("SUIT KEYS");

                //for (int i = 0; i < SuitKeyList.Count; i++)
                //{
                //    Console.Write(SuitKeyList[i] + ",");
                //}
                //Console.WriteLine("\n");


                //for (int i = 0; i < SuitKeyList.Count; i++)
                //{
                //    Console.WriteLine(i + "]" + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[i].item]);
                //}


                // transfer non used index's between lists

                //LIB_RDT.ListTransfer(FilmKeyList, SuitKeyList, 4, LIB_RDT.ItemClass.FILM_KEYS);
                //LIB_RDT.ListTransfer(FilmKeyList, SewerKeyList, 4, LIB_RDT.ItemClass.SEWER_KEYS);
                //LIB_RDT.ListTransfer(FilmKeyList, LabKeyList, 4, LIB_RDT.ItemClass.LAB_KEYS);


                // add remaining suit key indexs to other lists
                LIB_RDT.ListTransfer(SuitKeyList, ObjectKeyList, 4, LIB_RDT.ItemClass.OBJ_KEYS);


                LIB_RDT.ListTransfer(SuitKeyList, SewerKeyList, 4, LIB_RDT.ItemClass.SEWER_KEYS);
                LIB_RDT.ListTransfer(SuitKeyList, LabKeyList, 4, LIB_RDT.ItemClass.LAB_KEYS);



                //LIB_RDT.ListTransfer(ObjectKeyList, SewerKeyList, 13, LIB_RDT.ItemClass.SEWER_KEYS);
                //LIB_RDT.ListTransfer(ObjectKeyList, LabKeyList, 12, LIB_RDT.ItemClass.LAB_KEYS);



           //     LIB_RDT.ListTransfer(SewerKeyList, LabKeyList, 4, LIB_RDT.ItemClass.LAB_KEYS);

                // clean of keys b4 shuffle

               LIB_RDT.ListCleaner(FilmKeyList, Return_Shuffle);

                for(int i = 0; i < Film_Key_Idx.Count; i++)
                {
                    LIB_RDT.ITEM_DATA_OBJ OLD_ITEM = Return_Shuffle[FilmKeyList[i]];

                    Return_Shuffle[FilmKeyList[i]] = Return_Shuffle[Film_Key_Idx[i]];

                    Return_Shuffle[Film_Key_Idx[i]] = OLD_ITEM;

                if (DEBUG_MODE == 1)
                {
                    Console.WriteLine(Film_Key_Idx[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[FilmKeyList[i]].item] + " > " + FilmKeyList[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[Film_Key_Idx[i]].item]);
                }
                    SuitKeyList.Remove(SuitKeyList[i]);

                }



                LIB_RDT.ListCleaner(SuitKeyList, Return_Shuffle);



                Console.ForegroundColor = ConsoleColor.Green;

                for (int i = 0; i < KeySuit_Idx.Count; i++)
                {
                

                // if one of the first 4 suit key rolls is a key, reset it to the first non key index in the list > 1?
              


                if (Return_Shuffle[SuitKeyList[i]].item != 0x59 || Return_Shuffle[SuitKeyList[i]].item != 0x5A || Return_Shuffle[SuitKeyList[i]].item != 0x5B || Return_Shuffle[SuitKeyList[i]].item != 0x5C)
                {
                    LIB_RDT.ITEM_DATA_OBJ OLD_ITEM = Return_Shuffle[SuitKeyList[i]];

                     // new rolled spot becomes key
                        Return_Shuffle[SuitKeyList[i]] = Return_Shuffle[KeySuit_Idx[i]];

                        Return_Shuffle[KeySuit_Idx[i]] = OLD_ITEM;
                    //  }

                    if (DEBUG_MODE == 1)
                    {
                        Console.WriteLine(KeySuit_Idx[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[SuitKeyList[i]].item] + " > " + SuitKeyList[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[KeySuit_Idx[i]].item]);
                    }
                    // remove suitkey results from object key list
                    ObjectKeyList.Remove(SuitKeyList[i]);
                    }

                }


                // add remainder
              

                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.Red;
              


                LIB_RDT.ListCleaner(ObjectKeyList, Return_Shuffle);


                // for every newly rolled key object index
                for (int i = 0; i < RPD_Keys_Idx.Count; i++)
                {


                // first index less then 8 but greater then 1
                //   int First = ObjectKeyList.First(x => x < 80 && x > 1);

                // record original item at each keyobject index

              

                LIB_RDT.ITEM_DATA_OBJ OLD_ITEM = Return_Shuffle[ObjectKeyList[i]];
      

                    // place key object in rolled index
                    Return_Shuffle[ObjectKeyList[i]] = Return_Shuffle[RPD_Keys_Idx[i]];
                    // set original key idex to recorded old item
                    Return_Shuffle[RPD_Keys_Idx[i]] = OLD_ITEM;
                if (DEBUG_MODE == 1)
                {
                    Console.WriteLine(RPD_Keys_Idx[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[ObjectKeyList[i]].item] + " > " + ObjectKeyList[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[RPD_Keys_Idx[i]].item]);
                }
                    SewerKeyList.Remove(ObjectKeyList[i]);
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;

            
                //for (int i = 0; i < SewerKeyList.Count; i++)
                //{
                //    Console.WriteLine(i + "]" + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[i].item]);
                //}

                LIB_RDT.ListCleaner(SewerKeyList, Return_Shuffle);


            for (int i = 0; i < Sewer_Keys_Idx.Count; i++)
            {
                // if one of the rolled index's has a key
              


                if (!LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[SewerKeyList[i]].item))
                {
                    LIB_RDT.ITEM_DATA_OBJ OLD_ITEM = Return_Shuffle[SewerKeyList[i]];

                    Return_Shuffle[SewerKeyList[i]] = Return_Shuffle[Sewer_Keys_Idx[i]];

                    Return_Shuffle[Sewer_Keys_Idx[i]] = OLD_ITEM;
                    if (DEBUG_MODE == 1)
                    {
                        Console.WriteLine(Sewer_Keys_Idx[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[SewerKeyList[i]].item] + " > " + SewerKeyList[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[Sewer_Keys_Idx[i]].item]);

                    }
                    LabKeyList.Remove(SewerKeyList[i]);
                }
                
                }

                
            

                Console.ForegroundColor = ConsoleColor.Gray;
            
                LIB_RDT.ListCleaner(LabKeyList, Return_Shuffle);


                for (int i = 0; i < Lab_Keys_Idx.Count; i++)
                {

                //if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[LabKeyList[i]].item))
                //{
                //    // pick a new spot > 1 thats not a fucking key
                //    LabKeyList[i] = LabKeyList.First(x => x > 0 && !LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(Return_Shuffle[LabKeyList[x]].item));
                //}


                    LIB_RDT.ITEM_DATA_OBJ OLD_ITEM = Return_Shuffle[LabKeyList[i]];
                    Return_Shuffle[LabKeyList[i]] = Return_Shuffle[Lab_Keys_Idx[i]];

                    Return_Shuffle[Lab_Keys_Idx[i]] = OLD_ITEM;
                if (DEBUG_MODE == 1)
                {
                    Console.WriteLine(Lab_Keys_Idx[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[LabKeyList[i]].item] + " > " + LabKeyList[i] + "] " + LIB_ITEM.BIO2_ITEM_LUT[Return_Shuffle[Lab_Keys_Idx[i]].item]);
                }

                }

                var total_p_count = 0;
                var total_nKey_count = 0;
                Console.WriteLine("\n");

                Console.WriteLine("TOTAL COUNT: " + Return_Shuffle.Count);
            

            LIB_RDT.Integrity_Check(Return_Shuffle, DEBUG_MODE);

           

        }



        /// <summary>
        /// parse single rdt header, store offsets..
        /// </summary>
        /// <param name="file"></param>
        /// <param name="index"></param>
        private void PARSE_RDT(string file, int index)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            {

                fs.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(fs);


                RDT_HEADER.nSPrite = br.ReadByte();
                RDT_HEADER.nCut = br.ReadByte();
                RDT_HEADER.noModel = br.ReadByte();
                RDT_HEADER.nItem = br.ReadByte();
                RDT_HEADER.nDoor = br.ReadByte();
                RDT_HEADER.nRoom_At = br.ReadByte();
                RDT_HEADER.Reverb_lv = br.ReadByte();
                RDT_HEADER.nSprite_Max = br.ReadByte();



                Array.Resize(ref RDT_HEADER.nOffsets, 22); // resize all possible offsets to 23 max
                Array.Resize(ref RDT_DATA[index].Header_Offsets, 22);

                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < RDT_HEADER.nOffsets.Length; i++)
                {
                    RDT_HEADER.nOffsets[i] = br.ReadInt32();
                    RDT_DATA[index].Header_Offsets[i] = RDT_HEADER.nOffsets[i];
                    LIB_RDT.OFFSET_LIST.Add(RDT_HEADER.nOffsets[i]);
                    //     Console.WriteLine("[" + i.ToString() + "]" + LIB_RDT.LUT_RDT_OFFSET[i] + ": " + RDT_HEADER.nOffsets[i]);
                }


                Console.ForegroundColor = ConsoleColor.Yellow;
                SCD_MAIN_OFFS[index] = RDT_DATA[index].Header_Offsets[16];
                //  SCD_MAIN_OFFS[index] = RDT_HEADER.nOffsets[16];
                SCD_SUB_OFFS[index] = RDT_HEADER.nOffsets[17];
                SCD_EOF[index] = RDT_HEADER.nOffsets[18];




                LIB_RDT.OFFSET_LIST.Sort();


                fs.Close();
                br.Close();

            }

        }

        

        // get op cmd len prob wont even need
        private int SET_SCD2_CMD_LEN(byte xBYTE)
        {

            string xFF = xBYTE.ToString("X").PadLeft(2, '0');   //CONVERT TO HEX BYTE, USE PADDING!

            int cmd_len = 0;

            if (xFF == "00") { cmd_len = 1; }
            if (xFF == "01") { cmd_len = 2; }
            if (xFF == "02") { cmd_len = 2; }   //MIGHT BE 0x0200 INSTEAD OF 0x02 REALITY, MOST OF THE TIME FOLLOWED BY A 0x00 NOP...
            if (xFF == "03") { cmd_len = 4; }
            if (xFF == "04") { cmd_len = 2; }   //WAS 4 BEFORE...
            if (xFF == "05") { cmd_len = 2; }
            if (xFF == "06") { cmd_len = 4; }
            if (xFF == "07") { cmd_len = 4; }
            if (xFF == "08") { cmd_len = 2; }
            if (xFF == "09") { cmd_len = 4; }
            if (xFF == "0A") { cmd_len = 3; }
            if (xFF == "0B") { cmd_len = 1; }
            if (xFF == "0C") { cmd_len = 1; }
            if (xFF == "0D") { cmd_len = 6; }
            if (xFF == "0E") { cmd_len = 2; }
            if (xFF == "0F") { cmd_len = 4; }

            if (xFF == "10") { cmd_len = 2; }
            if (xFF == "11") { cmd_len = 4; }
            if (xFF == "12") { cmd_len = 2; }
            if (xFF == "13") { cmd_len = 4; }
            if (xFF == "14") { cmd_len = 6; }
            if (xFF == "15") { cmd_len = 2; }
            if (xFF == "16") { cmd_len = 2; }
            if (xFF == "17") { cmd_len = 6; }
            if (xFF == "18") { cmd_len = 2; }
            if (xFF == "19") { cmd_len = 2; }
            if (xFF == "1A") { cmd_len = 2; }
            if (xFF == "1B") { cmd_len = 6; }
            if (xFF == "1C") { cmd_len = 1; }
            if (xFF == "1D") { cmd_len = 4; }
            if (xFF == "1E") { cmd_len = 1; }
            if (xFF == "1F") { cmd_len = 1; }

            if (xFF == "20") { cmd_len = 1; }
            if (xFF == "21") { cmd_len = 4; }
            if (xFF == "22") { cmd_len = 4; }
            if (xFF == "23") { cmd_len = 6; }
            if (xFF == "24") { cmd_len = 4; }
            if (xFF == "25") { cmd_len = 4; }   //WAS 3 BEFORE...
            if (xFF == "26") { cmd_len = 6; }
            if (xFF == "27") { cmd_len = 4; }
            if (xFF == "28") { cmd_len = 2; }
            if (xFF == "29") { cmd_len = 2; }
            if (xFF == "2A") { cmd_len = 1; }
            if (xFF == "2B") { cmd_len = 6; }
            if (xFF == "2C") { cmd_len = 20; }
            if (xFF == "2D") { cmd_len = 38; }
            if (xFF == "2E") { cmd_len = 4; }	//SET BACK TO 4 AGAIN!! NOT SURE IF 3 OR 4 :S
            if (xFF == "2F") { cmd_len = 4; }

            if (xFF == "30") { cmd_len = 1; }
            if (xFF == "31") { cmd_len = 1; }
            if (xFF == "32") { cmd_len = 8; }
            if (xFF == "33") { cmd_len = 8; }
            if (xFF == "34") { cmd_len = 4; }
            if (xFF == "35") { cmd_len = 4; }   //WAS 3 BEFORE...
            if (xFF == "36") { cmd_len = 12; }
            if (xFF == "37") { cmd_len = 4; }
            if (xFF == "38") { cmd_len = 3; }
            if (xFF == "39") { cmd_len = 8; }
            if (xFF == "3A") { cmd_len = 16; }
            if (xFF == "3B") { cmd_len = 32; }
            if (xFF == "3C") { cmd_len = 2; }
            if (xFF == "3D") { cmd_len = 4; }	//MOST PROBABLY 4...    //WAS 3 BEFORE...
            if (xFF == "3E") { cmd_len = 6; }
            if (xFF == "3F") { cmd_len = 4; }

            if (xFF == "40") { cmd_len = 8; }
            if (xFF == "41") { cmd_len = 10; }
            if (xFF == "42") { cmd_len = 1; }
            if (xFF == "43") { cmd_len = 4; }
            if (xFF == "44") { cmd_len = 22; }
            if (xFF == "45") { cmd_len = 5; }
            if (xFF == "46") { cmd_len = 10; }
            if (xFF == "47") { cmd_len = 2; }
            if (xFF == "48") { cmd_len = 16; }
            if (xFF == "49") { cmd_len = 8; }
            if (xFF == "4A") { cmd_len = 2; }
            if (xFF == "4B") { cmd_len = 3; }
            if (xFF == "4C") { cmd_len = 5; }
            if (xFF == "4D") { cmd_len = 22; }
            if (xFF == "4E") { cmd_len = 22; }
            if (xFF == "4F") { cmd_len = 4; }

            if (xFF == "50") { cmd_len = 4; }
            if (xFF == "51") { cmd_len = 6; }
            if (xFF == "52") { cmd_len = 6; }
            if (xFF == "53") { cmd_len = 6; }
            if (xFF == "54") { cmd_len = 22; }
            if (xFF == "55") { cmd_len = 6; }
            if (xFF == "56") { cmd_len = 4; }
            if (xFF == "57") { cmd_len = 8; }
            if (xFF == "58") { cmd_len = 4; }
            if (xFF == "59") { cmd_len = 4; }
            if (xFF == "5A") { cmd_len = 2; }
            if (xFF == "5B") { cmd_len = 2; }
            if (xFF == "5C") { cmd_len = 3; }
            if (xFF == "5D") { cmd_len = 2; }
            if (xFF == "5E") { cmd_len = 2; }
            if (xFF == "5F") { cmd_len = 2; }

            if (xFF == "60") { cmd_len = 1; }	//was 14, might be just 1 or 2
            if (xFF == "61") { cmd_len = 4; }
            if (xFF == "62") { cmd_len = 2; }
            if (xFF == "63") { cmd_len = 1; }
            if (xFF == "64") { cmd_len = 16; }
            if (xFF == "65") { cmd_len = 2; }
            if (xFF == "66") { cmd_len = 1; }
            if (xFF == "67") { cmd_len = 28; }
            if (xFF == "68") { cmd_len = 40; }
            if (xFF == "69") { cmd_len = 30; }
            if (xFF == "6A") { cmd_len = 6; }
            if (xFF == "6B") { cmd_len = 4; }
            if (xFF == "6C") { cmd_len = 2; }   //was 1 before
            if (xFF == "6D") { cmd_len = 4; }
            if (xFF == "6E") { cmd_len = 6; }
            if (xFF == "6F") { cmd_len = 2; }

            if (xFF == "70") { cmd_len = 1; }
            if (xFF == "71") { cmd_len = 1; }
            if (xFF == "72") { cmd_len = 16; }
            if (xFF == "73") { cmd_len = 8; }
            if (xFF == "74") { cmd_len = 4; }
            if (xFF == "75") { cmd_len = 22; }
            if (xFF == "76") { cmd_len = 3; }
            if (xFF == "77") { cmd_len = 4; }
            if (xFF == "78") { cmd_len = 6; }
            if (xFF == "79") { cmd_len = 1; }
            if (xFF == "7A") { cmd_len = 16; }
            if (xFF == "7B") { cmd_len = 16; }
            if (xFF == "7C") { cmd_len = 6; }
            if (xFF == "7D") { cmd_len = 6; }
            if (xFF == "7E") { cmd_len = 6; }
            if (xFF == "7F") { cmd_len = 6; }

            if (xFF == "80") { cmd_len = 2; }
            if (xFF == "81") { cmd_len = 3; }	//seems to be 4...
            if (xFF == "82") { cmd_len = 3; }
            if (xFF == "83") { cmd_len = 1; }
            if (xFF == "84") { cmd_len = 2; }
            if (xFF == "85") { cmd_len = 6; }
            if (xFF == "86") { cmd_len = 1; }
            if (xFF == "87") { cmd_len = 1; }
            if (xFF == "88") { cmd_len = 3; }
            if (xFF == "89") { cmd_len = 1; }
            if (xFF == "8A") { cmd_len = 6; }
            if (xFF == "8B") { cmd_len = 6; }
            if (xFF == "8C") { cmd_len = 8; }
            if (xFF == "8D") { cmd_len = 24; }
            if (xFF == "8E") { cmd_len = 24; }

            if (xFF == "D4") { cmd_len = 2; }
            if (xFF == "ED") { cmd_len = 2; }

            return cmd_len;

        }



        // get total size of SCD block 
        private int SET_SCD2_TOTAL(int rdt_index)
        {

            int total_scd_sz = 0;


            Array.Sort(RDT_HEADER.nOffsets); // sort array 

            // set logical index to sub scd offset
            int logical_index = Array.BinarySearch(RDT_HEADER.nOffsets, SCD_SUB_OFFS[rdt_index]);


            // calculate and return size of main to next logical after sub to get the actual size of the entire scd section
            if (logical_index >= 0)
            {
                total_scd_sz = SCD_SUB_OFFS[logical_index + 1] - SCD_MAIN_OFFS[rdt_index];
            }


            return total_scd_sz;

        }

        /// <summary>
        /// Scan Dir for specific bit op/array flags
        /// </summary>
        /// <param name="op"></param>
        /// <param name="file"></param>
        /// <param name="index"></param>
        private void ARRAY_SCAN(byte op, int bitRay, int num, int val, string file, int index)
        {

            byte[] scd_buffer = new byte[0];


            int main_scd_sz = 0;
            int sub_scd_sz = 0;
            int subCount = 0;
            int scd_sz = 0;


            using (var fs = new FileStream(file, FileMode.Open))
            {

                using (BinaryReader br = new BinaryReader(fs))
                {

                    // sort current read RDT's offsets
                //    Array.Sort(RDT_HEADER.nOffsets);



                    int lgi = Array.BinarySearch(RDT_HEADER.nOffsets, SCD_SUB_OFFS[index]);

                    if (lgi >= 0)
                    {
                        scd_sz = RDT_HEADER.nOffsets[lgi + 1] - SCD_MAIN_OFFS[index]; // total size main+sub includes headers =( 
                        sub_scd_sz = RDT_HEADER.nOffsets[lgi + 1] - SCD_SUB_OFFS[index]; // sub size

                    }
                    Item_Offsets.Clear();

                    Array.Resize(ref scd_buffer, scd_sz);



                    //  Console.WriteLine("LOGICAL OFF: " + RDT_HEADER.nOffsets[lgi + 1]);
                    main_scd_sz = SCD_SUB_OFFS[index] - SCD_MAIN_OFFS[index]; // main size

                    //  Console.WriteLine("total sz : " + scd_sz);

                    fs.Seek(SCD_MAIN_OFFS[index], SeekOrigin.Begin);

                    // stored entire scd block , sub main in temp buffer
                    for (int j = 0; j < scd_sz; j++)
                    {
                        scd_buffer[j] = br.ReadByte();
                    }


                    for (int x = 0; x < scd_buffer.Length; x++)
                    {
                        if (x != scd_buffer.Length) {

                            if (scd_buffer[x] == op && scd_buffer[x + 1] == bitRay && scd_buffer[x + 2] == num && scd_buffer[x + 3] == val)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                Console.WriteLine("FOUND IN [" + file + "]!!");
                                
                            }
                            
                        }

                    }


                }
            }


        }


        /// <summary>
        /// Any list or array delcared in here will only be RDT relevant and not global..
        /// </summary>
        /// <param name="file"></param>
        /// <param name="index"></param>
        private void PARSE_SCD_BUFFER(string file, int index)
        {
            int cmd_len = 0;

            string difficulty = string.Empty;
            int scd_sz = 0;

            int item_aot_count = 0;
            int CKTEST_COUNT = 0;
            int Sce_ItemGet_Count = 0;
            int obj_model_count = 0;

            int emd_zombie_count = 0;


            int main_scd_sz = 0;
            int sub_scd_sz = 0;
            int subCount = 0;





          //  List<int> Obj_Model_Offsets = new List<int>();
            List<int> CK_Offsets = new List<int>();


            List<LIB_RDT.ITEM_DATA_OBJ> Item_Aots = new List<LIB_RDT.ITEM_DATA_OBJ>();

            List<LIB_RDT.CK_DATA_OBJ> CK_LIST = new List<LIB_RDT.CK_DATA_OBJ>();


            byte[] scd_buffer = new byte[0];



            RDT_DATA[index].f_rdt = file;

            using (var fs = new FileStream(file, FileMode.Open))
            {

                using (BinaryReader br = new BinaryReader(fs))
                {

                    // sort current read RDT's offsets
                    Array.Sort(RDT_HEADER.nOffsets);



                    int lgi = Array.BinarySearch(RDT_HEADER.nOffsets, SCD_SUB_OFFS[index]);
                    if (lgi >= 0)
                    {
                        scd_sz = RDT_HEADER.nOffsets[lgi + 1] - SCD_MAIN_OFFS[index]; // total size main+sub includes headers =( 
                        sub_scd_sz = RDT_HEADER.nOffsets[lgi + 1] - SCD_SUB_OFFS[index]; // sub size

                    }
                    Item_Offsets.Clear();

                    Array.Resize(ref scd_buffer, scd_sz);



                    //  Console.WriteLine("LOGICAL OFF: " + RDT_HEADER.nOffsets[lgi + 1]);
                    main_scd_sz = SCD_SUB_OFFS[index] - SCD_MAIN_OFFS[index]; // main size

                    //  Console.WriteLine("total sz : " + scd_sz);

                    fs.Seek(SCD_MAIN_OFFS[index], SeekOrigin.Begin);

                    // stored entire scd block , sub main in temp buffer
                    for (int j = 0; j < scd_sz; j++)
                    {
                        scd_buffer[j] = br.ReadByte();
                    }


                    RDT_DATA[index].List_Aot_Offs = new List<int>(); // create new offset lists for each rdt
                    RDT_DATA[index].LIST_EMD_OFFS = new List<int>(); // create new offset lists for emd offs
                    RDT_DATA[index].List_CK_OFFS = new List<int>(); // since each rdt has multiple offsets to these structures..

                    LIB_RDT.CK_DATA_OBJ TEMP_CK = new LIB_RDT.CK_DATA_OBJ();

                    // loop through temp buffer


               //    RDT_FUNCTIONS.ARRANGE_FILTER(scd_buffer, RDT_DATA, index, SCD_MAIN_OFFS);
               //     RDT_FUNCTIONS.SCENARIO_FILTER(scd_buffer, RDT_DATA, index, SCD_MAIN_OFFS);


                    // loop through buffer and count items / store offsets
                    for (int x = 0; x < scd_buffer.Length; x++)
                    {

                        if (x != scd_buffer.Length)
                        {

                            // if op ck detected

                            // lil op check 
                            //if (scd_buffer[x] == 0x22 && scd_buffer[x + 1] == 0x04 && scd_buffer[x + 2] == 0x4C && scd_buffer[x + 3] == 0x01)
                            //{
                            //    string f = file;
                            //    var t = x;

                            //}

                            //if (scd_buffer[x] == 0x22 && scd_buffer[x + 1] == 0x03 && scd_buffer[x + 2] == 131 && scd_buffer[x + 3] == 0x01)
                            //{
                            //    //    BIO2_LIB_RND.Disable_Letterbox(x, file, fs);

                            //    var t = 0;
                            //}


                            if (scd_buffer[x] == 0x21)
                            {
                                // if ck ends in a 0 or 1 its a ck..
                                if (scd_buffer[x + 3] == 0x01 || scd_buffer[x + 3] == 0x00)
                                {
                                    // store ck in temp
                                    TEMP_CK.op = scd_buffer[x];
                                    TEMP_CK.bit_array = scd_buffer[x + 1];
                                    TEMP_CK.num = scd_buffer[x + 2];
                                    TEMP_CK.val = scd_buffer[x + 3];
                                    
                                    //  CKTEST_COUNT += 1;
                                    //RDT_ITEM_DATA[index].List_CK_OFFS.Add(x + SCD_MAIN_OFFS[index]);
                                    //   CK_Offsets.Add(x + SCD_MAIN_OFFS[index]);
                                    //    RDT_DATA[index].List_CK_OFFS.Add(x + SCD_MAIN_OFFS[index]);


                                    //  store each read ck in current rdt's ck data


                                    // if SET LETTERBOX ON
                                 


                                        //    scenario filter
                                        //    if A / B CK // and its not nested in another ck

                                        if (TEMP_CK.bit_array == 1 && TEMP_CK.num == 1 && scd_buffer[x - 4] == 0x06)
                                        {
                                        // A                                                                 CK
                                        if (TEMP_CK.val == 0)
                                        {
                                            // store if block len
                                            short blk_len_A = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                            if (DEBUG_MODE == 1)
                                            {
                                                Console.WriteLine(file + "\\(A)" + blk_len_A);
                                            }

                                            // calculate else location of A (B)
                                            int else_loc = x + blk_len_A - 4;


                                            while (x != else_loc)
                                            {
                                                x++;

                                                // Scan Buffer for ARRANGE CHECK
                                                if (scd_buffer[x] == 0x21 && scd_buffer[x + 1] == 0x00 && scd_buffer[x + 2] == 0x19)
                                                {
                                                    // if ARRANGE CHECK IS NORMAL
                                                    if (scd_buffer[x + 3] == 0x00)
                                                    {
                                                        short DifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                                        int diff_els_loc = x + DifblkLen - 4;


                                                        while (x <= diff_els_loc)
                                                        {
                                                            x++;


                                                            if (scd_buffer[x] == 0x4E && scd_buffer[x + 2] == 0x02)
                                                            {

                                                                item_aot_count += 1; // rdt relative count
                                                                t_Items += 1; // global count


                                                                Item_Offsets.Add(x + SCD_MAIN_OFFS[index]); // RDT relative item offsets
                                                                RDT_DATA[index].List_Aot_Offs.Add(x + SCD_MAIN_OFFS[index]);

                                                                //  int n = x + SCD_MAIN_OFFS[index];
                                                                //Console.WriteLine(item_aot_count);

                                                            }
                                                            
                                                        }
                                                        
                                                    }
                                                    else
                                                    {
                                                        // if arrange..
                                                        if (scd_buffer[x + 3] == 0x01)
                                                        {
                                                            short Dif_Easy_BlkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);
                                                            int Dif_Easy_Else_Loc = x + Dif_Easy_BlkLen - 4;
                                                            x = Dif_Easy_Else_Loc;

                                                        }

                                                    }
                                                }



                                                if (scd_buffer[x] == 0x4E && scd_buffer[x + 2] == 0x02)
                                                {

                                                    item_aot_count += 1; // rdt relative count
                                                    t_Items += 1; // global count


                                                    Item_Offsets.Add(x + SCD_MAIN_OFFS[index]); // RDT relative item offsets
                                                    RDT_DATA[index].List_Aot_Offs.Add(x + SCD_MAIN_OFFS[index]);

                                                    //  int n = x + SCD_MAIN_OFFS[index];
                                                    //Console.WriteLine(item_aot_count);

                                                }


                                            }


                                            // this is skipping the if a...
                                            if (x == else_loc && scd_buffer[else_loc] == 0x07)
                                            {

                                                // get else length
                                                short els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[else_loc + 2], scd_buffer[else_loc + 3] }, 0);

                                                // skip B
                                                x = else_loc + els_blk_len;

                                            }



                                        }
                                        // IF B 
                                        if (TEMP_CK.val == 1)
                                        {

                                            short blk_len_B = BitConverter.ToInt16(new byte[2] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                            int else_loc = x + blk_len_B - 2;

                                            // if buffer index at end of if and its not end if..


                                            x = else_loc;

                                        }


                                    }

                                       

                                    // leon/claire filter
                                    if (TEMP_CK.bit_array == 1 && TEMP_CK.num == 0 && scd_buffer[x - 4] == 0x06)
                                    {   
                                        
                                        // IF LEON
                                        if (TEMP_CK.val == 0)
                                        {
                                            short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);


                                            int else_loc = x + ifblkLen - 4;


                                            while (x != else_loc)
                                            {

                                                x++;


                                                //  scan for arrange check inside leon / claire check... (LOL)
                                                if (scd_buffer[x] == 0x21 && scd_buffer[x + 1] == 0x00 && scd_buffer[x + 2] == 0x19)
                                                {
                                                    // if diff chk NORMAL
                                                    if (scd_buffer[x + 3] == 0x00)
                                                    {
                                                        short DifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                                        int diff_els_loc = x + DifblkLen - 4;


                                                        while (x <= diff_els_loc) { x++; }



                                                        if (x == diff_els_loc && scd_buffer[diff_els_loc] == 0x07)
                                                        {

                                                            // get else length
                                                            short dif_els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[diff_els_loc + 2], scd_buffer[diff_els_loc + 3] }, 0);

                                                            // skip else
                                                            x = else_loc + dif_els_blk_len;

                                                        }




                                                    }
                                                    else
                                                    {
                                                        short DifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                                        int diff_els_loc = x + DifblkLen - 4;
                                                        x = diff_els_loc;

                                                    }


                                                }


                                                // if item detected and type match..?
                                                if (scd_buffer[x] == 0x4E && scd_buffer[x + 2] == 0x02)
                                                {

                                                    item_aot_count += 1; // rdt relative count
                                                    t_Items += 1; // global count


                                                    Item_Offsets.Add(x + SCD_MAIN_OFFS[index]); // RDT relative item offsets
                                                    RDT_DATA[index].List_Aot_Offs.Add(x + SCD_MAIN_OFFS[index]);

                                                    //  int n = x + SCD_MAIN_OFFS[index];
                                                    //Console.WriteLine(item_aot_count);

                                                }


                                            }


                                            // if at else loc and its not end if skip easy
                                            if (x == else_loc && scd_buffer[else_loc] == 0x07)
                                            {

                                                // get else length
                                                short els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[else_loc + 2], scd_buffer[else_loc + 3] }, 0);

                                                // skip else
                                                x = else_loc + els_blk_len;

                                            }


                                        }



                                        //if (scd_buffer[x] == 0x4E && scd_buffer[x + 2] == 0x02)
                                        //{

                                        //    item_aot_count += 1; // rdt relative count
                                        //    t_Items += 1; // global count


                                        //    Item_Offsets.Add(x + SCD_MAIN_OFFS[index]); // RDT relative item offsets
                                        //    RDT_ITEM_DATA[index].List_Aot_Offs.Add(x + SCD_MAIN_OFFS[index]);

                                        //      int n = x + SCD_MAIN_OFFS[index];
                                        //    Console.WriteLine(item_aot_count);

                                        //}








                                    }



                                    


                                    // MR X FILTER.... 00
                                    if (TEMP_CK.bit_array == 0x06 && TEMP_CK.num == 0xC7 && scd_buffer[x - 4] == 0x06)
                                    {

                                        if (TEMP_CK.val == 0)
                                        {
                                            short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                            int else_loc = x + ifblkLen - 4;

                                            short els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[else_loc + 2], scd_buffer[else_loc + 3] }, 0);

                                            x = else_loc + els_blk_len;

                                        }



                                        if (TEMP_CK.val == 1)
                                        {
                                            short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                            int else_loc = x + ifblkLen - 4;

                                            short els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[else_loc + 2], scd_buffer[else_loc + 3] }, 0);

                                            x = else_loc + els_blk_len;


                                        }


                                    }


                                   // MR X FILTER... 01
                                    if (TEMP_CK.bit_array == 0x06 && TEMP_CK.num == 0xC9 && scd_buffer[x - 4] == 0x06)
                                    {

                                        if (TEMP_CK.val == 0)
                                        {
                                            short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                            int else_loc = x + ifblkLen - 4;

                                            short els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[else_loc + 2], scd_buffer[else_loc + 3] }, 0);

                                            x = else_loc + els_blk_len;

                                        }



                                        if (TEMP_CK.val == 1)
                                        {
                                            short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                            int else_loc = x + ifblkLen - 4;

                                            short els_blk_len = BitConverter.ToInt16(new byte[] { scd_buffer[else_loc + 2], scd_buffer[else_loc + 3] }, 0);

                                            x = else_loc + els_blk_len;


                                        }


                                    }


                                    // skip ingram in lab
                                    if (TEMP_CK.bit_array == 0x08 && TEMP_CK.num == 0x3E && scd_buffer[x - 4] == 0x06)
                                    {
                                        short ifblkLen = BitConverter.ToInt16(new byte[] { scd_buffer[x - 2], scd_buffer[x - 1] }, 0);

                                        int else_loc = x + ifblkLen - 4;

                                        x = else_loc;

                                    }



                                }

                            }


                            //   loose 1 // index != 122
                            if (scd_buffer[x] == 0x4E && scd_buffer[x + 2] == 0x02)
                            {

                                // skip rocket room + undefined knife/colt s.aa + some B sewer room with 2 much shit
                                if (scd_buffer[x + 14] != 0x01 && scd_buffer[x + 14] != 0x0D && index != 71 && index != 55 && index != 5 && index != 67 && index != 13 && index != 14)
                                {
                                    

                                        item_aot_count += 1; // rdt relative count
                                        t_Items += 1; // global count

                                        Item_Offsets.Add(x + SCD_MAIN_OFFS[index]); // RDT relative item offsets
                                        RDT_DATA[index].List_Aot_Offs.Add(x + SCD_MAIN_OFFS[index]);
                                    

                                }
                                //  int n = x + SCD_MAIN_OFFS[index];
                                //Console.WriteLine(item_aot_count);

                            }


                        }






                        // item check , store offsets

                        // detect and store op offsets and t_counts

                        //if (scd_buffer[x] == 0x2D && scd_buffer[x + 1] >= 0x01 && scd_buffer[x + 2] == 0x00 && scd_buffer[x+3] == 0x00)
                        //{

                        //    obj_model_count += 1;
                        //    Obj_Model_Offsets.Add(x + SCD_MAIN_OFFS[index]);
                        //}

                        // BIT CHKS , store offsets


                        //// cutscene op item get
                        //if (scd_buffer[x] == 0x76 && LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(scd_buffer[x + 1]))
                        //{
                        //    Sce_ItemGet_Count += 1;

                        //}


                    } // end of scd_buffer loop






                    //for (int x = 0; x < scd_buffer.Length; x++)
                    //{
                    //    if (scd_buffer[x] == 0x21)
                    //    {
                    //         if ck ends in a 0 or 1 its a ck..
                    //        if (scd_buffer[x + 3] == 0x01 || scd_buffer[x + 3] == 0x00)
                    //        {
                    //             store ck in temp
                    //            TEMP_CK.op = scd_buffer[x];
                    //            TEMP_CK.bit_array = scd_buffer[x + 1];
                    //            TEMP_CK.num = scd_buffer[x + 2];
                    //            TEMP_CK.val = scd_buffer[x + 3];

                    //            CKTEST_COUNT += 1;




                    //        }

                    //    }
                    //}


                    // rescan buffer for em_data
                    for (int x = 0; x < scd_buffer.Length; x++)
                    {
                        // if op 44 detected and em_id is in the lib..
                        if (scd_buffer[x] == 0x44 && LIB_EMD.BIO2_EMD_LUT.ContainsKey(scd_buffer[x + 3]))
                        {
                            emd_zombie_count += 1;
                            EMD_OFFSETS.Add(x + SCD_MAIN_OFFS[index]);
                            RDT_DATA[index].LIST_EMD_OFFS.Add(x + SCD_MAIN_OFFS[index]);

                        }
                    }


                    RDT_DATA[index].Item_Aot_Count = item_aot_count; // set cur rdt aot count to cur read item count
                    RDT_DATA[index].EMD_COUNT = emd_zombie_count; // set cur rdt to emd count (zombies only)
                                                                  //  Console.WriteLine(RDT_ITEM_DATA[index].Item_Aot_Count);

                    Array.Resize(ref RDT_DATA[index].Emd_Offsets, emd_zombie_count);
                    Array.Resize(ref RDT_DATA[index].Item_Aot_Offsets, item_aot_count); // resize each rdt's item offsets to relative count?
                    Array.Resize(ref RDT_DATA[index].ITEM_AOTS, item_aot_count); // resize item aot structures to relative rdt count?

                    Array.Resize(ref RDT_DATA[index].EM_DATA, emd_zombie_count); // resize each rdt emd array to cur rdt zombie count






                    //     Array.Resize(ref [index].offsets, item_aot_count);
                    Array.Resize(ref MD1_MODEL, obj_model_count);
                    Array.Resize(ref CK_TEST, CKTEST_COUNT);

                    //                    RDT_ITEM_TOTAL.Add(item_aot_count); // rdt relative item count will be stored in each list index


                


                    //foreach (int i in Obj_Model_Offsets)
                    //{
                    //    Console.WriteLine(i);
                    //}

                    // loop thru rdt item count, seek and run read


                    for (int z = 0; z < item_aot_count; z++)
                    {
                        fs.Seek(Item_Offsets[z], SeekOrigin.Begin);
                        // fs.Seek(RDT_ITEM_DATA[index].Item_Aot_Offsets[z], SeekOrigin.Begin);

                        //   RDT_AOT_OFFS[index].offsets[z] = int.Parse(fs.Position.ToString());

                        RDT_FUNCTIONS.Read_Item_Aots(fs, br, z, RDT_DATA[index].ITEM_AOTS, AllItems, DEBUG_MODE);

                    }


                    for (int z = 0; z < emd_zombie_count; z++)
                    {
                        fs.Seek(EMD_OFFSETS[z], SeekOrigin.Begin);
                        RDT_FUNCTIONS.Read_EM_SETS(fs, br, z, RDT_DATA[index].EM_DATA);
                    }

                    

                    // Array.Resize(ref ITEM_AOT[index].Offsets, item_aot_count);

                    //for (int x = 0; x < obj_model_count; x++)
                    //{

                    //    fs.Seek(Obj_Model_Offsets[x], SeekOrigin.Begin);
                    //    MD1_MODEL[x].op = br.ReadByte();
                    //    MD1_MODEL[x].md1 = br.ReadByte();

                    //}

                    //loop dump ck array tests
                    for (int i = 0; i < CKTEST_COUNT; i++)
                    {
                        fs.Seek(CK_Offsets[i], SeekOrigin.Begin);
                        CK_TEST[i].op = br.ReadByte();
                        CK_TEST[i].bit_array = br.ReadByte();
                        CK_TEST[i].num = br.ReadByte();
                        CK_TEST[i].val = br.ReadByte();


                        // store currently read ck struct into rdt relative ones
                      //  RDT_ITEM_DATA[index].CK_OPS[i] = CK_TEST[i];

                        Console.ForegroundColor = ConsoleColor.Red;

                        if (CK_TEST[i].bit_array == 0x00 && CK_TEST[i].num == 0x19)
                        {
                            if (CK_TEST[i].val == 0) { difficulty = "Easy"; }
                            if (CK_TEST[i].val == 1) { difficulty = "Normal"; }
                            if (DEBUG_MODE == 1)
                            {

                                Console.WriteLine("DIFFICULTY CHECK: " + difficulty);
                            }
                        }


                        Console.ForegroundColor = ConsoleColor.White;



                        if (CK_TEST[i].bit_array == 0x01 && CK_TEST[i].num == 0x06)
                        {

                            string GameType = string.Empty;

                            if (CK_TEST[i].val == 0x00) { GameType = "(Leon/Claire)"; }
                            if (CK_TEST[i].val == 0x01) { GameType = "(Hunk/Tofu)"; }

                            if (DEBUG_MODE == 1)
                            {
                                Console.WriteLine("GAMETYPE CHECK: " + GameType);
                            }

                        }


                        if (CK_TEST[i].bit_array == 0x01 && CK_TEST[i].num == 0x00)
                        {

                            string GameType = string.Empty;

                            if (CK_TEST[i].val == 0x00) { GameType = "(LEON)"; }
                            if (CK_TEST[i].val == 0x01) { GameType = "(CLAIRE)"; }

                            if (DEBUG_MODE == 1)
                            {
                                Console.WriteLine("LEON/CLAIRE CHECK: " + GameType);
                            }

                        }




                        if (CK_TEST[i].bit_array == 0x01 && CK_TEST[i].num == 0x01)
                        {
                            char Scenario = 'u';

                            if (CK_TEST[i].val == 0x00) { Scenario = 'A'; }
                            if (CK_TEST[i].val == 0x01) { Scenario = 'B'; }

                            Console.WriteLine("SCENARIO CHECK (" + Scenario + ")");

                        }
                        
                        CK_LIST.Add(CK_TEST[i]);
                    }


                    fs.Close();
                }
                
            }
            
        }

        
        public void Banner()
        {

            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine(Figgle.FiggleFonts.Ivrit.Render("BIO2 RNDX\t v0.50"));
            Console.WriteLine("##############################################\n---------------------------------------------");


            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Author");
            Console.WriteLine(Figgle.FiggleFonts.CyberSmall.Render("DCHAPS")); Console.Write("--------------------------------2018/\n");

            

        }

        public void Help()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            Console.Write("========================================================================================================================\n");
            Console.Write("BIO2 RNDX Version: 0.51 \t2018-2019, Dchaps\n\n");
            Console.Write("=======================================================================================================================\n\n");
            Console.Write("ARGS[0] = -l or -c (leon or claire)\n                                                                                     " );      
            Console.Write("=======================================================================================================================\n\n");
            Console.Write("=======================================================================================================================\n\n");


            Console.ReadKey();

            return;

            

        }

    }



}


