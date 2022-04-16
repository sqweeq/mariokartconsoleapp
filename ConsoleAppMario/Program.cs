using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ConsoleAppMario
{
    class Program
    {
        public static Dictionary<string, List<double>> ImageAttributesDict = new Dictionary<string, List<double>>();
        public static Dictionary<string, Bitmap> ImageBitmapDict = new Dictionary<string, Bitmap>();
        public static Dictionary<string, KartSetupCombo> CombinationDict = new Dictionary<string, KartSetupCombo>();
        static void Main(string[] args)
        {


            SQLiteConnection sqlite_conn = CreateConnection();
            //creates sqlite database file and table with indexes
            CreateTable(sqlite_conn);
            //ReadData(sqlite_conn);

            //assuming the 4 folders with images are in ConsoleAppMario\bin\Debug\netcoreapp3.1,
            //this will generate the ImageAttributesDict, with key of filename.png and value of List array[speed,accel,weight,handling,traction]
            
            //GetAllBarsFromImages();

            //just outputs bitmap of each selection on images in ConsoleAppMario\bin\Debug\netcoreapp3.1 folder
            //add to ImageBitmapDict key image, value bitmap pair. was going to store bitmaps of each combo in sql
            //but a little too complex
           // GetAllBitmapFromImages();


            //creates all combinations of karts,charac,wheels,flyers 542430 combinations.
            //stats(speed,accel.....) calculated  using each combo of kart,charac,wheels,flyers summed stats
            //then divided by 4 for the average
            //CreateAllCombinations();
            
            
            //insert into db
            //InsertData(sqlite_conn);





        }
        //creates connection, db created stored in ConsoleAppMario\bin\Debug\netcoreapp3.1
        static SQLiteConnection CreateConnection() {
            SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");

            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }
        //create table
        static bool CreateTable(SQLiteConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                string tablename = "MarioTable";
                var sql ="SELECT name FROM sqlite_master WHERE type='table' AND name='" + tablename + "';";

                SQLiteCommand command = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                //if table exist then create index if index not exists
                if (reader.HasRows)
                {
                    Console.WriteLine("TABLE EXIST");
                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = conn.CreateCommand();
                    sqlite_cmd.CommandText = "CREATE INDEX IF NOT EXISTS filter_index on '" + tablename + "'(speed,accel,weight,traction,handling,best)";
                    sqlite_cmd.ExecuteNonQuery();

                    return true;
                }
                else
                {
                    //no table exist, create table and index
                    Console.WriteLine("NO TABLE EXIST");
                    SQLiteCommand sqlite_cmd;
                    /*                  string charac,  kart, wheel, glid, 
                                        double speed, accel, weight, traction, handling, 
                                        Bitmap bmc, bmk, bmw, bmg*/
                    sqlite_cmd = conn.CreateCommand();
                    sqlite_cmd.CommandText = "CREATE TABLE '" + tablename + "'" +
                        "(combo_id INTEGER PRIMARY KEY, charac TEXT NOT NULL, kart TEXT NOT NULL, wheel TEXT NOT NULL, glid TEXT NOT NULL," +
                        "speed REAL, accel REAL, weight REAL, traction REAL, handling REAL, best REAL)"; 
                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_cmd.CommandText = "CREATE INDEX filter_index on '" + tablename + "'(speed,accel,weight,traction,handling)";
                    sqlite_cmd.ExecuteNonQuery();

                    return false;
                }
            }
            else
            {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }
        }
        //insert data(rows)
        static bool InsertData(SQLiteConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                string tablename = "MarioTable";
                var sql = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tablename + "';";

                SQLiteCommand command = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("theres table, ready to insert");

                    SQLiteCommand sqlite_cmd = conn.CreateCommand();

                    /*loop CombinationDict and execute inserts to db*/
                    foreach (var value in CombinationDict.Values)
                    {
                        /*  string bmc = Convert.ToBase64String(value.GetBMCharacter());
                          string bmk = Convert.ToBase64String(value.GetBMKart());
                          string bmw = Convert.ToBase64String(value.GetBMWheels());
                          string bmg = Convert.ToBase64String(value.GetBMGlider());*/

                        sqlite_cmd.CommandText = "INSERT INTO '" + tablename + "'(charac,  kart, wheel, glid, " +
                            "speed, accel, weight, traction, handling, best)" + 
                            "VALUES('" + value.GetChar() + "','" + value.GetKart() + "','" + value.GetWheels() + "','" + value.GetGlider() + "'," +
                            "'" + value.GetSpeed() + "','" + value.GetAccell() + "','" + value.GetWeight() + "','" + value.GetTraction() + "','" + value.GetHandling() + "','" + value.GetBest() + "')"; 
                        sqlite_cmd.ExecuteNonQuery();

                    }
                    Console.WriteLine("add rows success");
                    return true;
                }
                else
                {
                    Console.WriteLine("NO TABLE EXIST SO NO ROWS TO INSERT TO");
                    /* sqlite_cmd.CommandText = "INSERT INTO SampleTable(Col1, Col2) VALUES('Test Text ', 1); ";*/

                    /*                  string charac,  kart, wheel, glid, 
                                        double speed, accel, weight, traction, handling, best 
                                        Bitmap bmc, bmk, bmw, bmg*/
                    /* string Createsql = "CREATE TABLE '"+tablename + "'(Col1 VARCHAR(20), Col2 INT)";*/
                    SQLiteCommand sqlite_cmd = conn.CreateCommand();

                    /*loop CombinationDict and execute inserts to db*/
                    foreach (var value in CombinationDict.Values) {
                        /*                     string bmc = Convert.ToBase64String(value.GetBMCharacter());
                                             string bmk = Convert.ToBase64String(value.GetBMKart());
                                             string bmw = Convert.ToBase64String(value.GetBMWheels());
                                             string bmg = Convert.ToBase64String(value.GetBMGlider());*/

                        sqlite_cmd.CommandText = "INSERT INTO '" + tablename + "'(charac,  kart, wheel, glid, " +
                            "speed, accel, weight, traction, handling, best) " +
                            "VALUES ('" + value.GetChar() + "','" + value.GetKart() + "','" + value.GetWheels() + "','" + value.GetGlider() + "'," +
                            "'" + value.GetSpeed() + "','" + value.GetAccell() + "','" + value.GetWeight() + "','" + value.GetTraction() + "','" + value.GetHandling() + "','" + value.GetBest() + "')";
                        sqlite_cmd.ExecuteNonQuery();

                    }
                    Console.WriteLine("add rows FAIL DUE TO NO ROWS");

                    return false;
                }
            }
            else
            {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }
        
        }
        //read data
        static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT COUNT(*) FROM MARIOTABLE";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }
            conn.Close();
        }

        //creates all combinations of karts,charac,wheels,flyers 542430 combinations.
        //stats(speed,accel.....) calculated  using each combo of kart,charac,wheels,flyers summed stats
        //then divided by 4 for the average
        static public void CreateAllCombinations() {
            int count = 0;
            //iterate each file in the 4 folders with each other
            foreach (string kartFileName in Directory.GetFiles("karts"))
            {
                string kartWithoutExt = Path.GetFileNameWithoutExtension(kartFileName);
                foreach (string gliderFileName in Directory.GetFiles("gliders"))
                {
                    string gliderWithoutExt = Path.GetFileNameWithoutExtension(gliderFileName);
                    foreach (string charFileName in Directory.GetFiles("charaters"))
                    {
                        string charNameWithoutExt = Path.GetFileNameWithoutExtension(charFileName);
                        foreach (string wheelsFileName in Directory.GetFiles("wheels"))
                        {
                            string wheelsWithoutExt = Path.GetFileNameWithoutExtension(wheelsFileName);
                            //using each combo of kart,charac,wheels,flyers summed stats(from dict made from reading bars)
                            //then divided by 4 for the average
                            count++;
                            double speed = (ImageAttributesDict[kartWithoutExt][0] +
                                ImageAttributesDict[gliderWithoutExt][0] +
                                ImageAttributesDict[charNameWithoutExt][0] +
                                ImageAttributesDict[wheelsWithoutExt][0]) / 4;
                            double accell = (ImageAttributesDict[kartWithoutExt][1] +
                                ImageAttributesDict[gliderWithoutExt][1] +
                                ImageAttributesDict[charNameWithoutExt][1] +
                                ImageAttributesDict[wheelsWithoutExt][1]) / 4;
                            double weight = (ImageAttributesDict[kartWithoutExt][2] +
                                ImageAttributesDict[gliderWithoutExt][2] +
                                ImageAttributesDict[charNameWithoutExt][2] +
                                ImageAttributesDict[wheelsWithoutExt][2]) / 4;
                            double handling = (ImageAttributesDict[kartWithoutExt][3] +
                                ImageAttributesDict[gliderWithoutExt][3] +
                                ImageAttributesDict[charNameWithoutExt][3] +
                                ImageAttributesDict[wheelsWithoutExt][3]) / 4;
                            double traction = (ImageAttributesDict[kartWithoutExt][4] +
                                ImageAttributesDict[gliderWithoutExt][4] +
                                ImageAttributesDict[charNameWithoutExt][4] +
                                ImageAttributesDict[wheelsWithoutExt][4]) / 4;
                            //store object in CombinationDict using combo number as key 
                            CombinationDict["combo" + count.ToString()] = new KartSetupCombo(
                                charNameWithoutExt, kartWithoutExt, wheelsWithoutExt, gliderWithoutExt,
                                speed, accell, weight, traction, handling
                                );
                        }
                    }
                }
            }
            Console.WriteLine(count);
        }

        //iterate all images and run GetBars
        static public void GetAllBarsFromImages()
        {
            foreach(string imageFileName in Directory.GetFiles("karts"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                GetBars(new Bitmap(imageFileName), fileNameWithoutExt);
/*                Console.WriteLine( Path.GetFileNameWithoutExtension(imageFileName));
*/            }
            foreach (string imageFileName in Directory.GetFiles("gliders"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                GetBars(new Bitmap(imageFileName), fileNameWithoutExt);
              
            }
            foreach (string imageFileName in Directory.GetFiles("charaters"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                GetBars(new Bitmap(imageFileName), fileNameWithoutExt);
            }
            foreach (string imageFileName in Directory.GetFiles("wheels"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                GetBars(new Bitmap(imageFileName), fileNameWithoutExt);
            }
        }


        /* gets game attributes of an image (item )and stores in ImageAttributesDict */
        static public void GetBars(Bitmap img , string filename)
        {
           
            using (Graphics g = Graphics.FromImage(img))
            {
                double speed = 0;
                double accell = 0;
                double weight = 0;
                double handling = 0;
                double traction = 0;
                double dividedby = 0.01236749116;

                for (var y1 = 0; y1 <= 440; y1++)
                {
                    for (var x1 = 0; x1 <= 550; x1++)
                    {
                        int x = 1090 + x1;
                        int y = 40 + y1;
                        Color c = img.GetPixel(x, y);
                        if (System.Convert.ToInt32(c.R) > 180 & System.Convert.ToInt32(c.G) > 180)
                        {
                            img.SetPixel(x, y, Color.Magenta);
                            //depending on y axis if the x axis is yellow
                            if (y == 115) {
                                speed++;
                            }
                            if (y == 200)
                            {
                                accell++;
                            }
                            if (y == 280)
                            {
                                weight++;
                            }
                            if (y == 360)
                            {
                                handling++;
                            }
                            if (y == 440)
                            {
                                traction++;
                            }
                        }
                    }
                } 
                speed = Math.Round(speed * dividedby, 2);
                accell = Math.Round(accell * dividedby, 2);
                weight = Math.Round(weight * dividedby, 2);
                handling = Math.Round(handling * dividedby, 2);
                traction = Math.Round(traction * dividedby, 2);
             /*   add attributes to list for value of given key(filename) in dict*/
                List<double> statslist = new List<double>();
                statslist.Add(speed);
                statslist.Add(accell);
                statslist.Add(weight);
                statslist.Add(handling);
                statslist.Add(traction);
                ImageAttributesDict[filename] = statslist;
            }
        }
        //run GetTypeImg for each image
        static public void GetAllBitmapFromImages()
        {
            foreach (string imageFileName in Directory.GetFiles("karts"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                GetTypeImg( Image.FromFile(imageFileName), fileNameWithoutExt, "kart");
            }
            foreach (string imageFileName in Directory.GetFiles("gliders"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                GetTypeImg(Image.FromFile(imageFileName), fileNameWithoutExt, "glider");
            }
            foreach (string imageFileName in Directory.GetFiles("charaters"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                GetTypeImg(Image.FromFile(imageFileName), fileNameWithoutExt,"charater");
            }
            foreach (string imageFileName in Directory.GetFiles("wheels"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                GetTypeImg(Image.FromFile(imageFileName), fileNameWithoutExt,"wheel");
            }
        }
        //  generates bitmap image given a type of image(game item), for displaying later
        // store all images in ConsoleAppMario\bin\Debug\netcoreapp3.1
        static public void GetTypeImg(Image img, string filename,  string type)
        {
            int x = 0;
            int y = 230;
            int h = 220;
            int w = 315;
            switch (type)
            {
                case "kart":
                    {
                        x = 66;
                        break;
                    }

                case "charater":
                    {
                        x = 1264;
                        y = 350;
                        break;
                    }

                case "glider":
                    {
                        x = 672;
                        break;
                    }

                case "wheel":
                    {
                        x = 366;
                        break;
                    }
            }
            Bitmap o = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(o))
            {
                g.DrawImage(new Bitmap(img), -x, -y, 1777, 999);
            }
            //store image
            o.Save(filename + ".png");

            ImageBitmapDict[filename] = o;
        }
    }
}
