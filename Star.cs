using System;

[Serializable]
public class Star
{
    string StarName; //название звезды

    string StarConstellationName; //название созвездия

    double StarMagnitude;  //звёздная величина

    double StarDistance;  //расстояние до звезды

    double StarAscension;  //прямое восхождение

    double StarDeclension; //склонение

    public Star(
        string StarName,
        string StarConstellationName,
        double StarMagnitude,
        double StarDistance,
        double StarAscension,
        double StarDeclension)
	{
        Name = StarName;
        ConstellationName = StarConstellationName;
        Magnitude = StarMagnitude;
        Distance = StarDistance;
        Ascension = StarAscension;
        Declension = StarDeclension;
    }

    public string Name
    {
        set
        {
            StarName = value;
        }
        get
        {
            return StarName;
        }
    }

    public string ConstellationName
    {
        set
        {
            StarConstellationName = value;
        }
        get
        {
            return StarConstellationName;
        }
    }

    public double Magnitude
    {
        set
        {
            StarMagnitude = value;
        }
        get
        {
            return StarMagnitude;
        }
    }

    public double Distance
    {
        set
        {
            StarDistance = value;
        }
        get
        {
            return StarDistance;
        }
    } 

    public double Ascension
    {
        set
        {
            StarAscension = value;
        }
        get
        {
            return StarAscension;
        }
    }

    public double Declension
    {
        set
        {
            StarDeclension = value;
        }
        get
        {
            return StarDeclension;
        }
    }
}
