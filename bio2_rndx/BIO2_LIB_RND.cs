using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;


namespace bio2_rndx
{


    /// <summary>
    /// ALL RANDOMIZATION FUNCTIONS WORK ON A PER RDT FILE BASIS..., USING INDEXED RDT's
    /// </summary>
    public static class BIO2_LIB_RND
    {


        //public static void Postisitive_Enforcment(string rdt_file, int rdt_index, List<int> TargetIndicies)
        //{

        //}


        // only shuffles between reg key positions already
        //Randomize Key Items Only (-KO PARAM)
        /// <param name="Dir"></param>
        /// <param name="rdt_file"></param>
        /// <param name="rdt_index"></param>


        /// <summary>
        /// Shuffles common items and keys?
        /// </summary>
        /// <param name="rdt_file"></param>
        /// <param name="rdt_index"></param>
        /// <param name="rdt_item_count"></param>
        /// <param name="t_count"></param>
        /// <param name="RDT_DATA"></param>
        /// <param name="AllItems"></param>
        public static void Shuffle_CK(string rdt_file, int rdt_index, int rdt_item_count, ref int t_count, LIB_RDT.RDT_OBJ[] RDT_DATA, List<LIB_RDT.ITEM_DATA_OBJ> AllItems, List<string> outputstrings)
        {

            Random r_Item = new Random();
            Random r_Amount = new Random();
            Random r_Glimmer = new Random();

            bool Kendo_shotgun = false;

            int r_quantity = 0;
            // array of glimmer values.. non kneeling..
            byte[] GLIMMER_SET = new byte[] { 0xA0, 0xB0, 0xC0, 0xD0, 0xE0, 0xF0 };
            byte[] GLIMMER_SET_C = new byte[] { 0xA1, 0xB1, 0xC1, 0xD1, 0xE1, 0xF1 };


            // List<string> OutputStrngs = new List<string>();
            //  Array.Resize(ref Seed_Entries[rdt_index].Item, rdt_item_count);
            //   Array.Resize(ref Seed_Entries[rdt_index].Offsets, rdt_item_count);

            // open streams
            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {

                    // Console.WriteLine(RDT_ITEM_DATA[rdt_index].f_rdt.Substring(RDT_ITEM_DATA[rdt_index].f_rdt.Length - 12, 8));
                    // Console.WriteLine("------------------------------------------------------");

                    for (int x = 0; x < rdt_item_count; x++) // should be the same as # of offsets per file
                    {
                        byte Glimmer_Roll = byte.Parse(r_Glimmer.Next(0, GLIMMER_SET.Length).ToString()); // new roll per item..

                        //      if( LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                        if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item) || LIB_ITEM.BIO2_COMMON_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                        {
                            t_count++; // create ur own indexer for every valid item

                            if (t_count != AllItems.Count)
                            {
                                // if (t_count == 145) { t_count = 143; }
                                RDT_DATA[rdt_index].ITEM_AOTS[x] = AllItems[t_count]; // assign to shuffled index                                    
                            }

                            // if weapon/ammo
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item >= 0x00 && RDT_DATA[rdt_index].ITEM_AOTS[x].item <= 0x1F)
                            {
                                // not a knife? randomize quantity between min/max capacity 
                                if (LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item] > 2)
                                { 
                                    r_quantity = r_Amount.Next(2, LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                                }
                                else
                                {
                                    r_quantity = r_Amount.Next(1, LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                                }
                                // RDT_DATA[rdt_index].ITEM_AOTS[x].amount = short.Parse(r_quantity.ToString());
                                RDT_DATA[rdt_index].ITEM_AOTS[x].amount = (byte)r_quantity;
                            }
                            // if one of the suit keys then adjust quantity
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x59 || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5A || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5B || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5C)
                            {
                                RDT_DATA[rdt_index].ITEM_AOTS[x].amount = LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item];
                            }


                            // set crouch on keys only?
                            if (!LIB_ITEM.BIO2_COMMON_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                            {
                                RDT_DATA[rdt_index].ITEM_AOTS[x].ani = GLIMMER_SET_C[Glimmer_Roll]; // randomly assign a glimmer color
                            }
                            else
                            {
                                RDT_DATA[rdt_index].ITEM_AOTS[x].ani = GLIMMER_SET[Glimmer_Roll];
                            }


                            // debug log color code...
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item >= 0x01 && RDT_DATA[rdt_index].ITEM_AOTS[x].item <= 0x13)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item) || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x1F)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x59)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            }
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5A)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            }
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5B)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }

                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5C)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            if (LIB_ITEM.BIO2_COMMON_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item) && RDT_DATA[rdt_index].ITEM_AOTS[x].item > 0x13)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            }
                            // Console.WriteLine(t_count + "] " + LIB_ITEM.BIO2_ITEM_LUT[AllItems[t_count].item] + AllItems[t_count].amount);


                            // **************************************************************** DUMP OUTPUT SHUFFLE LOG ***************************************************************************//
                            // --------------------------------------------------------------------------------------------------------------------------------------------------------------------//

                            //   string rdt_num = rdt_file.Substring(57, 4);
                            //string rdt_str = string.Empty;


                            //if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_num))
                            //{
                            //    rdt_str = LIB_ROOM.LUT_ROOM[rdt_num];                           
                            //}


                            //Seed_Entries[t_count].count_per_room = (byte)rdt_item_count;                          
                            //Seed_Entries[t_count].rdt_id = rdt_num;
                            //Seed_Entries[t_count].ITEM = RDT_DATA[rdt_index].ITEM_AOTS[x];



                            //    string outStr = t_count + ")" + "ROOM" + rdt_num + "]" + LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")";

                            //                            OutputStrngs.Add(outStr);


                            // "ROOM" + rdt_num 

                            try
                            {

                                outputstrings.Add(rdt_file.Substring(rdt_file.Length - 12, 8) + " (" + LIB_ROOM.LUT_ROOM[rdt_file.Substring(rdt_file.Length - 8, 4)] + ") " + LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")");
                                Console.WriteLine(t_count + " " + rdt_file.Substring(rdt_file.Length - 12, 8) + " (" + LIB_ROOM.LUT_ROOM[rdt_file.Substring(rdt_file.Length - 8, 4)] + ") " + LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")");
                                // Console.Write(LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")" + "\n");
                                // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------//
                            }
                            catch (KeyNotFoundException KNF)
                            {

                                if (Program.DEBUG_MODE == 1)
                                {
                                    Console.WriteLine("KNF Exception: " + KNF.Message);
                                }
                            }



                            // Console.WriteLine(rdt_index + "]\\" + LIB_ITEM.BIO2_ITEM_LUT[RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].item] + "Q\\ " + RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].amount + " [" + RDT_ITEM_DATA[rdt_index].List_Aot_Offs[x] + "]");

                            //  Console.WriteLine(x + LIB_ITEM.BIO2_ITEM_LUT[RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].amount + ")");
                        }


                        // seek to item in file
                        //Console.WriteLine("Seeked struct off " + LIB_ITEM.BIO2_ITEM_LUT[Item_AOT[i].item]);


                        // fix this nigga kendos shotgun.. stupid aot_Reset..
                        if (rdt_index == 0x01)
                        {
                            int tx = rdt_index;
                            fs.Seek(6211, SeekOrigin.Begin);
                            bw.Write(0x00);
                        }


                        // fix brad aot_Reset overwrite item ID (U1)
                        if (rdt_index == 0x03)
                        {
                            fs.Seek(17450, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);

                            // if one of the keys use the quantity look up for the right amount of uses
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x59 || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5A || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5B || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5C)
                            {
                                fs.Seek(+1, SeekOrigin.Current);
                                bw.Write((byte)LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                            }// anything else just write the quantity
                            else
                            {
                                fs.Seek(+1, SeekOrigin.Current);
                                bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount);
                            }

                        }



                        // statue fix quantity reset?
                        if (rdt_index == 0x0C)
                        {
                            // if one of the keys use the quantity look up for the right amount of uses
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x59 || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5A || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5B || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5C)
                            {
                                fs.Seek(7000, SeekOrigin.Current);
                                bw.Write((byte)LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                            }// anything else just write the quantity
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x59 || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5A || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5B || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5C)
                            {
                                fs.Seek(7106, SeekOrigin.Current);
                                bw.Write((byte)LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                            }// anything else just write the quantity

                        }





                        // fix cog room aot_reset u1
                        if (rdt_index == 43 && x == 0)
                        {
                            fs.Seek(6192, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                        }

                        if (rdt_index == 12)
                        {
                            fs.Seek(6998, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);

                            fs.Seek(7104, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);


                        }

                        // fix clock room resets
                        if (rdt_index == 17)
                        {
                            fs.Seek(11176, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount);

                        }


                        // patch main hall spade key aot_reset
                        if (rdt_index == 30 && x == 0)
                        {
                            fs.Seek(10948, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount);
                        }


                        // fix sewer save room basement/lighter aot_Reset
                        if (rdt_index == 73)
                        {
                            fs.Seek(11056, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[4].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[4].amount);

                            fs.Seek(11994, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[4].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[4].amount);


                            fs.Seek(11178, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[5].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[5].amount);

                        }


                        if (rdt_index == 112)
                        {

                            fs.Seek(4072, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[0].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[0].amount);


                            fs.Seek(4110, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[0].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[0].amount);

                        }


                        // sg parts fix
                        if (rdt_index == 88) { byte box_fix = 0x10; RDT_DATA[rdt_index].ITEM_AOTS[x].flag = 0x15; RDT_DATA[rdt_index].ITEM_AOTS[x].ani = 0x01; fs.Seek(5540, SeekOrigin.Begin); bw.Write(box_fix); }



                        // seek to each item id
                        //Seed_Entries[t_count].Offset = RDT_DATA[rdt_index].List_Aot_Offs[x] + 14;
                        fs.Seek(RDT_DATA[rdt_index].List_Aot_Offs[x] + 14, SeekOrigin.Begin);



                        // re write newly assigned
                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                        fs.Seek(+1, SeekOrigin.Current);
                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount); //  rewrite quantity

                        // bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].flag);
                        //  bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].md1);

                        // skip md1 write
                        fs.Seek(+4, SeekOrigin.Current);

                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].ani);


                    }
                    // Console.WriteLine("count: " + t_count);

                }


            }



            //using (StreamWriter sw = new StreamWriter(rdt_file.Substring(0, rdt_file.Length - 12) + "\\SEEDDUMP.txt")) { 

            //    foreach (string str in OutputStrngs)
            //    {  
            //        sw.WriteLine(str);
            //    }

            //}





        }



        public static void Shuffle_CK_Claire(string rdt_file, int rdt_index, int rdt_item_count, ref int t_count, LIB_RDT.RDT_OBJ[] RDT_DATA, List<LIB_RDT.ITEM_DATA_OBJ> AllItems)
        {

            Random r_Item = new Random();
            Random r_Amount = new Random();
            Random r_Glimmer = new Random();

            bool Kendo_shotgun = false;

            int r_quantity = 0;
            // array of glimmer values.. non kneeling..
            byte[] GLIMMER_SET = new byte[] { 0xA0, 0xB0, 0xC0, 0xD0, 0xE0, 0xF0 };
            byte[] GLIMMER_SET_C = new byte[] { 0xA1, 0xB1, 0xC1, 0xD1, 0xE1, 0xF1 };


            List<string> OutputStrngs = new List<string>();
            //  Array.Resize(ref Seed_Entries[rdt_index].Item, rdt_item_count);
            //   Array.Resize(ref Seed_Entries[rdt_index].Offsets, rdt_item_count);

            // open streams
            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {

                    // Console.WriteLine(RDT_ITEM_DATA[rdt_index].f_rdt.Substring(RDT_ITEM_DATA[rdt_index].f_rdt.Length - 12, 8));
                    // Console.WriteLine("------------------------------------------------------");

                    for (int x = 0; x < rdt_item_count; x++) // should be the same as # of offsets per file
                    {
                        byte Glimmer_Roll = byte.Parse(r_Glimmer.Next(0, GLIMMER_SET.Length).ToString()); // new roll per item..

                        //      if( LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                        if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item) || LIB_ITEM.BIO2_COMMON_LUT_CA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                        {
                            t_count++; // create ur own indexer for every valid item

                            if (t_count != AllItems.Count)
                            {
                                // if (t_count == 145) { t_count = 143; }
                                RDT_DATA[rdt_index].ITEM_AOTS[x] = AllItems[t_count]; // assign to shuffled index                                    
                            }

                            // if weapon/ammo range
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item >= 0x00 && RDT_DATA[rdt_index].ITEM_AOTS[x].item <= 0x1F)
                            {
                                try
                                {

                                    if (LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item] > 2)
                                    {
                                        r_quantity = r_Amount.Next(2, LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                                    }
                                    else
                                    {
                                        r_quantity = r_Amount.Next(1, LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                                    }
                                }
                                catch (Exception ex)
                                {


                                }
                                // RDT_DATA[rdt_index].ITEM_AOTS[x].amount = short.Parse(r_quantity.ToString());
                                RDT_DATA[rdt_index].ITEM_AOTS[x].amount = (byte)r_quantity;
                            }
                            // if one of the suit keys then adjust quantity
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x59 || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5A || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5B || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5C)
                            {
                                RDT_DATA[rdt_index].ITEM_AOTS[x].amount = LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item];
                            }


                            // set crouch on keys only?
                            if (LIB_ITEM.BIO2_COMMON_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                            {
                                RDT_DATA[rdt_index].ITEM_AOTS[x].ani = GLIMMER_SET_C[Glimmer_Roll]; // randomly assign a glimmer color
                            }
                            else
                            {
                                RDT_DATA[rdt_index].ITEM_AOTS[x].ani = GLIMMER_SET[Glimmer_Roll];
                            }


                            // debug log color code...
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item >= 0x01 && RDT_DATA[rdt_index].ITEM_AOTS[x].item <= 0x13)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item) || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x1F)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x59)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            }
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5A)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            }
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5B)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }

                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5C)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            if (LIB_ITEM.BIO2_COMMON_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item) && RDT_DATA[rdt_index].ITEM_AOTS[x].item > 0x13)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            }
                            // Console.WriteLine(t_count + "] " + LIB_ITEM.BIO2_ITEM_LUT[AllItems[t_count].item] + AllItems[t_count].amount);


                            // **************************************************************** DUMP OUTPUT SHUFFLE LOG ***************************************************************************//
                            // --------------------------------------------------------------------------------------------------------------------------------------------------------------------//

                            string rdt_num = rdt_file.Substring(57, 4);
                            //string rdt_str = string.Empty;


                            //if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_num))
                            //{
                            //    rdt_str = LIB_ROOM.LUT_ROOM[rdt_num];                           
                            //}


                            //Seed_Entries[t_count].count_per_room = (byte)rdt_item_count;                          
                            //Seed_Entries[t_count].rdt_id = rdt_num;
                            //Seed_Entries[t_count].ITEM = RDT_DATA[rdt_index].ITEM_AOTS[x];



                            //    string outStr = t_count + ")" + "ROOM" + rdt_num + "]" + LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")";

                            //                            OutputStrngs.Add(outStr);


                            // "ROOM" + rdt_num 
                            Console.WriteLine(t_count + ")" + "ROOM " + rdt_num + "]" + LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")");
                            // Console.Write(LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")" + "\n");
                            // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------//




                            // Console.WriteLine(rdt_index + "]\\" + LIB_ITEM.BIO2_ITEM_LUT[RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].item] + "Q\\ " + RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].amount + " [" + RDT_ITEM_DATA[rdt_index].List_Aot_Offs[x] + "]");

                            //  Console.WriteLine(x + LIB_ITEM.BIO2_ITEM_LUT[RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].amount + ")");
                        }


                        // seek to item in file
                        //Console.WriteLine("Seeked struct off " + LIB_ITEM.BIO2_ITEM_LUT[Item_AOT[i].item]);


                        // fix this nigga kendos shotgun.. stupid aot_Reset..
                        //if (rdt_index == 0x01)
                        //{
                        //    int tx = rdt_index;
                        //    fs.Seek(6211, SeekOrigin.Begin);
                        //    bw.Write(0x00);

                        //}


                        // fix brad aot_Reset overwrite item ID (U1)
                        if (rdt_index == 0x03)
                        {
                            fs.Seek(17450, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);

                            // if one of the keys use the quantity look up for the right amount of uses
                            if (RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x59 || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5A || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5B || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x5C)
                            {
                                fs.Seek(+1, SeekOrigin.Current);
                                bw.Write((byte)LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                            }// anything else just write the quantity
                            else
                            {
                                fs.Seek(+1, SeekOrigin.Current);
                                bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount);
                            }

                        }





                        // fix cog room aot_reset u1
                        if (rdt_index == 43 && x == 0)
                        {
                            fs.Seek(6192, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                        }

                        if (rdt_index == 12)
                        {
                            fs.Seek(6998, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);

                            fs.Seek(7104, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);


                        }

                        // fix clock room resets
                        if (rdt_index == 17)
                        {
                            fs.Seek(11176, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount);

                        }


                        // patch main hall spade key aot_reset
                        if (rdt_index == 30 && x == 0)
                        {
                            fs.Seek(10948, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount);
                        }


                        //// fix sewer save room basement/lighter aot_Reset
                        //if (rdt_index == 73)
                        //{
                        //    fs.Seek(11056, SeekOrigin.Begin);
                        //    bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[4].item);
                        //    fs.Seek(+1, SeekOrigin.Current);
                        //    bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[4].amount);

                        //    fs.Seek(11994, SeekOrigin.Begin);
                        //    bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[4].item);
                        //    fs.Seek(+1, SeekOrigin.Current);
                        //    bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[4].amount);


                        //    fs.Seek(11178, SeekOrigin.Begin);
                        //    bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[5].item);
                        //    fs.Seek(+1, SeekOrigin.Current);
                        //    bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[5].amount);

                        //}


                        if (rdt_index == 112)
                        {

                            fs.Seek(4072, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[0].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[0].amount);


                            fs.Seek(4110, SeekOrigin.Begin);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[0].item);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[0].amount);

                        }


                        // sg parts fix
                        if (rdt_index == 88) { byte box_fix = 0x10; RDT_DATA[rdt_index].ITEM_AOTS[x].flag = 0x15; RDT_DATA[rdt_index].ITEM_AOTS[x].ani = 0x01; fs.Seek(5540, SeekOrigin.Begin); bw.Write(box_fix); }




                        // re write newly assigned
                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                        fs.Seek(+1, SeekOrigin.Current);
                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount); //  rewrite quantity

                        // bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].flag);
                        //  bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].md1);

                        // skip md1 write
                        fs.Seek(+4, SeekOrigin.Current);

                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].ani);


                    }
                    // Console.WriteLine("count: " + t_count);



                }


            }



            //using (StreamWriter sw = new StreamWriter(rdt_file.Substring(0, rdt_file.Length - 12) + "\\SEEDDUMP.txt")) { 

            //    foreach (string str in OutputStrngs)
            //    {  
            //        sw.WriteLine(str);
            //    }

            //}





        }


        /// <summary>
        /// KNUTH FISHER YATES SHUFFLE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="rng"></param>
        public static void KFY_Shuffle<T>(this IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Generates a list of random numbers between paramters
        /// </summary>
        /// <param name="Enum_Min"></param>
        /// <param name="Enum_Max"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static List<int> Gen_Index(int Enum_Min, int Enum_Max, int take)
        {
            Random random = new Random();

            //  var IndexArray = Enumerable.Range(0, 10).OrderBy(t => random.Next()).ToArray();
            var IndexArray = Enumerable.Range(Enum_Min, Enum_Max).OrderBy(t => random.Next()).Take(take).ToList();
            //for (int i = 0; i < IndexArray.Length; i++)
            //{
            //    Console.WriteLine("Result " + IndexArray[i]);
            //}

            return IndexArray;
        }

        /// <summary>
        /// Swap List indices
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        /// <returns></returns>
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }


        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }


        /// <summary>
        /// Patch in some items ahead of time so they get thrown in the mixture, seems to work good for arrange patching ahead of time aswell
        /// </summary>
        /// <param name="rdt_file"></param>
        /// <param name="rdt_index"></param>
        public static void Item_Patch(string rdt_file, int rdt_index)
        {
            Random r_Marvin = new Random();



            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    // patch in flame fuel
                    if (rdt_index == 0x01)
                    {
                        fs.Seek(6202, SeekOrigin.Begin);
                        bw.Write((byte)0x17);
                    }

                    if (rdt_index == 99)
                    {
                        fs.Seek(11516, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(11538, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(11606, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }



                    if (rdt_index == 0x02) // van
                    {
                        fs.Seek(7838, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                        //7838
                    }

                    // patch out arrange item aot (front of rpd)
                    if (rdt_index == 0x03)
                    {
                        fs.Seek(14082, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }

                    if (rdt_index == 0x06)
                    {
                        fs.Seek(3122, SeekOrigin.Begin);
                        bw.Write((byte)0x2A);
                    }



                    if (rdt_index == 0x2C)
                    {
                        fs.Seek(6534, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }


                    // if art storage room patch arrange
                    if (rdt_index == 0x0B)
                    {

                        fs.Seek(5978, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(6230, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }


                    if (rdt_index == 13)
                    {
                        fs.Seek(2134, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }

                    // 2f statue hall, arrange item
                    if (rdt_index == 0x0C)
                    {
                        fs.Seek(6840, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }

                    // patch in handgun over 2nd shotgun?
                    if (rdt_index == 0x0F)
                    {
                        fs.Seek(1706, SeekOrigin.Begin);
                        bw.Write((byte)0x5C); // club?
                    }


                    // ROOM1180(misty alley) patch out arrange item_aot
                    if (rdt_index == 0x18)
                    {
                        fs.Seek(7930, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }

                    if (rdt_index == 17)
                    {
                        fs.Seek(8284, SeekOrigin.Begin);
                        bw.Write((byte)0x3D);
                    }

                    // blue coke hall
                    if (rdt_index == 19)
                    {
                        fs.Seek(3822, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }

                    // STARS ROOM ARRANGE MODE ITEM PATCH + random desk
                    if (rdt_index == 21)
                    {

                        byte desk_count = (byte)r_Marvin.Next(1, 50);

                        fs.Seek(5218, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(6688, SeekOrigin.Begin);
                        bw.Write(desk_count);
                        ;
                    }


                    if (rdt_index == 30)
                    {

                        fs.Seek(9966, SeekOrigin.Begin);
                        bw.Write((byte)0x02);
                    }

                    //  patch in marvin random common item + patch out arrange item
                    if (rdt_index == 32)
                    {
                        //fs.Seek(8945, SeekOrigin.Begin);


                        //byte R_Mode = (byte)(r_Marvin.Next(0, 2));



                        //byte R_Item = 0x00;
                        //byte R_Quan = 0x00;




                        //// if marvin is common mode
                        //if (R_Mode == 0x00)
                        //{
                        //     R_Item = LIB_ITEM.BIO2_COMMON_LUT_LA.ElementAt(r_Marvin.Next(0, LIB_ITEM.BIO2_COMMON_LUT_LA.Count)).Key;

                        //     R_Quan = 0x00;
                        //    //     byte R_Item =  (byte)r_Marvin.Next(0, 0x2E);


                        //    // if common dict contains the random item and its not 0
                        //    if (LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(R_Item) && R_Item != 0)
                        //    {

                        //        if (LIB_ITEM.BIO2_LUT_QUANTITY.ContainsKey(R_Item))
                        //        {
                        //            R_Quan = (byte)r_Marvin.Next(0, LIB_ITEM.BIO2_LUT_QUANTITY[R_Item]);
                        //        }
                        //        else
                        //        {
                        //            R_Quan = 0x01;
                        //        }


                        //    }

                        //}

                        //if (R_Mode == 0x01)
                        //{

                        //     R_Item = LIB_ITEM.BIO2_KEY_LUT_LA.ElementAt(r_Marvin.Next(0, LIB_ITEM.BIO2_KEY_LUT_LA.Count)).Key;

                        //     R_Quan = 0x00;
                        //    //     byte R_Item =  (byte)r_Marvin.Next(0, 0x2E);

                        //    if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(R_Item) && R_Item != 0)
                        //    {
                        //        // R_Quan = (byte)r_Marvin.Next(0, LIB_ITEM.BIO2_LUT_QUANTITY[R_Item]);

                        //        if (LIB_ITEM.BIO2_LUT_QUANTITY.ContainsKey(R_Item))
                        //        {
                        //            R_Quan = (byte)0x03;
                        //        }
                        //        else
                        //        {
                        //            R_Quan = (byte)0x01;
                        //        }

                        //    }

                        //}

                        //bw.Write(R_Item);
                        //bw.Write(R_Quan);


                        fs.Seek(7394, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                        fs.Seek(8074, SeekOrigin.Begin);
                        bw.Write(0x00); // get rid of marvin scene entirely

                    }

                    // patch in over ink ribbon + patch out arrange
                    if (rdt_index == 33)
                    {
                        fs.Seek(1794, SeekOrigin.Begin);
                        bw.Write((byte)0x2B);

                        fs.Seek(2064, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }

                    // dark room arrange item
                    if (rdt_index == 38)
                    {
                        // patch in rocket over film pic A
                        fs.Seek(5110, SeekOrigin.Begin);
                        bw.Write(0x26);

                        // mixed herb
                        fs.Seek(5132, SeekOrigin.Begin);
                        bw.Write(0x2E);

                        // smg clip
                        fs.Seek(5154, SeekOrigin.Begin);
                        bw.Write(0x1B);


                        fs.Seek(5360, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);



                        fs.Seek(5384, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);



                        // g+g herb over film d pic
                        fs.Seek(5420, SeekOrigin.Begin);
                        bw.Write(0x29);


                    }

                    // evidence locker extra film patch
                    if (rdt_index == 39)
                    {
                        fs.Seek(3330, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }

                    // east office arrange item + patch in small key over ink + flip hunk mode to avoid dead zombie status + patch invis item model
                    if (rdt_index == 41)
                    {

                        // fix dead guy targeting status
                        fs.Seek(12373, SeekOrigin.Begin);
                        bw.Write((byte)0x00);

                        // patch in SMG AMMO key over ink ribbon
                        fs.Seek(14074, SeekOrigin.Begin);
                        bw.Write((byte)0x1B);

                        fs.Seek(14020, SeekOrigin.Begin);
                        bw.Write((byte)0x02); // update MD1 for sparkle/destroy

                        // enable B handgun model
                        fs.Seek(14089, SeekOrigin.Begin);
                        bw.Write((byte)0x00);

                        // shift b handgun model to invis spot
                        fs.Seek(14104, SeekOrigin.Begin);
                        bw.Write((Int16)(-2703));
                        bw.Write((Int16)(0));
                        bw.Write((Int16)(-24979));



                        // patch b item trigger

                        fs.Seek(14128, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                        // remove arrange item trigger
                        fs.Seek(14182, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);



                    }

                    if (rdt_index == 42)
                    {
                        fs.Seek(7414, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                        fs.Seek(7534, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                    }


                    // press room arranged
                    if (rdt_index == 43)
                    {
                        fs.Seek(4416, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }


                    // patch out arrange + B item
                    if (rdt_index == 44)
                    {

                        fs.Seek(6618, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);



                        //arrange
                        fs.Seek(6898, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);






                    }

                    // yellow hall arrange items
                    if (rdt_index == 45)
                    {
                        fs.Seek(6228, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                        fs.Seek(6430, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }


                    /// night watch arrange item
                    if (rdt_index == 46)
                    {

                        fs.Seek(4962, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }

                    // basement arrange items
                    if (rdt_index == 47)
                    {
                        fs.Seek(6018, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }

                    // patch kennel door aot during events + force C film to spawn A/B flip
                    if (rdt_index == 53)
                    {

                        // force film C in play
                        fs.Seek(2723, SeekOrigin.Begin);
                        bw.Write((byte)0x01);

                        fs.Seek(2922, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(3776, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }

                    //// patch in blue keycard over club key clone
                    if (rdt_index == 62)
                    {
                        fs.Seek(8066, SeekOrigin.Begin);
                        bw.Write((byte)0x35);

                    }

                    if (rdt_index == 70)
                    {
                        fs.Seek(6758, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(6818, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                    }

                    // valve area (sewer) patch out arrange
                    if (rdt_index == 77)
                    {
                        fs.Seek(10654, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }


                    if (rdt_index == 85)
                    {
                        fs.Seek(5078, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(7178, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                        fs.Seek(7200, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);




                    }


                    if (rdt_index == 88)
                    {
                        fs.Seek(5570, SeekOrigin.Begin);
                        bw.Write((int)0x00); // 4
                        bw.Write((int)0x00); // 8
                        bw.Write((int)0x00); // 12
                        bw.Write((int)0x00); //16
                        bw.Write((int)0x00); // 20
                        bw.Write((short)0x00); // 22

                    }

                    if (rdt_index == 90)
                    {

                        fs.Seek(7476, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }


                    if (rdt_index == 92)
                    {



                        fs.Seek(3520, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                        fs.Seek(3584, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(8570, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);



                        fs.Seek(8894, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                    }


                    if (rdt_index == 93)
                    {

                        fs.Seek(8570, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                        fs.Seek(8596, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);

                        fs.Seek(8894, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);


                        fs.Seek(8920, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);



                    }
                    //// patch in small key over Ink ribbon
                    if (rdt_index == 99)
                    {
                        fs.Seek(11552, SeekOrigin.Begin);
                        bw.Write((byte)0x17);
                    }

                    //// patch in flame can over ink ribbon
                    if (rdt_index == 105)
                    {
                        fs.Seek(1122, SeekOrigin.Begin);
                        bw.Write((byte)0x17);
                    }

                    //if (rdt_index == 107) {

                    //    fs.Seek()
                    //}


                    //if (rdt_index == 112) {

                    //}


                    /// should fix the 3rd arrange item in birkins lab
                    if (rdt_index == 117)
                    {
                        fs.Seek(12252, SeekOrigin.Begin);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((int)0x00);
                        bw.Write((short)0x00);
                    }


                }

            }

        }

        /// <summary>
        /// For shuffling HUNK/A/B ROOM LAYOUTS and A/B BOSS FLIPPING
        /// </summary>
        /// <param name="rdt_file"></param>
        /// <param name="rdt_index"></param>
        public static void Shuffle_GTYPE(string rdt_file, int rdt_index)
        {
            Random r_Gtype = new Random();
            byte nop = 0x00;

            byte route_val = 0;

            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {


                    if (rdt_index == 0)
                    {

                        byte Gtype_Roll_0x00 = byte.Parse(r_Gtype.Next(0, 2).ToString());
                        fs.Seek(4585, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll_0x00);

                    }


                    if (rdt_index == 6)
                    {

                        byte Gtype_Roll_0x06 = (byte)r_Gtype.Next(0, 2);
                        fs.Seek(3461, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll_0x06);

                    }
                    // mr x random?
                    //if (rdt_index == 9)
                    //{
                    //    byte GType_Roll_0x09 = byte.Parse(r_Gtype.Next(0, 2).ToString());
                    //    fs.Seek(8471, SeekOrigin.Begin);
                    //    bw.Write(GType_Roll_0x09);
                    //}

                    if (rdt_index == 10)
                    {
                        byte Gtype_Roll_0x10 = byte.Parse(r_Gtype.Next(0, 2).ToString()); fs.Seek(6737, SeekOrigin.Begin); bw.Write(Gtype_Roll_0x10);

                        fs.Seek(6870, SeekOrigin.Begin);

                        var Patchray = new byte[10];
                        for (int i = 0; i < Patchray.Length; i++)
                        {
                            Patchray[i] = 0x00;
                        }

                        bw.Write(Patchray, 0, Patchray.Length);


                    }

                    // re write randomized game type bit/fix return
                    if (rdt_index == 15)
                    {

                        byte Gtype_Roll_0x15 = byte.Parse(r_Gtype.Next(0, 2).ToString());


                        fs.Seek(1465, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll_0x15); // write roll over check to keep itembox/type writer
                        bw.Write(Gtype_Roll_0x15); // write roll over return

                        fs.Seek(1769, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll_0x15); // write roll over check to keep item spawns

                        fs.Seek(+2, SeekOrigin.Current); // skip gosub 
                        bw.Write(Gtype_Roll_0x15);  // write roll over return


                        // seek to dupe opcodes and patch them out..
                        fs.Seek(1922, SeekOrigin.Begin);

                        var Patchray = new byte[101];
                        for (int i = 0; i < Patchray.Length; i++)
                        {
                            Patchray[i] = 0x00;
                        }

                        bw.Write(Patchray, 0, Patchray.Length);


                        // Console.WriteLine("test");


                    } // 2f Save room


                    if (rdt_index == 27)
                    {
                        byte RouteRoll = (byte)r_Gtype.Next(0, 2);
                        Int16 NX = 0;
                        Int16 NY = 0;
                        Int16 NZ = 0;
                        Int16 NR = 0;
                        byte RID = 0;
                        byte CID = 0;


                        if (RouteRoll == 0)
                        {

                            route_val = 0;
                            // Console.WriteLine("NORMAL ROUTE!");
                            NX = -24371;
                            NY = -5400;
                            NZ = -20441;
                            NR = -1024;

                            RID = 3;
                            CID = 0;


                            fs.Seek(2328, SeekOrigin.Begin);
                            bw.Write(NX);
                            bw.Write(NY);
                            bw.Write(NZ);
                            bw.Write(NR);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RID);
                            bw.Write(CID);

                            // write NY
                            // write NZ
                            // write NR
                            // write byte room ID
                            // write byte cam

                        }

                        // back route =]
                        if (RouteRoll == 1)
                        {


                            route_val = 1;

                            //   Console.WriteLine("REVERSE ROUTE!");

                            NX = 12210;
                            NY = 0;
                            NZ = -11899;
                            NR = 1120;

                            RID = 5;
                            CID = 0;


                            fs.Seek(2328, SeekOrigin.Begin);
                            bw.Write(NX);
                            bw.Write(NY);
                            bw.Write(NZ);
                            bw.Write(NR);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(RID);
                            bw.Write(CID);
                            bw.Write((byte)0x00);
                            // write NX
                            // write NY
                            // write NZ
                            // write NR
                            // write byte room ID
                            // write byte cam
                        }



                    }

                    // east office A/B enemy flip =]
                    if (rdt_index == 32)
                    {
                        byte gtype_Roll_34 = (byte)r_Gtype.Next(0, 2);
                        fs.Seek(7699, SeekOrigin.Begin);
                        bw.Write(gtype_Roll_34);


                    }

                    // COG ROOM A/B LICKER ;)
                    if (rdt_index == 43)
                    {
                        byte Gtype_Roll_43 = byte.Parse(r_Gtype.Next(0, 2).ToString());
                        byte mrXRoll = (byte)r_Gtype.Next(0, 2);

                        fs.Seek(4685, SeekOrigin.Begin);
                        bw.Write(mrXRoll);



                        fs.Seek(4909, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll_43);


                    }

                    if (rdt_index == 45) // yellow hall b4 basement
                    {
                        byte Gtype_Roll_0x45 = byte.Parse(r_Gtype.Next(0, 2).ToString());

                        fs.Seek(6041, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll_0x45);

                        if (Gtype_Roll_0x45 == 0)
                        {
                            fs.Seek(6118, SeekOrigin.Begin); bw.Write((byte)0x00);
                            fs.Seek(6042, SeekOrigin.Begin);
                            bw.Write(0x00);
                            bw.Write(0x00);
                            bw.Write((short)0x00);


                        }
                        else
                        {
                            fs.Seek(6118, SeekOrigin.Begin); bw.Write((byte)0x01);
                        }


                    }


                    // enable tofu parking lot
                    if (rdt_index == 52)
                    {
                        // byte Gtype_Roll_52 = (byte)r_Gtype.Next(0, 2);
                        fs.Seek(3975, SeekOrigin.Begin);
                        bw.Write((byte)0x00);


                        // enable item aot_scd (in corner)
                        fs.Seek(3980, SeekOrigin.Begin);
                        bw.Write((byte)0x18);
                        bw.Write((byte)0x0D);
                    }

                    // prison cell block, patch in hunk setup, + desk item
                    if (rdt_index == 53)
                    {

                        byte g_type_roll_53 = (byte)r_Gtype.Next(0, 2);
                        fs.Seek(2577, SeekOrigin.Begin);
                        bw.Write(g_type_roll_53);
                        fs.Seek(2710, SeekOrigin.Begin);
                        bw.Write((byte)0x00);
                        fs.Seek(2714, SeekOrigin.Begin);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x00);

                    }


                    // BLUE COKE HALLWAY
                    //if (rdt_index == 19)
                    //{
                    //    byte Gtype_Roll_0x19 = byte.Parse(r_Gtype.Next(0, 2).ToString());

                    //    byte enable = 0x01;

                    //    // fix difficulty issue..
                    //    fs.Seek(3675, SeekOrigin.Begin);
                    //    bw.Write(enable);

                    //    fs.Seek(3577, SeekOrigin.Begin);
                    //    bw.Write(Gtype_Roll_0x19);
                    //    fs.Seek(+2, SeekOrigin.Current);
                    //    bw.Write(Gtype_Roll_0x19);

                    //    if (Gtype_Roll_0x19 == 0) {

                    //        fs.Seek(3982, SeekOrigin.Begin);
                    //        bw.Write(nop);
                    //        bw.Write(nop);
                    //    }

                    //    if (Gtype_Roll_0x19 == 1)
                    //    {
                    //        byte sub_op = 0x18;
                    //        byte sub = 0x05;
                    //        fs.Seek(3982, SeekOrigin.Begin);
                    //        bw.Write(sub_op);
                    //        bw.Write(sub);


                    //    }

                    //}


                    // ?
                    if (rdt_index == 20)
                    {
                        byte Gtype_Roll_0x20 = byte.Parse(r_Gtype.Next(0, 2).ToString());
                        fs.Seek(2729, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll_0x20);

                    }

                    // chopper hall

                    // Randomize 1st BOSS (A/B) FLIP
                    if (rdt_index == 59)
                    {

                        byte EarlyBossRoll = byte.Parse(r_Gtype.Next(0, 2).ToString());

                        fs.Seek(6137, SeekOrigin.Begin);
                        bw.Write(EarlyBossRoll);


                        fs.Seek(6217, SeekOrigin.Begin);
                        bw.Write(EarlyBossRoll);


                        byte Gtype_Roll59 = byte.Parse(r_Gtype.Next(0, 2).ToString());


                        fs.Seek(6145, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll59);


                        fs.Seek(6233, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll59);

                        fs.Seek(7509, SeekOrigin.Begin);
                        bw.Write(Gtype_Roll59);

                    }


                    // ada segment A/B flip
                    if (rdt_index == 61) { byte Gtype_Roll_0x20 = byte.Parse(r_Gtype.Next(0, 2).ToString()); fs.Seek(6051, SeekOrigin.Begin); bw.Write(Gtype_Roll_0x20); }

                    // randomize train fight (A/B FLIP)
                    if (rdt_index == 95) { byte Gtype_Roll95 = byte.Parse(r_Gtype.Next(0, 2).ToString()); fs.Seek(7271, SeekOrigin.Begin); bw.Write(Gtype_Roll95); }


                 

                }

            }

        }

        // hard coded patching various things
        public static void BANDAID_FIX(string rdt_File, int rdt_index)
        {

            using (FileStream fs = new FileStream(rdt_File, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {


                    byte flag = 0x01;
                    byte nop = 0x00;
                    byte SubID = 0x00;

                    Random r_GoSub = new Random();

                    // lab train area...



                    Int16 TrainNX = 13248;
                    Int16 TrainNY = -1800;
                    Int16 TrainZ = -16378;
                    Int16 TrainR = -616;
                    byte TrainID = 0x04;
                    byte TrainCam = 0x07;
                    byte TrainFloor = 0x01;


                    //flipping ck values around because of flawed scanner..
                    switch (rdt_index)
                    {
                        case 0:
                            fs.Seek(4888, SeekOrigin.Begin);

                            bw.Write(0x00);
                            break;

                        case 2: // van item fix
                            fs.Seek(7837, SeekOrigin.Begin);
                            bw.Write(flag);
                            fs.Close();
                            break;

                        // brad patch
                        case 3:
                            fs.Seek(14159, SeekOrigin.Begin);
                            bw.Write((byte)0x01);
                            break;

                        // remove lock from the crow hall


                        // fix the cabin door to prevent that fucking event crash...
                        case 6:
                            fs.Seek(3006, SeekOrigin.Begin);
                            bw.Write((int)0x00);
                            bw.Write((int)0x00);
                            bw.Write((int)0x00);
                            bw.Write((int)0x00);
                            bw.Write((int)0x00);
                            bw.Write((int)0x00);
                            bw.Write((int)0x00);
                            bw.Write((int)0x00);
                            break;

                        case 9:
                            fs.Seek(8460, SeekOrigin.Begin);
                            bw.Write((int)0x00);
                            break;



                        //    // stars hall unlock spade key door
                        //case 20:
                        //    fs.Seek(2590, SeekOrigin.Begin);
                        //    bw.Write((int)0x00);
                        //    break;

                        //case 21:// stars room hg bullets
                        //    fs.Seek(5153, SeekOrigin.Begin);
                        //    bw.Write(flag);
                        //    fs.Close();
                        //    break;


                        // unlock outside stairs lock
                        case 22:
                            fs.Seek(2582, SeekOrigin.Begin);
                            bw.Write((int)0x00);
                            break;

                        case 25: // before bus, swap sub call instead of em_state
                            fs.Seek(3215, SeekOrigin.Begin);
                            SubID = byte.Parse(r_GoSub.Next(3, 5).ToString());
                            bw.Write(SubID);

                            Random R_pose = new Random();

                            byte crawl = byte.Parse(R_pose.Next(0, 5).ToString());


                            byte status = 0x00;

                            fs.Seek(3274, SeekOrigin.Begin);
                            bw.Write(0x01); // skip crash

                            fs.Seek(3596, SeekOrigin.Begin);
                            bw.Write(crawl);
                            //     bw.Write(status);
                            fs.Seek(3618, SeekOrigin.Begin);
                            bw.Write(crawl);
                            //   bw.Write(status);
                            fs.Seek(3640, SeekOrigin.Begin);
                            bw.Write(crawl);
                            //   bw.Write(status);

                            fs.Seek(3662, SeekOrigin.Begin);
                            bw.Write(crawl);
                            //  bw.Write(status);



                            break;

                        //case 30:
                        //    fs.Seek(9842, SeekOrigin.Begin);
                        //    bw.Write((byte)0x0FF);
                        //    break;


                        //case 45: // yellow hall sg rounds fix
                        //    fs.Seek(10429, SeekOrigin.Begin);
                        //    bw.Write(flag);
                        //    fs.Close();
                        //    break;


                        // night watch room locker aot reset fix
                        case 46:
                            fs.Seek(4942, SeekOrigin.Begin);
                            bw.Write(0);
                            bw.Write(0);
                            bw.Write(nop);
                            bw.Write(nop);

                            fs.Seek(5026, SeekOrigin.Begin);
                            bw.Write(0);
                            bw.Write(0);
                            bw.Write(nop);
                            bw.Write(nop);

                            fs.Seek(5045, SeekOrigin.Begin);
                            bw.Write((byte)0x00);

                            break;


                        case 79:
                            fs.Seek(13115, SeekOrigin.Begin);
                            bw.Write((byte)0x00);
                            fs.Seek(13201, SeekOrigin.Begin);
                            bw.Write((byte)0x00);

                            break;

                        //crocodile check, remove it
                        case 83:
                            fs.Seek(14372, SeekOrigin.Begin);
                            bw.Write((byte)0x00);
                            bw.Write((byte)0x00);
                            bw.Write((byte)0x00);
                            bw.Write((byte)0x00);
                            break;


                        //sg parts fix
                        case 88:
                            fs.Seek(5505, SeekOrigin.Begin);
                            bw.Write(nop);
                            break;


                        // lab warp..
                        case 102:
                            fs.Seek(3608, SeekOrigin.Begin);
                            bw.Write(TrainNX);
                            bw.Write(TrainNY);
                            bw.Write(TrainZ);
                            bw.Write(TrainR);
                            bw.Write((byte)0x04);
                            bw.Write((byte)0x04);
                            bw.Write((byte)0x07);
                            bw.Write((byte)0x01);
                            bw.Write((byte)0x07);


                            //fs.Seek(+1, SeekOrigin.Begin);
                            //bw.Write(TrainID);
                            //bw.Write(TrainCam);
                            //bw.Write(TrainFloor);
                            break;


                        case 106: // flame thrower room
                            fs.Seek(5390, SeekOrigin.Begin);

                            bw.Write(0);
                            bw.Write(0);
                            bw.Write(nop);
                            bw.Write(nop);

                            fs.Close();
                            break;


                        // open the lab
                        case 108:
                            fs.Seek(4041, SeekOrigin.Begin);
                            bw.Write((byte)0x01);
                            break;

                        // patch condition in lab so no matter what the random item is obtainable (kendo dead check instead of item check)
                        case 111:
                            fs.Seek(1941, SeekOrigin.Begin);
                            bw.Write((byte)0x04);
                            bw.Write((byte)0x1B);
                            bw.Write((byte)0x01);
                            break;

                        // enable A/B seamless ending
                        case 120:
                            fs.Seek(6435, SeekOrigin.Begin);
                            bw.Write((byte)0x00); // remove loss of player input


                            bw.Write((byte)0x01); // place return to skip end game

                            fs.Seek(5755, SeekOrigin.Begin);
                            bw.Write((byte)0x00);

                            fs.Seek(5759, SeekOrigin.Begin);
                            bw.Write((byte)0x00);
                            break;

                    }
                }

            }

        }


        /// <summary>
        /// Shuffling Common Items only, Weapons/Ammo (-co argument)
        /// </summary>
        /// <param name="rdt_file"></param>
        /// <param name="rdt_index"></param>
        /// <param name="rdt_item_count"></param>
        /// <param name="t_count"></param>
        /// <param name="RDT_DATA"></param>
        /// <param name="AllItems"></param>
        public static void Shuffle_Common(string rdt_file, int rdt_index, int rdt_item_count, ref int t_count, LIB_RDT.RDT_OBJ[] RDT_DATA, List<LIB_RDT.ITEM_DATA_OBJ> AllItems)
        {
            Random r_Item = new Random();
            Random r_Amount = new Random();
            Random r_Glimmer = new Random();

            bool Kendo_shotgun = false;


            // 0 variants = fast no anim, 1 variants = kneel
            byte[] GLIMMER_SET = new byte[] { 0xA0, 0xB0, 0xC0, 0xD0, 0xE0, 0xF0, 0xA1, 0xB1, 0xC1, 0xD1, 0xE1, 0xF1 };


            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {

                using (BinaryWriter bw = new BinaryWriter(fs))

                {
                    Console.ForegroundColor = ConsoleColor.Blue;

                    // Console.WriteLine(RDT_ITEM_DATA[rdt_index].f_rdt.Substring(RDT_ITEM_DATA[rdt_index].f_rdt.Length - 12, 8));
                    // Console.WriteLine("------------------------------------------------------");

                    Console.ForegroundColor = ConsoleColor.Green;


                    for (int x = 0; x < rdt_item_count; x++) // should be the same as # of offsets per file
                    {
                        byte Glimmer_Roll = byte.Parse(r_Glimmer.Next(0, GLIMMER_SET.Length).ToString()); // new roll per item..

                        if (LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                        {

                            //if (RDT_DATA[rdt_index].ITEM_AOTS[x].item >= 0x00 && RDT_DATA[rdt_index].ITEM_AOTS[x].item <= 0x2E || RDT_DATA[rdt_index].ITEM_AOTS[x].item == 0x50)
                            //{

                            if (LIB_ITEM.BIO2_COMMON_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                            {

                                t_count++; // create ur own indexer for every valid item

                                if (t_count != AllItems.Count)
                                {

                                    //if (AllItems[t_count].item == 0x0F)
                                    //{

                                    //}

                                    RDT_DATA[rdt_index].ITEM_AOTS[x] = AllItems[t_count]; // assign to shuffled index

                                    // if its between weapons and ammo randomize quantity..
                                    if (RDT_DATA[rdt_index].ITEM_AOTS[x].item >= 0x00 && RDT_DATA[rdt_index].ITEM_AOTS[x].item <= 0x1F)
                                    {
                                        int r_quantity = r_Amount.Next(3, LIB_ITEM.BIO2_LUT_QUANTITY[RDT_DATA[rdt_index].ITEM_AOTS[x].item]);
                                        //RDT_DATA[rdt_index].ITEM_AOTS[x].amount = short.Parse(r_quantity.ToString());
                                        RDT_DATA[rdt_index].ITEM_AOTS[x].amount = (byte)r_quantity;
                                    }

                                }

                                RDT_DATA[rdt_index].ITEM_AOTS[x].ani = GLIMMER_SET[Glimmer_Roll]; // randomly assign a glimmer color
                                RDT_DATA[rdt_index].ITEM_AOTS[x].md1 = 0xFF;


                                //    Console.WriteLine(RDT_ITEM_DATA[rdt_index].List_Aot_Offs[x]);

                                // WEAPON CHECK just debug.. stuff
                                if (RDT_DATA[rdt_index].ITEM_AOTS[x].item >= 0x01 && RDT_DATA[rdt_index].ITEM_AOTS[x].item <= 0x13)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                }
                                else if (LIB_ITEM.BIO2_KEY_LUT_LA.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                }


                                //if (LIB_ITEM.BIO2_ITEM_LUT.ContainsKey(RDT_DATA[rdt_index].ITEM_AOTS[x].item))
                                //{
                                //     Console.WriteLine(t_count + "] " + LIB_ITEM.BIO2_ITEM_LUT[AllItems[t_count].item] + AllItems[t_count].amount);
                                //    Console.WriteLine(t_count + "]" + LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")");
                                //}

                                Console.WriteLine(rdt_file + ") " + t_count + "]" + LIB_ITEM.BIO2_ITEM_LUT[RDT_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_DATA[rdt_index].ITEM_AOTS[x].amount + ")");

                                //Console.WriteLine(rdt_index + "]\\" + LIB_ITEM.BIO2_ITEM_LUT[RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].item] + "Q\\ " + RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].amount + " [" + RDT_ITEM_DATA[rdt_index].List_Aot_Offs[x] + "]");

                                //  Console.WriteLine(x + LIB_ITEM.BIO2_ITEM_LUT[RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].item] + " (" + RDT_ITEM_DATA[rdt_index].ITEM_AOTS[x].amount + ")");
                            }

                        }
                        // seek to item in file
                        //Console.WriteLine("Seeked struct off " + LIB_ITEM.BIO2_ITEM_LUT[Item_AOT[i].item]);


                        // fix this nigga kendos shotgun.. stupid aot_Reset..
                        if (rdt_index == 0x01)
                        {
                            // int tx = rdt_index;
                            fs.Seek(6211, SeekOrigin.Begin);
                            bw.Write(0x00);

                        }

                        // sg parts fix
                        if (rdt_index == 88) { byte box_fix = 0x10; RDT_DATA[rdt_index].ITEM_AOTS[x].flag = 0x15; RDT_DATA[rdt_index].ITEM_AOTS[x].ani = 0x01; fs.Seek(5540, SeekOrigin.Begin); bw.Write(box_fix); }

                        // seek to each item id
                        fs.Seek(RDT_DATA[rdt_index].List_Aot_Offs[x] + 14, SeekOrigin.Begin);


                        // re write newly assigned
                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].item);
                        fs.Seek(+1, SeekOrigin.Current);
                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].amount); //  rewrite quantity

                        //  fs.Seek(+1, SeekOrigin.Begin);
                        // bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].flag);
                        //  bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].md1);

                        // skip md1 write
                        fs.Seek(+3, SeekOrigin.Current);

                        bw.Write(RDT_DATA[rdt_index].ITEM_AOTS[x].ani);

                    }

                }


            }


            //for (int i = 0; i < CommonList.Count; i++)
            //{
            //    Console.WriteLine(i + "]" + LIB_ITEM.BIO2_ITEM_LUT[CommonList[i].item]);
            //}

        }


        ///// http://re123.bplaced.net/board/viewtopic.php?f=11&t=35 //

        /// <summary>
        /// Shuffle Enemy Data
        /// </summary>
        /// <param name="rdt_file"></param>
        /// <param name="rdt_index"></param>
        /// <param name="emd_count"></param>
        /// <param name="t_count"></param>
        /// <param name="RDT_DATA"></param>
        public static void EMD_SHUFFLE(string rdt_file, int rdt_index, int emd_count, ref int t_count, LIB_RDT.RDT_OBJ[] RDT_DATA)
        {

            Random r_EMD = new Random();
            Random r_Pose = new Random();


            // 0x48 = static munch, causing problems..
            byte[] ZOMBIE_IDS = new byte[] { 0x10, 0x11, 0x12, 0x15, 0x16, 0x13, 0x17 }; // zombie types
                                                                                         // byte[] ZOMBIE_IDS = new byte[] {0x10,0x11, 0x15, 0x16, 0x17, 0x1E, 0x1F}; // zombie types
            byte[] POSE_IDS = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x08, 0x09, 0x43, 0x86, 0xC2, 0xC3, 0xC6 };


            byte[] LICKER_IDS = new byte[] { 0x22, 0x24 };
            //   byte LICKER_POSES = new byte[] {0x16, 0x18},



            byte emd_roll = byte.Parse(r_EMD.Next(0, ZOMBIE_IDS.Length).ToString());


            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {

                    for (int i = 0; i < emd_count; i++) // emd count will be rdt relative
                    {
                        // roll random emd id
                        byte emd_state = (byte)r_Pose.Next(0, POSE_IDS.Length); // roll random pose/state
                        byte emd_status = 64;

                        // exclude ada
                        if (RDT_DATA[rdt_index].EM_DATA[i]._emdID != 0x41 || RDT_DATA[rdt_index].EM_DATA[i]._emdID != 0x43)
                        {
                            RDT_DATA[rdt_index].EM_DATA[i]._emdID = ZOMBIE_IDS[emd_roll]; // assign new rolls
                            RDT_DATA[rdt_index].EM_DATA[i]._emPose = POSE_IDS[emd_state]; // assign new rolls

                            fs.Seek(RDT_DATA[rdt_index].LIST_EMD_OFFS[i] + 3, SeekOrigin.Begin); // change this 3/4 depending on emd_write
                            bw.Write(RDT_DATA[rdt_index].EM_DATA[i]._emdID);
                            bw.Write(RDT_DATA[rdt_index].EM_DATA[i]._emPose);
                        }
                        //  bw.Write(emd_status);

                    }

                }

            }

        }

        public static void EMD_SWAP(string rdt_File, int rdt_index, int emd_count, ref int t_count, LIB_RDT.RDT_OBJ[] RDT_DATA, LIB_EMD.EMD_OUT_OBJ[] EMD_OUT)
        {


            // Console.WriteLine(rdt_index + "] " + rdt_File);

            // zombies
            byte[] POSE_IDS = new byte[] { 0x01, 0x02, 0x03, 0x08, 0x09, 0x43, 0x86, 0xC3 }; 
          
             // safer enemy sets
            byte[] Safe_Rolls = new byte[] { 2, 3, 6, 7, 8, 9, 10, 12 };

            byte[] SafeIsh_Rolls = new byte[] { 0, 1, 2, 3, 6, 7, 8, 9, 10, 12 }; // no mr x

            byte[] Zombie_Tex_Rolls = new byte[] { 0, 1, 2, 3 };
            
            // create a new enemy object
            LIB_RDT.EM_SET_OBJ EM_SET = new LIB_RDT.EM_SET_OBJ();


            LIB_RDT.SUPER_EM_SET_OBJ x8E_SET = new LIB_RDT.SUPER_EM_SET_OBJ();

            Random Em_Roll = new Random();

            byte roll = (byte)Em_Roll.Next(0, 14);
            byte safe_role = (byte)Em_Roll.Next(0, Safe_Rolls.Length);
            byte safeIsh_roll = (byte)Em_Roll.Next(0, SafeIsh_Rolls.Length);
            byte pose_roll = 0;
            byte tex_roll = 0;

            //   Console.WriteLine("Roll is : "+ roll);

            using (FileStream fs = new FileStream(rdt_File, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    for (int i = 0; i < emd_count; i++) // emd count will be rdt relative
                    {
                        // roll random emd id

                        //RDT_DATA[rdt_index].EM_DATA[i]._emdID = EM_SET._emdID; // assign new rolls
                        // RDT_DATA[rdt_index].EM_DATA[i]._emPose = EM_SET._emPose; // assign new rolls


                        if (rdt_index == 94) { if (i > 7) { break; } } // see if this works
                        if (rdt_index == 88) { if (i > 4) { break; } } // break out of the loops on ada hallway? to prevent her emd from being touched?


                        // roll a new pose roll and texture roll for every emd code for zombies
                        pose_roll = (byte)Em_Roll.Next(0, POSE_IDS.Length);

                        tex_roll = (byte)Em_Roll.Next(0, Zombie_Tex_Rolls.Length);



                        // if bus,dumpster, or bball court, force roll to be one of the safe rolls
                        if (rdt_index == 2 || rdt_index == 24 || rdt_index == 26) { roll = Safe_Rolls[safe_role]; }


                        /// dont use zombies in streets/bus room anymore..
                        if (rdt_index == 25)
                        {
                            if (roll == 7) { roll -= 1; }
                            if (roll == 8) { roll += 2; }
                            if (roll == 9) { roll -= 4; }
                        }

                        // evidence room / red hallways , dont roll mr x
                        if (rdt_index == 39) { roll = SafeIsh_Rolls[safeIsh_roll]; }

                        // rdt_index == 42 // red hall if u want

                        // lickers
                        if (roll == 0)
                        {
                            EM_SET._emdID = 0x22; EM_SET._emPose = 0; EM_SET._TEX = 0; EM_SET._SND = 0x0E; EM_SET._AnimFlag00 = 144;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "LICKER";

                            //Console.WriteLine("RED LICKER SET IN " + rdt_File);
                        }
                        // licker gray
                        if (roll == 1)
                        {
                            EM_SET._emdID = 0x24; EM_SET._emPose = 0; EM_SET._TEX = 0; EM_SET._SND = 0x33; EM_SET._AnimFlag00 = 144;

                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "SUPER LICKER";
                        }

                        // spiders
                        if (roll == 2)
                        {
                            EM_SET._emdID = 0x25; EM_SET._emPose = 0; EM_SET._TEX = 131; EM_SET._SND = 0x10;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "SPIDER";
                        }
                        // dogs
                        if (roll == 3)
                        {
                            EM_SET._emdID = 0x20; EM_SET._emPose = 1; EM_SET._TEX = 0; EM_SET._SND = 0x0C;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "DOG";
                        }
                        // green ivys
                        if (roll == 4)
                        {
                            EM_SET._emdID = 0x2E; EM_SET._emPose = 1; EM_SET._SND = 0x13;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "GREEN IVY";
                        }
                        // purple ivys
                        if (roll == 5)
                        {
                            EM_SET._emdID = 0x39; EM_SET._emPose = 17; EM_SET._TEX = 0; EM_SET._SND = 0x13;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "PURPLE IVY";
                        }

                        // hands...
                        if (roll == 6)
                        {
                            EM_SET._emdID = 0x2D; EM_SET._emPose = 0; EM_SET._TEX = 0; EM_SET._SND = 0x11;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "JAZZ HANDS";
                        }


                        // g-mutant
                        //   

                        //  if (roll == 5) { EM_SET._emdID = 0x3B; EM_SET._emPose = 2; EM_SET._SND = 0x28; EM_SET._AnimFlag00 = 255; }


                        // zombies (zombies have pose roll)
                        if (roll == 7)
                        {
                            EM_SET._emdID = 0x1F; EM_SET._emPose = POSE_IDS[pose_roll]; EM_SET._TEX = Zombie_Tex_Rolls[tex_roll]; EM_SET._SND = 0x02;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "ZOMBIE";
                        }
                        // zombies
                        if (roll == 8)
                        {
                            EM_SET._emdID = 0x15; EM_SET._emPose = POSE_IDS[pose_roll]; EM_SET._TEX = 128; EM_SET._SND = 0x2F;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "ZOMBIE";
                        }
                        //zombies
                        if (roll == 9)
                        {
                            EM_SET._emdID = 0x17; EM_SET._emPose = POSE_IDS[pose_roll]; EM_SET._TEX = 0; EM_SET._SND = 0x2E;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "ZOMBIE";
                        }

                        //  if (roll == 10) { EM_SET._emdID = 0x23; EM_SET._emPose = 0; EM_SET._SND = 0x16; EM_SET._emFlag = 1; }

                        // moth boys
                        if (roll == 10)
                        {
                            EM_SET._emdID = 0x3A; EM_SET._emPose = 0; EM_SET._TEX = 0; EM_SET._SND = 0x17;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "MOTH";
                        }

                        // mr sex
                        if (roll == 11)
                        {
                            EM_SET._emdID = 0x2A; EM_SET._emPose = 67; EM_SET._TEX = 0; EM_SET._SND = 0x12;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "MR X";
                        }

                        //   if (roll == 11) { EM_SET._emdID = 0x44; EM_SET._emPose = 0; EM_SET._TEX = 0; EM_SET._SND = 0x02; }


                        //   if (roll == 13) { EM_SET._emdID = 0x30; EM_SET._emPose = 01; EM_SET._TEX = 0; EM_SET._SND = 0x1C; EM_SET._emFlag = 198; }

                        //crows
                        if (roll == 12)
                        {
                            EM_SET._emdID = 0x21; EM_SET._emPose = 64; EM_SET._TEX = 0; EM_SET._SND = 0x0D;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "CROW";
                        }

                        // brads

                        if (roll == 13)
                        {
                            EM_SET._emdID = 0x11; 
                            EM_SET._emPose = POSE_IDS[pose_roll]; 
                            EM_SET._TEX = 0x00; 
                            EM_SET._SND = 0x02;
                            EMD_OUT[rdt_index].fname = rdt_File;
                            if (LIB_ROOM.LUT_ROOM.ContainsKey(rdt_File.Substring(rdt_File.Length - 8, 4)))
                            {
                                EMD_OUT[rdt_index].roomname = LIB_ROOM.LUT_ROOM[rdt_File.Substring(rdt_File.Length - 8, 4)];
                            }
                            EMD_OUT[rdt_index].enemy_name = "BRAD";
                        }



                        // ben?
                        //if (roll == 14) { EM_SET._emdID = 0x44; EM_SET._emPose = 0; EM_SET._TEX = 0; EM_SET._SND = 0x02; }


                        // g-mutant
                        //gbaby'S DONT RENDER
                        // if (roll == 14) { EM_SET._emdID = 0x27; EM_SET._emPose = 0; EM_SET._TEX = 0; EM_SET._SND = 0x14; EM_SET._AnimFlag00 = 99; }
                        //   if (roll == 14) { EM_SET._emdID = 0x30; EM_SET._emPose = 0; EM_SET._TEX = 0; EM_SET._SND = 0x1C; EM_SET._AnimFlag00 = 98; }

                        // g-mutnts
                        //     if (roll == 15) { EM_SET._emdID = 0x28; EM_SET._emPose = 1; EM_SET._TEX = 0; EM_SET._SND = 0x15; EM_SET._AnimFlag00 = 98; }





                        // no arms in marvin room (West office)
                        if (rdt_index == 32 && roll == 6)
                        {
                            roll = (byte)Em_Roll.Next(8, 11);
                        }

                        if (rdt_index == 32 && roll == 7)
                        {
                            roll = (byte)Em_Roll.Next(8, 11);
                        }

                        // no arms in ada segments
                        if (rdt_index == 88 && roll == 6)
                        {
                            roll = (byte)Em_Roll.Next(7, 11);
                        }
                        // no arms in ada segments
                        if (rdt_index == 94 && roll == 6)
                        {
                            roll = (byte)Em_Roll.Next(7, 11);
                        }

                        //
                        if (rdt_index == 25 && roll == 6)
                        {
                            roll = (byte)Em_Roll.Next(7, 11);
                        }

                        /// handle super em set
                        if (roll == 15) {
                            x8E_SET._emdID = 0x34;
                            
                        }

                        // skip opcode and jump to newly ID/rolled pose..
                        fs.Seek(RDT_DATA[rdt_index].LIST_EMD_OFFS[i] + 3, SeekOrigin.Begin);
                        bw.Write(EM_SET._emdID);
                        bw.Write(EM_SET._emPose);
                        

                        // if before bus, or ada halls or lab hall
                        if (rdt_index == 25 || rdt_index == 88 || rdt_index == 107)
                        {
                            // write a 00 status? (fix dead guys?)
                            bw.Write((byte)0x00);
                            fs.Seek(+1, SeekOrigin.Current);
                            bw.Write(EM_SET._SND);
                            bw.Write(EM_SET._TEX);
                        }
                        else
                        {
                            // otherwise skip write to SND/TEX?
                            fs.Seek(+2, SeekOrigin.Current);
                            bw.Write(EM_SET._SND);
                            bw.Write(EM_SET._TEX);
                        } //  fs.Seek(+1, SeekOrigin.Current);
                          // bw.Write(EM_SET._AnimFlag00);

                        //  bw.Write(emd_status);



                        // if (rdt_index == 88) { fs.Seek(5659, SeekOrigin.Begin); bw.Write((byte)0x00); fs.Seek(5685, SeekOrigin.Begin); bw.Write((byte)0x00); }
                        // if (rdt_index == 94) { fs.Seek(5693, SeekOrigin.Begin); bw.Write((byte)0x00); fs.Seek(5719, SeekOrigin.Begin); bw.Write((byte)0x00); }


                    }

                }

            }

        }



        /// <summary>
        /// Mirrors randomized data in 2 different rooms within ada cutscene
        /// </summary>
        /// <param name="rdt_file"></param>
        /// <param name="roomIdx00"></param>
        /// <param name="roomIdx01"></param>
        /// <param name="RDT_DATA"></param>
        public static void ADA_THROW_PATCH(string rdt_file, int roomIdx00, int roomIdx01, LIB_RDT.RDT_OBJ[] RDT_DATA)
        {

            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {

                using (BinaryWriter bw = new BinaryWriter(fs))
                {

                    fs.Seek(3436, SeekOrigin.Begin);
                    bw.Write(RDT_DATA[roomIdx00].ITEM_AOTS[0].item);
                    fs.Seek(+1, SeekOrigin.Current);
                    bw.Write(RDT_DATA[roomIdx00].ITEM_AOTS[0].amount);

                    fs.Seek(3544, SeekOrigin.Begin);
                    bw.Write(RDT_DATA[roomIdx01].ITEM_AOTS[0].item);
                    fs.Seek(+1, SeekOrigin.Current);
                    bw.Write(RDT_DATA[roomIdx01].ITEM_AOTS[0].amount);


                    fs.Seek(6420, SeekOrigin.Begin);
                    bw.Write(RDT_DATA[roomIdx00].ITEM_AOTS[0].item);
                    fs.Seek(+1, SeekOrigin.Current);
                    bw.Write(RDT_DATA[roomIdx00].ITEM_AOTS[0].amount);

                    fs.Seek(6434, SeekOrigin.Begin);
                    bw.Write(RDT_DATA[roomIdx00].ITEM_AOTS[0].item);
                    fs.Seek(+1, SeekOrigin.Current);
                    bw.Write(RDT_DATA[roomIdx00].ITEM_AOTS[0].amount);


                    fs.Seek(6616, SeekOrigin.Begin);
                    bw.Write(RDT_DATA[roomIdx01].ITEM_AOTS[0].item);
                    fs.Seek(+1, SeekOrigin.Current);
                    bw.Write(RDT_DATA[roomIdx01].ITEM_AOTS[0].amount);




                }

            }
        }

        // Possible solution for swapping Door lock codes ;)
        public static void DOOR_AOT_SWAP(string rdt_file, int rdt_index, ref byte LockFlag)
        {

            Random r_DoorID = new Random();
            Random r_LockState = new Random();
            Random r_KeySet = new Random();

            //byte lockFlag_Spade = 0x89;


            // spade, diamond, heart,club
            byte[] KeyFlags = new byte[] { 0x59, 0x5A, 0x5B, 0x5C };


            int SetKey = 0;
            byte KeyFlag = 0;

            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {

                    // if rdt index is one of the regular lock rooms..
                    if (LIB_DOOR.ROOM_LUT.ContainsKey(rdt_index))
                    {

                        // if east office roll between all except club
                        if (rdt_index == 41) { SetKey = r_KeySet.Next(0, 3); }

                        // if 1f stairs roll between spade/diamond
                        if (rdt_index == 37) { SetKey = r_KeySet.Next(0, 2); }

                        if (rdt_index == 20 || rdt_index == 34 || rdt_index == 42 || rdt_index == 45 || rdt_index == 47)
                        {
                            SetKey = r_KeySet.Next(0, 4);
                        }


                        // and its not east office or 1f stairs/evidence
                        //  if (rdt_index != 41 || rdt_index != 37) { SetKey = r_KeySet.Next(0, 4); }

                        if (rdt_index != 42) // if not red hall, (other key rooms)
                        {

                            KeyFlag = KeyFlags[SetKey];
                            LockFlag += 1; // incrementing lock flag everytime we hit a valid room

                            fs.Seek(LIB_DOOR.HSDC_LOCKS[rdt_index], SeekOrigin.Begin);
                            bw.Write((byte)LockFlag);
                            bw.Write((byte)KeyFlag);
                            Console.WriteLine("LF: " + LockFlag + "] " + LIB_DOOR.ROOM_LUT[rdt_index] + "\\ " + LIB_DOOR.LOCK_LUT[KeyFlag]);

                        }
                        else if (rdt_index == 42)// red hall has 2 locked doors.. so using offset dic doesent really work
                        {
                            // re roll keys for each door, hopefully lock flag is incremented correctly
                            SetKey = r_KeySet.Next(0, 4);
                            KeyFlag = KeyFlags[SetKey];
                            fs.Seek(7143, SeekOrigin.Begin);
                            bw.Write((byte)LockFlag);
                            bw.Write((byte)KeyFlag);

                            Console.WriteLine("LF: " + LockFlag + "]" + LIB_DOOR.ROOM_LUT[rdt_index] + "Conference Room \\ " + LIB_DOOR.LOCK_LUT[KeyFlag]);

                            LockFlag += 1;

                            SetKey = r_KeySet.Next(0, 4);
                            KeyFlag = KeyFlags[SetKey];
                            fs.Seek(7175, SeekOrigin.Begin);
                            bw.Write((byte)LockFlag);
                            bw.Write((byte)KeyFlag);
                            Console.WriteLine("LF: " + LockFlag + "]" + LIB_DOOR.ROOM_LUT[rdt_index] + "InteroGation Room \\" + LIB_DOOR.LOCK_LUT[KeyFlag]);
                        }

                    }


                    //set trial bball court
                    //if (rdt_index == 0x00)
                    //{
                    //    byte new_door = byte.Parse(r_DoorID.Next(0, 2).ToString());

                    //    switch (new_door)
                    //    {
                    //        case 0: new_door = 0x02; break;
                    //        case 1: new_door = 0x1d; break;

                    //    }

                    //    fs.Seek(4269, SeekOrigin.Begin);
                    //    bw.Write(new_door);
                    //}

                    //  update spade key use quan based on rolls
                    //if (rdt_index == 30)
                    //{
                    //    fs.Seek(9842, Seekorigin.Begin);
                    //    bwWrite((byte));
                    //}


                }

            }

        }


        public static void CUTSCENE_EM_SWAP(string rdt_file, int rdt_index)
        {

            /* 0x42 = IRONS
             * 0x43 = ADA
             * 0x44 = BEN
             * 0x45 = SHERRY
             * 0x48 = KENDO
             * 
             */

            byte[] KENDO_SWAPS = new byte[] { 0x42, 0x43, 0x44, 0x48, 0x45 };
            //  byte[] MODEL_SWAPS = new byte[] { 0x43, 0x44, 0x45, 0x48 };
            byte[] MODEL_SWAPS = new byte[] { 0x3E, 0x3E, 0x3E, 0x3E };
            // 42 irons 43 ada, 48, kendo
            Random M_Swap = new Random(); // random 



            int K_Roll = M_Swap.Next(0, KENDO_SWAPS.Length); // kendo shop roll

            int m_Roll = M_Swap.Next(0, MODEL_SWAPS.Length);


            // if rdt index is kendo shop, stars room, end of game?
            if (rdt_index == 1 || rdt_index == 21 || rdt_index == 103 || rdt_index == 123)
            {
                using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        switch (rdt_index)
                        {
                            // kendo
                            case 1:
                                fs.Seek(6807, SeekOrigin.Begin);
                                bw.Write((byte)KENDO_SWAPS[K_Roll]);

                                fs.Seek(6999, SeekOrigin.Begin);
                                bw.Write((byte)KENDO_SWAPS[K_Roll]);

                                break;

                            case 21:

                                m_Roll = M_Swap.Next(0, MODEL_SWAPS.Length); // For stars room

                                if (m_Roll == 0) { m_Roll += 1; }

                                fs.Seek(5499, SeekOrigin.Begin);
                                bw.Write((byte)MODEL_SWAPS[m_Roll]);

                                fs.Seek(5691, SeekOrigin.Begin);
                                bw.Write((byte)MODEL_SWAPS[m_Roll]);
                                break;

                            case 103:
                                m_Roll = M_Swap.Next(0, MODEL_SWAPS.Length); // For end game B train

                                fs.Seek(3299, SeekOrigin.Begin);
                                bw.Write((byte)MODEL_SWAPS[m_Roll]);
                                break;

                            case 123:

                                m_Roll = M_Swap.Next(0, MODEL_SWAPS.Length); // For end game B train

                                fs.Seek(7939, SeekOrigin.Begin);
                                bw.Write((byte)MODEL_SWAPS[m_Roll]);

                                fs.Seek(7991, SeekOrigin.Begin);
                                bw.Write((byte)MODEL_SWAPS[m_Roll]);

                                fs.Seek(8049, SeekOrigin.Begin);
                                bw.Write((byte)MODEL_SWAPS[m_Roll]);
                                break;

                        }


                    }

                }
            }
        }


        // for
        public static void Torch_Shuffle(string rdt_file)
        {

            Random r_combo = new Random();

            LIB_RDT.TORCH_COMBO_OBJ[] TORCH_OBJS = new LIB_RDT.TORCH_COMBO_OBJ[6];


            for (int i = 0; i < TORCH_OBJS.Length; i++)
            {
                Array.Resize(ref TORCH_OBJS[i].AOT_X, 3);
                Array.Resize(ref TORCH_OBJS[i].ESPR_X, 3);

            }

            // sub index = torch
            // MRL
            TORCH_OBJS[0].AOT_X[0] = -22000;
            TORCH_OBJS[0].AOT_X[1] = -18300;
            TORCH_OBJS[0].AOT_X[2] = -25600;

            TORCH_OBJS[0].ESPR_X[0] = -21600;
            TORCH_OBJS[0].ESPR_X[1] = -17950;
            TORCH_OBJS[0].ESPR_X[2] = -25200;


            // MLR
            TORCH_OBJS[1].AOT_X[0] = -22000;
            TORCH_OBJS[1].AOT_X[1] = -25600;
            TORCH_OBJS[1].AOT_X[2] = -18300;

            TORCH_OBJS[1].ESPR_X[0] = -21600;
            TORCH_OBJS[1].ESPR_X[1] = -25200;
            TORCH_OBJS[1].ESPR_X[2] = -17950;



            // RML
            TORCH_OBJS[2].AOT_X[0] = -18300;
            TORCH_OBJS[2].AOT_X[1] = -22000;
            TORCH_OBJS[2].AOT_X[2] = -25600;

            TORCH_OBJS[2].ESPR_X[0] = -17950;
            TORCH_OBJS[2].ESPR_X[1] = -21600;
            TORCH_OBJS[2].ESPR_X[2] = -25200;


            //RLM
            TORCH_OBJS[3].AOT_X[0] = -18300;
            TORCH_OBJS[3].AOT_X[1] = -25600;
            TORCH_OBJS[3].AOT_X[2] = -22000;

            TORCH_OBJS[3].ESPR_X[0] = -17950;
            TORCH_OBJS[3].ESPR_X[1] = -25200;
            TORCH_OBJS[3].ESPR_X[2] = -21600;



            //LRM
            TORCH_OBJS[4].AOT_X[0] = -25600;
            TORCH_OBJS[4].AOT_X[1] = -18300;
            TORCH_OBJS[4].AOT_X[2] = -22000;

            TORCH_OBJS[4].ESPR_X[0] = -25200;
            TORCH_OBJS[4].ESPR_X[1] = -17950;
            TORCH_OBJS[4].ESPR_X[2] = -21600;

            //LMR
            TORCH_OBJS[5].AOT_X[0] = -25600;
            TORCH_OBJS[5].AOT_X[1] = -22000;
            TORCH_OBJS[5].AOT_X[2] = -18300;

            TORCH_OBJS[5].ESPR_X[0] = -25200;
            TORCH_OBJS[5].ESPR_X[1] = -21600;
            TORCH_OBJS[5].ESPR_X[2] = -17950;





            // MRL





            int combo_seed = r_combo.Next(1, TORCH_OBJS.Length);




            using (var fs = new FileStream(rdt_file, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    // write aot x
                    fs.Seek(4564, SeekOrigin.Begin);
                    bw.Write(TORCH_OBJS[combo_seed].AOT_X[0]);


                    fs.Seek(4584, SeekOrigin.Begin);
                    bw.Write(TORCH_OBJS[combo_seed].AOT_X[1]);
                    fs.Seek(4604, SeekOrigin.Begin);
                    bw.Write(TORCH_OBJS[combo_seed].AOT_X[2]);


                    //write espr x

                    //fs.Seek(5518, SeekOrigin.Begin);
                    //bw.Write((byte)0x04); // new effect

                    //fs.Seek(5522, SeekOrigin.Begin);
                    //bw.Write((Int16)32150); // scale

                    fs.Seek(5524, SeekOrigin.Begin);
                    bw.Write(TORCH_OBJS[combo_seed].ESPR_X[0]); // x 



                    //fs.Seek(5704, SeekOrigin.Begin);
                    //bw.Write((byte)0x04); // new effect

                    //fs.Seek(5708, SeekOrigin.Begin);
                    //bw.Write((Int16)32150); // scale

                    fs.Seek(5710, SeekOrigin.Begin);
                    bw.Write(TORCH_OBJS[combo_seed].ESPR_X[1]); // x


                    //fs.Seek(5890, SeekOrigin.Begin);
                    //bw.Write((byte)0x04); // effect

                    //fs.Seek(5894, SeekOrigin.Begin);
                    //bw.Write((Int16)32150); // scale

                    fs.Seek(5896, SeekOrigin.Begin);
                    bw.Write(TORCH_OBJS[combo_seed].ESPR_X[2]); // x





                }

            }


        }


        /// <summary>
        /// Shuffle The Locker codes in the west office...
        /// </summary>
        /// <param name="rdt_file"></param>
        /// <param name="rdt_index"></param>
        /// <param name="op"></param>
        /// <param name="array"></param>
        /// <param name="num"></param>
        /// <param name="SafeArray"></param>
        public static void Safe_Shuffle(string rdt_file, int rdt_index)
        {

            Random r_combo = new Random();

            // 2236, 3622, 2362, 3226, 3262, 6322, 6232



            LIB_RDT.SAFE_COMBO_OBJ[] SAFE_COMBOS = new LIB_RDT.SAFE_COMBO_OBJ[12];

            for (int i = 0; i < SAFE_COMBOS.Length; i++)
            {
                Array.Resize(ref SAFE_COMBOS[i].SafeArray, 12); // resize each combos byte array to hold 1-4 numbers (3bytes per number) 3x4=12 0bazed = 11

            }



            // DEFAULT COMBO 2236
            SAFE_COMBOS[0].SafeArray[0] = 0; //
            SAFE_COMBOS[0].SafeArray[1] = 0; //
            SAFE_COMBOS[0].SafeArray[2] = 1; // 2

            SAFE_COMBOS[0].SafeArray[3] = 0; //
            SAFE_COMBOS[0].SafeArray[4] = 0; //
            SAFE_COMBOS[0].SafeArray[5] = 1; // 2

            SAFE_COMBOS[0].SafeArray[6] = 0; //
            SAFE_COMBOS[0].SafeArray[7] = 1; //
            SAFE_COMBOS[0].SafeArray[8] = 0; // 3


            SAFE_COMBOS[0].SafeArray[9] = 1; //
            SAFE_COMBOS[0].SafeArray[10] = 0; //
            SAFE_COMBOS[0].SafeArray[11] = 1; // 6



            // 2263
            SAFE_COMBOS[1].SafeArray[0] = 0; //
            SAFE_COMBOS[1].SafeArray[1] = 0; //
            SAFE_COMBOS[1].SafeArray[2] = 1; // 2

            SAFE_COMBOS[1].SafeArray[3] = 0; //
            SAFE_COMBOS[1].SafeArray[4] = 0; //
            SAFE_COMBOS[1].SafeArray[5] = 1; // 2

            SAFE_COMBOS[1].SafeArray[6] = 1; //
            SAFE_COMBOS[1].SafeArray[7] = 0; //
            SAFE_COMBOS[1].SafeArray[8] = 1; // 6


            SAFE_COMBOS[1].SafeArray[9] = 0; //
            SAFE_COMBOS[1].SafeArray[10] = 1; //
            SAFE_COMBOS[1].SafeArray[11] = 0; // 3



            // 2623
            SAFE_COMBOS[2].SafeArray[0] = 0; //
            SAFE_COMBOS[2].SafeArray[1] = 0; //
            SAFE_COMBOS[2].SafeArray[2] = 1; // 2

            SAFE_COMBOS[2].SafeArray[3] = 1; //
            SAFE_COMBOS[2].SafeArray[4] = 0; //
            SAFE_COMBOS[2].SafeArray[5] = 1; // 6

            SAFE_COMBOS[2].SafeArray[6] = 0; //
            SAFE_COMBOS[2].SafeArray[7] = 0; //
            SAFE_COMBOS[2].SafeArray[8] = 1; // 2


            SAFE_COMBOS[2].SafeArray[9] = 0; //
            SAFE_COMBOS[2].SafeArray[10] = 1; //
            SAFE_COMBOS[2].SafeArray[11] = 0; // 3



            // 2632
            SAFE_COMBOS[3].SafeArray[0] = 0; //
            SAFE_COMBOS[3].SafeArray[1] = 0; //
            SAFE_COMBOS[3].SafeArray[2] = 1; // 2

            SAFE_COMBOS[3].SafeArray[3] = 1; //
            SAFE_COMBOS[3].SafeArray[4] = 0; //
            SAFE_COMBOS[3].SafeArray[5] = 1; // 6

            SAFE_COMBOS[3].SafeArray[6] = 0; //
            SAFE_COMBOS[3].SafeArray[7] = 1; //
            SAFE_COMBOS[3].SafeArray[8] = 0; // 3


            SAFE_COMBOS[3].SafeArray[9] = 0; //
            SAFE_COMBOS[3].SafeArray[10] = 0; //
            SAFE_COMBOS[3].SafeArray[11] = 1; // 2



            // 2632
            SAFE_COMBOS[4].SafeArray[0] = 0; //
            SAFE_COMBOS[4].SafeArray[1] = 0; //
            SAFE_COMBOS[4].SafeArray[2] = 1; // 2

            SAFE_COMBOS[4].SafeArray[3] = 0; //
            SAFE_COMBOS[4].SafeArray[4] = 1; //
            SAFE_COMBOS[4].SafeArray[5] = 0; // 3

            SAFE_COMBOS[4].SafeArray[6] = 0; //
            SAFE_COMBOS[4].SafeArray[7] = 0; //
            SAFE_COMBOS[4].SafeArray[8] = 1; // 2


            SAFE_COMBOS[4].SafeArray[9] = 1; //
            SAFE_COMBOS[4].SafeArray[10] = 0; //
            SAFE_COMBOS[4].SafeArray[11] = 1; // 6




            // 3226
            SAFE_COMBOS[5].SafeArray[0] = 0; //
            SAFE_COMBOS[5].SafeArray[1] = 1; //
            SAFE_COMBOS[5].SafeArray[2] = 0; // 3

            SAFE_COMBOS[5].SafeArray[3] = 0; //
            SAFE_COMBOS[5].SafeArray[4] = 0; //
            SAFE_COMBOS[5].SafeArray[5] = 1; // 2

            SAFE_COMBOS[5].SafeArray[6] = 0; //
            SAFE_COMBOS[5].SafeArray[7] = 0; //
            SAFE_COMBOS[5].SafeArray[8] = 1; // 2


            SAFE_COMBOS[5].SafeArray[9] = 1; //
            SAFE_COMBOS[5].SafeArray[10] = 0; //
            SAFE_COMBOS[5].SafeArray[11] = 1; // 6

            // 3622
            SAFE_COMBOS[6].SafeArray[0] = 0; //
            SAFE_COMBOS[6].SafeArray[1] = 1; //
            SAFE_COMBOS[6].SafeArray[2] = 0; // 3

            SAFE_COMBOS[6].SafeArray[3] = 1; //
            SAFE_COMBOS[6].SafeArray[4] = 0; //
            SAFE_COMBOS[6].SafeArray[5] = 1; // 6

            SAFE_COMBOS[6].SafeArray[6] = 0; //
            SAFE_COMBOS[6].SafeArray[7] = 0; //
            SAFE_COMBOS[6].SafeArray[8] = 1; // 2


            SAFE_COMBOS[6].SafeArray[9] = 0; //
            SAFE_COMBOS[6].SafeArray[10] = 0; //
            SAFE_COMBOS[6].SafeArray[11] = 1; // 2


            // 2362
            SAFE_COMBOS[7].SafeArray[0] = 0; //
            SAFE_COMBOS[7].SafeArray[1] = 0; //
            SAFE_COMBOS[7].SafeArray[2] = 1; // 2

            SAFE_COMBOS[7].SafeArray[3] = 0; //
            SAFE_COMBOS[7].SafeArray[4] = 1; //
            SAFE_COMBOS[7].SafeArray[5] = 0; // 3

            SAFE_COMBOS[7].SafeArray[6] = 1; //
            SAFE_COMBOS[7].SafeArray[7] = 0; //
            SAFE_COMBOS[7].SafeArray[8] = 1; // 6


            SAFE_COMBOS[7].SafeArray[9] = 0; //
            SAFE_COMBOS[7].SafeArray[10] = 0; //
            SAFE_COMBOS[7].SafeArray[11] = 1; // 1


            // 3262
            SAFE_COMBOS[8].SafeArray[0] = 0; //
            SAFE_COMBOS[8].SafeArray[1] = 1; //
            SAFE_COMBOS[8].SafeArray[2] = 0; // 3

            SAFE_COMBOS[8].SafeArray[3] = 0; //
            SAFE_COMBOS[8].SafeArray[4] = 0; //
            SAFE_COMBOS[8].SafeArray[5] = 1; // 2

            SAFE_COMBOS[8].SafeArray[6] = 1; //
            SAFE_COMBOS[8].SafeArray[7] = 0; //
            SAFE_COMBOS[8].SafeArray[8] = 1; // 6


            SAFE_COMBOS[8].SafeArray[9] = 0; //
            SAFE_COMBOS[8].SafeArray[10] = 0; //
            SAFE_COMBOS[8].SafeArray[11] = 1; // 2


            //6322
            SAFE_COMBOS[9].SafeArray[0] = 1; //
            SAFE_COMBOS[9].SafeArray[1] = 0; //
            SAFE_COMBOS[9].SafeArray[2] = 1; // 6

            SAFE_COMBOS[9].SafeArray[3] = 0; //
            SAFE_COMBOS[9].SafeArray[4] = 1; //
            SAFE_COMBOS[9].SafeArray[5] = 0; // 3

            SAFE_COMBOS[9].SafeArray[6] = 0; //
            SAFE_COMBOS[9].SafeArray[7] = 0; //
            SAFE_COMBOS[9].SafeArray[8] = 1; // 2


            SAFE_COMBOS[9].SafeArray[9] = 0; //
            SAFE_COMBOS[9].SafeArray[10] = 0; //
            SAFE_COMBOS[9].SafeArray[11] = 1; // 2


            // 6232
            SAFE_COMBOS[10].SafeArray[0] = 1; //
            SAFE_COMBOS[10].SafeArray[1] = 0; //
            SAFE_COMBOS[10].SafeArray[2] = 1; // 6

            SAFE_COMBOS[10].SafeArray[3] = 0; //
            SAFE_COMBOS[10].SafeArray[4] = 0; //
            SAFE_COMBOS[10].SafeArray[5] = 1; // 2

            SAFE_COMBOS[10].SafeArray[6] = 0; //
            SAFE_COMBOS[10].SafeArray[7] = 1; //
            SAFE_COMBOS[10].SafeArray[8] = 0; // 3


            SAFE_COMBOS[10].SafeArray[9] = 0; //
            SAFE_COMBOS[10].SafeArray[10] = 0; //
            SAFE_COMBOS[10].SafeArray[11] = 1; // 2


            // 6223
            SAFE_COMBOS[11].SafeArray[0] = 1; //
            SAFE_COMBOS[11].SafeArray[1] = 0; //
            SAFE_COMBOS[11].SafeArray[2] = 1; // 6

            SAFE_COMBOS[11].SafeArray[3] = 0; //
            SAFE_COMBOS[11].SafeArray[4] = 0; //
            SAFE_COMBOS[11].SafeArray[5] = 1; // 2

            SAFE_COMBOS[11].SafeArray[6] = 0; //
            SAFE_COMBOS[11].SafeArray[7] = 0; //
            SAFE_COMBOS[11].SafeArray[8] = 1; // 2


            SAFE_COMBOS[11].SafeArray[9] = 0; //
            SAFE_COMBOS[11].SafeArray[10] = 1; //
            SAFE_COMBOS[11].SafeArray[11] = 0; // 3


            int combo_seed = r_combo.Next(0, SAFE_COMBOS.Length);




            using (var fs = new FileStream(rdt_file, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {

                    fs.Seek(12921, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[0]);
                    fs.Seek(12929, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[1]);
                    fs.Seek(12937, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[2]);
                    //------------- WRITE RANDOM DIGIT 1


                    fs.Seek(12967, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[3]);
                    fs.Seek(12975, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[4]);
                    fs.Seek(12983, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[5]);
                    //------------- WRITE RANDOM DIGIT 2


                    fs.Seek(13013, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[6]);
                    fs.Seek(13021, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[7]);
                    fs.Seek(13029, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[8]);
                    //------------- WRITE RANDOM DIGIT 3


                    fs.Seek(13059, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[9]);
                    fs.Seek(13067, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[10]);
                    fs.Seek(13075, SeekOrigin.Begin);
                    bw.Write(SAFE_COMBOS[combo_seed].SafeArray[11]);
                    //------------- WRITE RANDOM DIGIT 4







                }

            }

            //if (rdt_index == 249)
            //{
            //    Console.ForegroundColor = ConsoleColor.Magenta;
            //    Console.WriteLine("Safe Seed :" + combo_seed);
            //}


        }

        public static void Box_Shuffle(string rdt_file)
        {



            Random r_Pos = new Random();



        }

        public static void Statue_Shuffle(string rdt_file)
        {

            Random r_Pos = new Random();


            LIB_RDT.STATUE_POS_OBJ[] STATUE_SET = new LIB_RDT.STATUE_POS_OBJ[5];



            // gray
            STATUE_SET[0].statue_1X = -22900;
            STATUE_SET[0].statue_1Z = -13000;
            STATUE_SET[0].statue_1R = 2048;
            //####> Default Setup
            //bronze
            STATUE_SET[0].statue_2X = -20100;
            STATUE_SET[0].statue_2Z = -13000;
            STATUE_SET[0].statue_2R = 0;



            // 
            STATUE_SET[1].statue_1X = 6979;
            STATUE_SET[1].statue_1Z = -8039;
            STATUE_SET[1].statue_1R = 2048;
            //####> Trolliest setup ever
            //bronze
            STATUE_SET[1].statue_2X = -3626;
            STATUE_SET[1].statue_2Z = -10137;
            STATUE_SET[1].statue_2R = 0;


            // rotation flips
            STATUE_SET[2].statue_1X = -22900;
            STATUE_SET[2].statue_1Z = -13000;
            STATUE_SET[2].statue_1R = 0;
            //####> Default Setup
            //bronze
            STATUE_SET[2].statue_2X = -20100;
            STATUE_SET[2].statue_2Z = -13000;
            STATUE_SET[2].statue_2R = 2048;


            // quick setup
            STATUE_SET[3].statue_1X = -17000;
            STATUE_SET[3].statue_1Z = -14300;
            STATUE_SET[3].statue_1R = 0;
            //
            //bronze
            STATUE_SET[3].statue_2X = -26000;
            STATUE_SET[3].statue_2Z = -10800;
            STATUE_SET[3].statue_2R = 2048;








            int pos_Seed = r_Pos.Next(0, 4);

            using (var fs = new FileStream(rdt_file, FileMode.Open))
            {

                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    fs.Seek(6514, SeekOrigin.Begin);
                    bw.Write(STATUE_SET[pos_Seed].statue_1X); // write random x pos
                    fs.Seek(6518, SeekOrigin.Begin);
                    bw.Write(STATUE_SET[pos_Seed].statue_1Z);
                    fs.Seek(6522, SeekOrigin.Begin);
                    bw.Write(STATUE_SET[pos_Seed].statue_1R);


                    fs.Seek(6552, SeekOrigin.Begin);
                    bw.Write(STATUE_SET[pos_Seed].statue_2X); // write random x pos
                    fs.Seek(6556, SeekOrigin.Begin);
                    bw.Write(STATUE_SET[pos_Seed].statue_2Z);
                    fs.Seek(6560, SeekOrigin.Begin);
                    bw.Write(STATUE_SET[pos_Seed].statue_2R);





                }


            }

        }

        /// <summary>
        /// Set the final birkin timer to different values
        /// </summary>
        /// <param name="rdt_File"></param>
        public static void Timer_Shuffle(string rdt_File, int rdt_index)
        {
            Int16 seconds = 0;
            Random r_time = new Random();

            var val = r_time.Next(0, 4);


            using (FileStream fs = new FileStream(rdt_File, FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    if (rdt_index == 119)
                    {

                        switch (val)
                        {
                            case 0: seconds = 200; break;
                            case 1: seconds = 125; break;
                            case 2: seconds = 105; break;
                            case 3: seconds = 220; break;
                            case 4: seconds = 666; break; // joke
                        }

                        fs.Seek(14540, SeekOrigin.Begin);
                        bw.Write(seconds);
                    }

                    if (rdt_index == 123)
                    {

                        switch (val)
                        {
                            case 0: seconds = 120; break;
                            case 1: seconds = 100; break;
                            case 2: seconds = 85; break;
                            case 3: seconds = 61; break;
                        }

                        fs.Seek(8882, SeekOrigin.Begin);
                        bw.Write(seconds);
                    }


                    if (rdt_index == 121)
                    {

                        switch (val)
                        {
                            case 0: seconds = 300; break;
                            case 1: seconds = 250; break;
                            case 2: seconds = 225; break;
                            case 3: seconds = 200; break;
                        }

                        fs.Seek(9084, SeekOrigin.Begin);
                        bw.Write(seconds);
                    }



                }
            }

        }


        /// <summary>
        /// Disable Random Item boxes ......
        /// just going to disable the trigger
        /// </summary> 201, 208, 10F, 30C, 30D, 403, 503, 607, 612 (ROOM IDS)
        /// <param name="rdt_file"></param>
        /// <param name="rdt_index"></param>
        public static void ITEMBOX_DISABLE(string rdt_file, int boxCount, List<int> RoomList, int i)
        {


            using (FileStream fs = new FileStream(rdt_file, FileMode.Open))
            {

                using (BinaryWriter bw = new BinaryWriter(fs))
                {

                    // switch through the case list and whatever it hits, fuck the item box in the ass
                    switch (RoomList[i])
                    {

                        case 15: //10f0.rdt 2f save room
                            fs.Seek(1576, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type
                            Console.WriteLine("2F save room Box Disabled");

                            break;

                        case 31:     // 2010.RDT (waiting room)
                            fs.Seek(3084, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type
                            Console.WriteLine("Waiting Room Box Disabled");
                            break;

                        case 38:  // 2080.RDT (dark room)
                            fs.Seek(4840, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type
                            Console.WriteLine("Dark Room Box Disabled");
                            break;


                        case 68:  // 30C0.RDT (Sewer Save room WEST)
                            fs.Seek(1080, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type
                            Console.WriteLine("Sewer Save Room WEST Box Disabled");
                            break;

                        case 69:  // 30D0.RDT (Sewer Save room EAST)
                            fs.Seek(620, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type
                            Console.WriteLine("Sewer save room EAST Box Disabled");
                            break;


                        case 73:  // MANAGMENT ROOM NORTH
                            fs.Seek(8834, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type
                            Console.WriteLine("Management room north box disabled");
                            break;


                        case 89:  //BASEMENT CORRIDOR ONE
                            fs.Seek(2094, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type
                            Console.WriteLine("Basement Corridor one box disabled");
                            break;


                        case 103:  //BASEMENT CORRIDOR ONE
                            fs.Seek(2984, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type
                            Console.WriteLine("Lab save room box Disabled");
                            break;


                        case 113:  //MAIN SHAFT B 6120.rdt
                            fs.Seek(5318, SeekOrigin.Begin); // seek offset
                            bw.Write((byte)0x0B); // write dmg animation 
                            bw.Write((byte)0x41); // write collision type

                            break;


                    }






                }
            }


        }

        public static void Disable_Letterbox(int Off, string rdt_file, FileStream fs)
        {

            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                fs.Seek(Off + 3, SeekOrigin.Begin);
                bw.Write((byte)0x00);
            }

        }




        //public static void Randomize_Common(LIB_RDT.ITEM_DATA_OBJ[] Item_AOT, LIB_RDT.MD1_MODEL_OBJ[] MD1_OBJ, List<int> ITEM_OFFS, List<int> MD1_OFFS, FileStream fs, BinaryWriter bw)
        //{

        //    int seed = 0;
        //    Random r_item = new Random();
        //    Random r_quantity = new Random(seed);





        //    for (int i = 0; i < ITEM_OFFS.Count; i++) // for all rdt items
        //    {

        //        if (Item_AOT[i].item >= 0x01 && Item_AOT[i].item < 0x2E)
        //        {

        //            // if item between empty and GGG herb (last item b4 keys)


        //            //  byte t = byte.Parse(r_item.Next(0x01, 0x2E).ToString()); // randomize item ID between common range

        //            Item_AOT[i].item = byte.Parse(r_item.Next(0x01, 0x2E).ToString()); // randomize item ID between common range

        //            if (LIB_ITEM.BIO2_LUT_QUANTITY.ContainsKey(Item_AOT[i].item))
        //            {
        //                Item_AOT[i].amount = short.Parse(r_quantity.Next(0x01, LIB_ITEM.BIO2_LUT_QUANTITY[Item_AOT[i].item]).ToString()); // randomize quantity between 10?
        //            }
        //            else
        //            {
        //                Item_AOT[i].amount = 1;
        //            }

        //            Item_AOT[i].flag = 0x5D; // force blink color, 0x7C == club key 0x5D spade key
        //                                     //    Item_AOT[i].md1 = 0x00;
        //            Item_AOT[i].ani = 0xA0; // Anim type, 0xA1 == Crouch, 0xA0 == mid? 



        //            fs.Seek(ITEM_OFFS[i] + 14, SeekOrigin.Begin); // seek to item in file
        //                                                          //Console.WriteLine("Seeked struct off " + LIB_ITEM.BIO2_ITEM_LUT[Item_AOT[i].item]);



        //            bw.Write(Item_AOT[i].item);
        //            fs.Seek(+1, SeekOrigin.Current);
        //            bw.Write(Item_AOT[i].amount);

        //            bw.Write(Item_AOT[i].flag);

        //            bw.Write(Item_AOT[i].md1);

        //            bw.Write(Item_AOT[i].ani);


        //            // loop through all


        //            for (int z = 0; z < MD1_OFFS.Count; z++)
        //            {
        //                if (MD1_OBJ[z].md1 == Item_AOT[i].md1) // assuming the md1 in the md1 load matches the item aot md1 over write it 00, thus disabling it from the game
        //                {
        //                    fs.Seek(MD1_OFFS[z] + 1, SeekOrigin.Begin);
        //                    bw.Write(0x00);
        //                }
        //            }

        //        }
        //    }


        //}



    }


































}
