using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
 

namespace bio2_rndx
{
    public static class CmdParse
    {

        /// <summary>
        /// Debug Mode Prompt
        /// </summary>
        public static byte PromptDebug(byte dbg_flag) {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enable Debugging? Y\\N");


            ConsoleKeyInfo opt = Console.ReadKey();
           
                if (opt.Key == ConsoleKey.Y) { Console.ForegroundColor = ConsoleColor.Green; dbg_flag = 1; Console.WriteLine("\n\nDebugging Enabled \n"); System.Threading.Thread.Sleep(300); }
                else { Console.ForegroundColor = ConsoleColor.Red; dbg_flag = 0; Console.WriteLine("\n\nDebugging Disabled \n"); System.Threading.Thread.Sleep(300); }
             //    else {  Console.WriteLine("\nChow"); System.Threading.Thread.Sleep(500); Environment.Exit(0); }


            return dbg_flag;

        }

        public static byte PromptEnemy(byte em_flag)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enable Enemy Swap?\n Y/N");
            ConsoleKeyInfo opt = Console.ReadKey();

            if (opt.Key == ConsoleKey.Y) { Console.ForegroundColor = ConsoleColor.Green; em_flag = 1; Console.WriteLine("\n\nEnemy Swap Enabled \n"); System.Threading.Thread.Sleep(300); }
            else { Console.ForegroundColor = ConsoleColor.Red; em_flag = 0; Console.WriteLine("\n\nEnemy Swap Disabled \n"); System.Threading.Thread.Sleep(300); }


            //if (em_flag == 1) {

            //    EnemyPrompt();
            //}

            return em_flag;
           
        }


        /// <summary>
        /// Puzzle Shuffling
        /// </summary>
        /// <param name="pz_flag"></param>
        /// <returns></returns>
        public static byte PromptPuzzle(byte pz_flag)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enable Puzzle Shuffle? (Puzzles/Timer Rando) \n Y/N");
            ConsoleKeyInfo opt = Console.ReadKey();

            if (opt.Key == ConsoleKey.Y) { Console.ForegroundColor = ConsoleColor.Green; pz_flag = 1; Console.WriteLine("\n\n Puzzle Shuffle Enabled \n"); System.Threading.Thread.Sleep(300); }
           else { Console.ForegroundColor = ConsoleColor.Red; pz_flag = 0; Console.WriteLine("\n\n Puzzle Shuffle Disabled \n"); System.Threading.Thread.Sleep(300); }
           
            return pz_flag;

        }


        /// <summary>
        /// Cutscene swapping for lulz
        /// </summary>
        /// <param name="pz_flag"></param>
        /// <returns></returns>
        public static byte PromptCut(byte cut_flag)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enable Cutscene Swap? \n Y/N");
            ConsoleKeyInfo opt = Console.ReadKey();

            if (opt.Key == ConsoleKey.Y) { Console.ForegroundColor = ConsoleColor.Green; cut_flag = 1; Console.WriteLine("\n\n Cutscene Swap Enabled \n"); System.Threading.Thread.Sleep(300); }
            else { Console.ForegroundColor = ConsoleColor.Red; cut_flag = 0; Console.WriteLine("\n\n Cutscene Swap Disabled \n"); System.Threading.Thread.Sleep(300); }



            return cut_flag;

        }


        /// <summary>
        /// Hunk/A/B enemy layout prompt
        /// </summary>
        /// <param name="layout_flag"></param>
        /// <returns></returns>
        public static byte PromptLayout(byte layout_flag)
        { 

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enable Enemy Layout Shuffle? (Tofu/A/B ROOMS & Boss Flips) \n Y/N");
            ConsoleKeyInfo opt = Console.ReadKey();

            if (opt.Key == ConsoleKey.Y) { Console.ForegroundColor = ConsoleColor.Green; layout_flag = 1; Console.WriteLine("\n\n Enemy Layout Shuffle Enabled \n"); System.Threading.Thread.Sleep(300); }
            else { Console.ForegroundColor = ConsoleColor.Red; layout_flag = 0; Console.WriteLine("\n\n Enemy Layout Shuffle Disabled \n"); System.Threading.Thread.Sleep(300); }



            return layout_flag;

        }


        public static byte PromptHG(byte hg_flag) {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Disble Handgun from starting load out? \n Y/N");
            ConsoleKeyInfo opt = Console.ReadKey();

            if (opt.Key == ConsoleKey.Y) { Console.ForegroundColor = ConsoleColor.Red; hg_flag = 1; Console.WriteLine("\n\n Handgun Disabled \n"); System.Threading.Thread.Sleep(300); }
            else { Console.ForegroundColor = ConsoleColor.Green; hg_flag = 0; Console.WriteLine("\n\n Handgun Enabled \n"); System.Threading.Thread.Sleep(300); }


            return hg_flag;
        }

        public static byte PromptItemBox(byte Box_Flag)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enable Item Box Mode? (Random Boxes Disabled) \n Y/N");
            ConsoleKeyInfo opt = Console.ReadKey();

            if (opt.Key == ConsoleKey.Y) { Console.ForegroundColor = ConsoleColor.Green; Box_Flag = 1; Console.WriteLine("\n\n Item Box Mode Enabled \n"); System.Threading.Thread.Sleep(300); }
            else { Console.ForegroundColor = ConsoleColor.Red; Box_Flag = 0; Console.WriteLine("\n\n Item Box Mode Disabled \n"); System.Threading.Thread.Sleep(300); }



            return Box_Flag;

        }


        public  static void SeedLog(List<string> output, string configPath) {

            using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\SeedLog.Txt")) {

                foreach (string str in output) {

                    sw.WriteLine(str);
                    
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nLog Succesffuly Written to " + AppDomain.CurrentDomain.BaseDirectory + "as 'SeedLog.txt'");

            }

            Console.WriteLine("\n And Copied to  " + configPath + "\\pl0\\rdt as 'SeedLog.txt'");

            File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\SeedLog.Txt", configPath + "\\pl0\\rdt\\SeedLog.Txt", true);

        }




        /// <summary>
        /// 0x11 BRADS
        /// 0x1F = ZOMBIE 00
        /// 0x15 ZOMBIE 01
        /// 0x17 = ZOMBIE 03
        /// 0x20 = DOGS
        /// 0x21 = CROWS
        /// 0x22 = LICKERS
        /// 0x24 = SUPER LICKERS
        /// 0x25 = SPIDERS
        /// 0x2E = IVY
        /// 0x39 purple ivy
        /// 0x2A MR X
        /// 0x2D hands
        /// 0x3A MOTH
        /// 0x44 BEN
        /// 0x
        /// 

        public enum EM_BIT
        {
            BRAD,
            ZOMBIE00,
            ZOMBIE01,
            ZOMBIE02,
            DOGS,
            CROWS,
            LICKERS,
            SUPERLICKERS,
            SPIDERS,
            IVY,
            PURPLE_IVY,
            MRX,
            HANDS,
            MOTH
                
        }

        /// </summary>
        public static void EnemyPrompt() {


            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine(Figgle.FiggleFonts.ShadowSmall.Render("ENEMY SELECTION DIALOGUE"));
            Console.WriteLine("Which enemies do you want to include\n");

            string BitStr = string.Empty;
            byte[] emdArray = new byte[15];
            ConsoleKeyInfo opt = Console.ReadKey();


            Console.WriteLine("Enable 0x11 BRADS?");

            
            




            //Console.WriteLine("0x1F ZOMBIE 00");

            //Console.WriteLine("0x15 ZOMBIE 01");

            //Console.WriteLine("0x17 ZOMBIE 03");

            //Console.WriteLine("0x20 DOGS");

            //Console.WriteLine("0x21 CROWS");

            //Console.WriteLine("0x22 LICKERS");

            //Console.WriteLine("0x24 SUPER LICKER");

            //Console.WriteLine("0x25 SPIDERS");

            //Console.WriteLine("0x2E IVYS");

            //Console.WriteLine("0x39 PURPLE IVYS");

            //Console.WriteLine("0x2A MR X");

            //Console.WriteLine("0x2D HANDS");

            //Console.WriteLine("0x3A MOTH");

            //Console.WriteLine("0x44 BENS??");

            //for (int i = 0; i < emdArray.Length; i++) {

            //    emdArray[i] = 

            //}


            


            

        }



    }
}
