using BattleShipsLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattleShips
{
    public static class Options
    {
        public static byte[] serverIP;
        public static int serverPort = -1;

        public static List<Vector2i> allowedShips = new List<Vector2i>(6);

        public static int gridSize = -1;
        public static int cellWidth = -1;
        public static int cellHeight = -1;
        
        public static void LoadOptions()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "/options.txt"))
            {
                CreateOptionsFile();
                MessageBox.Show("Options file was not found.\nCreated an options file at the application folder");
            }
            if (File.Exists(Directory.GetCurrentDirectory() + "/options.txt"))
            {
                serverIP = new byte[4];
                var file = File.OpenText(Directory.GetCurrentDirectory() + "/options.txt");
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    if (line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("IP:"))
                    {
                        line = line.Split(':')[1];
                        var lines = line.Split('.');
                        for (int i = 0; i < lines.Count(); i++)
                        {
                            if (i > 3)
                                break;
                            byte.TryParse(lines[i], out serverIP[i]);
                        }
                    }
                    else if (line.StartsWith("Port:"))
                    {
                        line = line.Split(':')[1];
                        int.TryParse(line.Trim(), out serverPort);
                    }
                    else if(line.StartsWith("Ship:"))
                    {
                        int size;
                        int amount;
                        line = line.Split(':')[1];
                        string[] lines;
                        if (line.Contains(","))
                            lines = line.Split(',');
                        else
                            lines = line.Split('.');

                        int.TryParse(lines[0].Trim(), out size);
                        int.TryParse(lines[1].Trim(), out amount);

                        allowedShips.Add(new Vector2i(size, amount));
                    }
                    /*
                    else if (line.StartsWith("GridSize:"))
                    {
                        line = line.Split(':')[1];
                        int.TryParse(line.Trim(), out gridSize);
                    }
                    else if (line.StartsWith("CellWidth:"))
                    {
                        line = line.Split(':')[1];
                        int.TryParse(line.Trim(), out cellWidth);
                    }
                    else if (line.StartsWith("CellHeight:"))
                    {
                        line = line.Split(':')[1];
                        int.TryParse(line.Trim(), out cellHeight);
                    }
                    */
                }
            }
            
        }

        /// <summary>
        /// Creates the OptionsFile if it doesn't exist
        /// </summary>
        private static void CreateOptionsFile()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "/options.txt"))
            {
                var writer = File.CreateText(Directory.GetCurrentDirectory() + "/options.txt");
                writer.WriteLine("# Comments can be made by using the # char");
                writer.WriteLine("IP: 192.168.4.250");
                writer.WriteLine("Port: 25000");
                writer.WriteLine("# Ships should have 2 values.");
                writer.WriteLine("# The first one is the size of the ship.");
                writer.WriteLine("# The second on is the amount of that type ships");
                writer.WriteLine("# The two numbers is seperated by . or ,");
                writer.WriteLine("Ship: 2,2");
                writer.WriteLine("Ship: 3,2");
                writer.WriteLine("Ship: 4,1");
                writer.WriteLine("Ship: 5,1");
                /*
                writer.WriteLine("GridSize: 10");
                writer.WriteLine("CellWidth: 32");
                writer.WriteLine("CellHeight: 32");
                */
                writer.Flush();
                writer.Close();
            }

        }
    }
}
