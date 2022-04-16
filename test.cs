public void GetBars(Bitmap img)
{
    using (Graphics g = Graphics.FromImage(img))
    {
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
                }
            }
        }
    }
}

public Bitmap GetTypeImg()
{
    int x = 0;
    int y = 230;
    int h = 220;
    int w = 315;
    switch (Type_)
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
        g.DrawImage(new Bitmap(title), -x, -y, 1777, 999);
    }
    return o;
}
static void Main(string[] args){
    Console.WriteLine("This is C#");
}