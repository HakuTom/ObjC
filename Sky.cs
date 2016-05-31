using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using MetroFramework.Controls;
using System.Linq;

[Serializable]
public class Sky
{
    List <Star> TheSky;


	public Sky()
	{
        string filename = "skyData.dat";

        if (!File.Exists(filename))
        {
            TheSky = new List<Star>();
            Serialize(TheSky, filename);
        }
        else
        {
            TheSky = Deserialize<List<Star>>(filename);
        }
    }

    public bool Add(Star star)
    {
        if (TheSky.Exists(x => x.Name == star.Name))
        {
            MessageBox.Show(
                "Звезда с таким название уже существует!",
                "Внимание!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Hand);
            return false;
        }
        else {
            string filename = "skyData.dat";
            TheSky.Add(star);
            Serialize(TheSky, filename);

            return true;
        }
    }

    public Star this[int i]
    {
        get
        {
            return this.TheSky[i];
        }
        set
        {
            this.TheSky.Add(value);
        }
    }

    public static void Serialize<T>(T sky, String filename)
    {
        //Create the stream to add object into it.
        System.IO.Stream ms = File.OpenWrite(filename);
        //Format the object as Binary
        BinaryFormatter formatter = new BinaryFormatter();
        //It serialize the employee object
        formatter.Serialize(ms, sky);
        ms.Flush();
        ms.Close();
        ms.Dispose();
    }

    public static T Deserialize<T>(String filename)
    {
        //Format the object as Binary
        BinaryFormatter formatter = new BinaryFormatter();
        //Reading the file from the server
        FileStream fs = File.Open(filename, FileMode.Open);
        //It deserializes the file as object.
        object obj = formatter.Deserialize(fs);
        //Generic way of setting typecasting object.
        T sky = (T)obj;
        fs.Flush();
        fs.Close();
        fs.Dispose();
        return sky;
    }

    public void PrintAllStars(DataGridView list)
    {
        foreach (Star star in TheSky)
        {
            list.Rows.Add(
                star.Name,
                star.ConstellationName,
                star.Magnitude.ToString("0.00##"),
                star.Distance.ToString(),
                star.Ascension.ToString("0#ч ##м ##.00с"),
                star.Declension > 0 ? star.Declension.ToString("+0#° ##` ##.00``") : star.Declension.ToString("-0#° ##` ##.00``"));
        }
        
    }

    public void Remove(string name)
    {
        string filename = "skyData.dat";

        TheSky.RemoveAll(x => x.Name == name);
        Serialize(TheSky, filename);
    }

    public void FindBrightestStar(string constellationName, MetroLabel Brightest)
    {
        var sky = TheSky.Where(x => x.ConstellationName == constellationName);
        var max = sky.Max(x => x.Magnitude);
        var item = sky.First(x => x.Magnitude == max);

        Brightest.Text = item.Name;
    }

    public void PrintConstellation(DataGridView list,string constellationName)
    {
        var sky = TheSky.Where(x => x.ConstellationName == constellationName);

        foreach (Star star in sky)
        {
            list.Rows.Add(
                star.Name,
                star.Magnitude.ToString("0.00##"),
                star.Distance.ToString(),
                star.Ascension.ToString("0#ч ##м ##.00с"),
                star.Declension > 0 ? star.Declension.ToString("+0#° ##` ##.00``") : star.Declension.ToString("-0#° ##` ##.00``"));
        }

    }

    public bool isVisible(Star star, double Lon, double Lat, double Year, double Month, double Day, int UT)
    {
        
        double RA = star.Ascension;
        double DEC = star.Declension;        

        if (Month <= 2)
        {
            Month = Month + 12;
            Year = Year - 1;
        }

        double Var = Year / 400 - Year / 100 + Year / 4;
        double Var2 = (365 * Year) - 679004;

        double MD = Var + Var2 + 306001 * (Month + 1) / 10000 + Day;//модифицированной  юлианской даты на начало суток MD
        MD = (int)MD;

        Console.WriteLine("MD = " + MD);

        double TO = (MD - 51544.5) / 36525;//мод.юл.дата на начало суток в юлианских столетиях
        double a1 = 24110.54841;
        double a2 = 8640184.812;
        double a3 = 0.093104;
        double a4 = 0.0000062;
        double SO = a1 + a2 * TO + a3 * TO * TO - a4 * TO * TO * TO; //звездное время в Гринвиче на начало суток в секундах


        double Nsec = UT * 3600;//количество секунд, прошедших  от начала суток до момента наблюдения


        double NsecS = Nsec * 366.2422 / 365.2422; //количество звездных секунд, прошедших от начала суток


        double SG = (SO + NsecS) / 3600 * 15;//гринвическое среднее звездное время в градусах

        double ST = SG + Lon;
        ST -= (int)(ST / 360) * 360; //местное звездное время, среднее


        double TH = ST - RA + 360;//часовой угол
                                  //TH += (int)(TH/360)*360;


        double z = Math.Acos(Math.Sin(Lat * Math.PI / 180) * Math.Sin(DEC * Math.PI / 180) + Math.Cos(Lat * Math.PI / 180) * Math.Cos(DEC * Math.PI / 180) * Math.Cos(TH * Math.PI / 180));
        double H = 90 - z * 180 / Math.PI;


        return H > 0 ? true : false;
    }

    public void PrintVisible(DataGridView list, double Lon, double Lat, double Year, double Month, double Day, int UT)
    {
        var sky = TheSky.Where(x => isVisible(x,Lon,Lat,Year,Month,Day,UT));

        foreach (Star star in sky)
        {
            list.Rows.Add(
                star.Name,
                star.ConstellationName,
                star.Magnitude.ToString("0.00##"),
                star.Distance.ToString(),
                star.Ascension.ToString("0#ч ##м ##.00с"),
                star.Declension > 0 ? star.Declension.ToString("+0#° ##` ##.00``") : star.Declension.ToString("-0#° ##` ##.00``"));
        }

}

    public void ShowAllStars(ComboBox box)
    {
        foreach (Star star in TheSky)
        {
            box.Items.Add(star.Name);
        }
    }

    public void GetStarProp(
        ComboBox box,
        MetroLabel LableName,
        MetroLabel LableConstellationName,
        MetroLabel LableMagnitude,
        MetroLabel LableDistance,
        MetroLabel LableAscension,
        MetroLabel LableDeclension,
        DataGridView grid,
        MetroLabel Lable,
        MetroLabel LableB)
    {
        Star star = TheSky[box.SelectedIndex];
        LableName.Text = star.Name;
        LableConstellationName.Text = star.ConstellationName;
        LableMagnitude.Text = star.Magnitude.ToString("0.00##");
        LableDistance.Text = star.Distance.ToString();
        LableAscension.Text = star.Ascension.ToString("0#ч ##м ##.00с");
        LableDeclension.Text = star.Declension > 0 ? star.Declension.ToString("+0#° ##` ##.00``")
                                                   : star.Declension.ToString("-0#° ##` ##.00``");

        grid.Rows.Clear();

        PrintConstellation(grid, star.ConstellationName);
        FindBrightestStar(star.ConstellationName, LableB);

        LableName.Visible = true;
        LableConstellationName.Visible = true;
        LableMagnitude.Visible = true;
        LableDistance.Visible = true;
        LableAscension.Visible = true;
        LableDeclension.Visible = true;
        Lable.Visible = true;
        LableB.Visible = true;

        grid.Visible = true;

    }

    public void Check1(
        MaskedTextBox box1,
        MaskedTextBox box2,
        MaskedTextBox box3,
        MaskedTextBox box4,
        MaskedTextBox box5,
        MaskedTextBox box6,
        DataGridView grid)
    {
        if (box1.MaskCompleted
                && box1.MaskCompleted
                && box2.MaskCompleted
                && box3.MaskCompleted
                && box4.MaskCompleted
                && box5.MaskCompleted
                && box6.MaskCompleted)
        {
            if (Add(new Star(
                box1.Text,
                box2.Text,
                Convert.ToDouble(box3.Text),
                Convert.ToDouble(box4.Text),
                Convert.ToDouble(box5.Text),
                Convert.ToDouble(box6.Text))
                ))
            {
                grid.Rows.Clear();
                PrintAllStars(grid);

                box1.Text = "";
                box2.Text = "";
                box3.Text = "";
                box4.Text = "";
                box5.Text = "";
                box6.Text = "";

                MessageBox.Show(
                "Вы успешно добавили звезду!",
                "Внимание!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
            
        }
        else
        {
            MessageBox.Show(
            "Заполните все поля правильно!",
            "Внимание!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Hand);
        }
    }

    public void Check2(
        MaskedTextBox box1,
        MaskedTextBox box2,
        MaskedTextBox box3,
        MaskedTextBox box4,
        MaskedTextBox box5,
        MaskedTextBox box6,
        DataGridView grid)
    {
        if (box1.MaskCompleted
                 && box2.MaskCompleted
                 && box3.MaskCompleted
                 && box2.MaskCompleted
                 && box5.MaskCompleted
                 && box6.MaskCompleted
                 && Convert.ToDouble(box2.Text) <= 12
                 && Convert.ToDouble(box3.Text) <= 31
                 && Math.Abs(Convert.ToDouble(box2.Text)) <= 14
                 && Math.Abs(Convert.ToDouble(box5.Text)) <= 90
                 && Math.Abs(Convert.ToDouble(box5.Text)) <= 90
               )

        {

            PrintVisible(
                 grid,
                 Convert.ToDouble(box5.Text),
                 Convert.ToDouble(box6.Text),
                 Convert.ToDouble(box1.Text),
                 Convert.ToDouble(box2.Text),
                 Convert.ToDouble(box3.Text),
                 Convert.ToInt32(box2.Text)
            );

        }
        else
        {
            MessageBox.Show(
            "Заполните все поля правильно!",
            "Внимание!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Hand);
        }
    }
}


