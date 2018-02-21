using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace bio2_rndx
{
    public class Program
    {

        // Item_Aot_set len x22
        public struct ITEM_DATA_OBJ
        {
            public byte opcode;
           


        }

        public struct ITEM_BUFFER_OBJ
        {
            public byte[] itemBuffer;

        }


        public ITEM_BUFFER_OBJ[] ItemBuffer = new ITEM_BUFFER_OBJ[0]; // resize based on RDT relative aot count

        public static Program prg = new Program();
        public static LIB_RDT.RDT_HEADER_OBJ RDT_HEADER = new LIB_RDT.RDT_HEADER_OBJ();
        public string Dir_Path = string.Empty;
        public string Config_Path = string.Empty;
        public string[] RDT_FILES = new string[100];
        public int[] SCD_MAIN_OFFS = new int[0];
        public int[] SCD_SUB_OFFS = new int[0];
        public int[] SCD_EOF = new int[0];



        static void Main(string[] args)
        {
            Console.BufferHeight = 32000;

            prg.Banner();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Config.INI"))
            {
                
                        
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CONFIG INI FOUND");

              


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
                    if (args[0] == "-sl")
                    {
                        string[] rdt_files = Directory.GetFiles(prg.Dir_Path + "\\pl0\\Rdt");


                        Array.Resize(ref prg.SCD_MAIN_OFFS, rdt_files.Length);
                        Array.Resize(ref prg.SCD_SUB_OFFS, rdt_files.Length);
                        Array.Resize(ref prg.SCD_EOF, rdt_files.Length);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("TOTAL RDT's FOUND: " + rdt_files.Length + "\n");

                        for (int i = 0; i < rdt_files.Length; i++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("\n[" + i.ToString() + "] " + "[" + rdt_files[i].Substring(rdt_files[i].Length - 12, 12) + "]");
                            Console.WriteLine("-------------------------------------------------------------");


                            prg.PARSE_RDT(rdt_files[i], i);
                            prg.PARSE_ITEM_AOTS(rdt_files[i], i);
                        }



                       

                        Console.Read();
                        

                    }
                    
                }
                else
                {
                    prg.Help();
                }

            }
            else
            {
                
               

            }

          
            
        }

        

       

        /// <summary>
        /// parse single rdt
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

                Console.ForegroundColor = ConsoleColor.White;

                //Console.WriteLine("**************************************************");
                //Console.WriteLine("nSprite: " + RDT_HEADER.nSPrite.ToString());
                //Console.WriteLine("nCut: " + RDT_HEADER.nCut.ToString());
                //Console.WriteLine("nModel: " + RDT_HEADER.noModel.ToString());
                //Console.WriteLine("nItem: " + RDT_HEADER.nItem.ToString());
                // Console.WriteLine("nDoor: " + RDT_HEADER.nDoor.ToString());
                //  Console.WriteLine("nRoom_At: " + RDT_HEADER.nRoom_At.ToString());
                //   Console.WriteLine("nReverb_lv: " + RDT_HEADER.Reverb_lv.ToString());
                //    Console.WriteLine("nSprite_Max: " + RDT_HEADER.nSprite_Max.ToString());



                for (int i = 0; i < RDT_HEADER.nOffsets.Length; i++)
                {
                    RDT_HEADER.nOffsets[i] = br.ReadInt32();
                    LIB_RDT.OFFSET_LIST.Add(RDT_HEADER.nOffsets[i]);
                    //     Console.WriteLine("[" + i.ToString() + "]" + LIB_RDT.LUT_RDT_OFFSET[i] + ": " + RDT_HEADER.nOffsets[i]);
                }


                Console.ForegroundColor = ConsoleColor.Yellow;
                SCD_MAIN_OFFS[index] = RDT_HEADER.nOffsets[16];
                SCD_SUB_OFFS[index] = RDT_HEADER.nOffsets[17];
                SCD_EOF[index] = RDT_HEADER.nOffsets[18];
                
                Console.WriteLine("MAIN OFF: " + SCD_MAIN_OFFS[index]);
               Console.WriteLine("SUB OFF: " + SCD_SUB_OFFS[index]);
               // Console.WriteLine("18th OFF " + SCD_EOF[index]);

             //   int scd_sum = SCD_MAIN_OFFS[index] + SCD_SUB_OFFS[index];
             //   Console.WriteLine("SCD SUM: " + scd_sum);


           


                LIB_RDT.OFFSET_LIST.Sort();
                

                fs.Close();
                br.Close();

            }

        }



        


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



        private void PARSE_ITEM_AOTS(string file, int index)
        {
            int cmd_len = 0;
            string Mainbytestr = string.Empty;
            string subbytestr = string.Empty;
            int scd_sz = 0;
            int item_aot_count = 0;
      
        

            byte[] opBuff = new byte[0];

            using (var fs = new FileStream(file, FileMode.Open))
            {

                using (BinaryReader br = new BinaryReader(fs))
                {
                    // seek to main scd

                    fs.Seek(SCD_MAIN_OFFS[index], SeekOrigin.Begin);
                    
                    
                    


                    //  SET_SCD2_TOTAL(index, SCD_MAIN_OFFS[index]);
                    //SCD_SUB_OFFS[index] - SCD_MAIN_OFFS[index] loop between main and sub

                  

                    Array.Sort(RDT_HEADER.nOffsets);

                    int lgi = Array.BinarySearch(RDT_HEADER.nOffsets, SCD_SUB_OFFS[index]);
                    if (lgi >= 0)
                    {
                        scd_sz = RDT_HEADER.nOffsets[lgi + 1] - SCD_MAIN_OFFS[index]; 
                    }

                    Console.WriteLine("scd sz : " + scd_sz);

                    for (int j = 0; j < scd_sz; j++)
                    {
                      //  Console.WriteLine("fs pos:" + fs.Position);

                        if (fs.Position != fs.Length)
                        {
                            
                            byte opchk = br.ReadByte();

                            if (opchk > 0x00 || opchk < 0x8D)
                            {
                                if (opchk != 0x4E)
                                {
                                    fs.Seek(fs.Position + SET_SCD2_CMD_LEN(opchk), SeekOrigin.Begin);
                                }
                            }

                            // if item_aot && multiple of 2
                           else if (opchk == 0x4E && fs.Position % 2 != 0)
                            {

                                int l = int.Parse(fs.Position.ToString());
                                l -= 1;

                                item_aot_count += 1;
                                Console.WriteLine("4e pos " + l);
                                // set oplen
                               // fs.Seek(-1, SeekOrigin.Current);
                              //  cmd_len = SET_SCD2_CMD_LEN(opchk);
                                //resize buffer to op len
                              //  Array.Resize(ref opBuff, cmd_len);
                                // read op into buffer
                               // opBuff = br.ReadBytes(cmd_len);


                            }
                        }

                    }
                    


                    // loop through buffer concat op into printable byte str
                  //  for (int z = 0; z < opBuff.Length; z++)
                //    {
                 //       Mainbytestr += opBuff[z].ToString("X").PadLeft(2, '0');
                 //   }


                  //  Console.WriteLine("MAIN SCD: " + Mainbytestr);
                    Console.WriteLine("ItemCount: " + item_aot_count);



                    br.Close();
                    fs.Close();



                    // fs.Seek(SCD_SUB_OFFS[index], SeekOrigin.Begin);

                    // int scdJump = br.ReadInt16();
                    // fs.Seek(SCD_SUB_OFFS[index] + scdJump, SeekOrigin.Begin);


                    // Array.Sort(RDT_HEADER.nOffsets); // sort array for array search
                    //int log_idx = Array.BinarySearch(RDT_HEADER.nOffsets, SCD_SUB_OFFS[index]); // get index of sub scd start out of list of offsets

                    // if (log_idx >= 0) 
                    // {
                    //     logOff = RDT_HEADER.nOffsets[log_idx + 1] - SCD_SUB_OFFS[index]; // get offset of next highest entry in list and subtract between current read rdt's sub scd offset
                    //     Console.WriteLine("log off: " + RDT_HEADER.nOffsets[log_idx + 1]);
                    // }

                    // // SCD_EOF[index] - SCD_SUB_OFFS[index]
                    // // loop between sub scd offset and next highest logical offset
                    // for (int x = 0; x < logOff); x++)
                    //{
                    //     byte sig = br.ReadByte();


                    //     if (sig == 0x4E && fs.Position % 2 != 0) // if read byte is 4E and is even
                    //     {
                    //         sub_item_aot_count += 1;

                    //             fs.Seek(-1, SeekOrigin.Current); // skip back one
                    //             cmd_len = SET_SCD2_CMD_LEN(sig); // set cmd len to op len

                    //             Array.Resize(ref opBuff, cmd_len); // resize op buffer to cmd len
                    //             opBuff = br.ReadBytes(cmd_len); // read op into buffer using length


                    //     }

                    // }


                    // for (int z = 0; z < opBuff.Length; z++)
                    // {
                    //     subbytestr += opBuff[z].ToString("X").PadLeft(2, '0');
                    // }



                    //     Console.WriteLine("SUB SCD: " + subbytestr);
                    // Console.WriteLine("SUb ItemCount: " + sub_item_aot_count);







                }

               

            }



            

        }

        public void Banner()
        {

            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine(Figgle.FiggleFonts.Ivrit.Render("BIO2 RNDX\t v0.012"));
            Console.WriteLine("##############################################\n---------------------------------------------");


            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Author");
            Console.WriteLine(Figgle.FiggleFonts.CyberSmall.Render("DCHAPS")); Console.Write("--------------------------------02/17/18\n");
        }

       public void Help()
        {


            Console.ForegroundColor = ConsoleColor.Cyan;

         

            Console.Write("========================================================================================================================\n");
            Console.Write("BIO2 RNDX Version: 0.11 \t2018, Dchaps\n\n");
            Console.Write("=======================================================================================================================\n\n");
            Console.Write("\t Initialising Skynet subroutine XR-321AGV4F...\n\n");
            Console.Write("=======================================================================================================================\n\n");

            Console.Write("=======================================================================================================================\n\n");
            Console.WriteLine("arguments: -mode -directory path");
            Console.WriteLine("Modes -rx (random seed xtreme) -ri (random seed items) ");

            Console.Read();
            
            

        }

       


    }
}
