using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ConsoleAppMario
{
    class KartSetupCombo
    {
        string character;
        string kart;
        string wheels;
        string glider;
        double speed;
        double accelleration;
        double weight;
        double traction;
        double handling;
        Bitmap bmCharacter;
        Bitmap bmKart;
        Bitmap bmWheels;
        Bitmap bmGlider;

        public KartSetupCombo(string c, string k , string w, string g, double sp, double ac, double we, double tr, double ha) {
            this.character = c;
            this.kart = k;
            this.wheels = w;
            this.glider = g;
            this.speed = sp;
            this.accelleration = ac;
            this.weight = we;
            this.traction = tr;
            this.handling = ha;
      /*      this.bmCharacter = bmc;
            this.bmKart = bmk;
            this.bmWheels = bmw;
            this.bmGlider = bmg;*/
        }
        public string GetChar()
        {
            return character;
        }
        public string GetKart()
        {
            return kart;
        }
        public string GetWheels()
        {
            return wheels;
        }
        public string GetGlider()
        {
            return glider;
        }
        public double GetSpeed()
        {
            return speed;
        }
        public double GetAccell()
        {
            return accelleration;
        }
        public double GetHandling()
        {
            return handling;
        }
        public double GetWeight()
        {
            return weight;
        }
        public double GetTraction()
        {
            return traction;
        }
       /* public byte[] GetBMCharacter()
        {
            using (var memoryStream = new MemoryStream())
            {
                bmCharacter.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
        public byte[] GetBMKart()
        {
            using (var memoryStream = new MemoryStream())
            {
                bmKart.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
        public byte[] GetBMWheels()
        {
            using (var memoryStream = new MemoryStream())
            {
                bmWheels.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
        public byte[] GetBMGlider()
        {
            using (var memoryStream = new MemoryStream())
            {
                bmGlider.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }*/
        public double GetBest()
        {
            return speed + accelleration + traction + weight + handling;
        }
    }
}
