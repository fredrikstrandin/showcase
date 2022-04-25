using TestdataGenerator.Model;

namespace TestdataGenerator;

public class Personnummer
{
    private string _pnr;
    private int _century;

    public Personnummer(string pnr)
    {
        //TODO: check if pnr isvaild
        pnr = pnr.Replace("-", "");

        if(pnr.Length == 12)
        {
            int.TryParse(pnr.Substring(0, 2), out _century);
         }

        if(pnr.Length == 10)
        {
            if(int.TryParse(pnr.Substring(0, 2), out int year))
            {
                if(year < DateTime.Now.Year)
                {
                    _century = 20;
                }
                else
                {
                    _century = 19;
                }
            }

            pnr = $"{_century}{pnr}";
        }

        _pnr = pnr;
    }

    public DateOnly Born
    {
        get
        {
            int year = int.Parse(_pnr.Substring(0, 4));
            int mounth = int.Parse(_pnr.Substring(4, 2));
            int day = int.Parse(_pnr.Substring(6, 2));

            return new DateOnly(year, mounth, day);
        }
    }

    public bool IsAdult
    {
        get
        {
            int year = int.Parse(_pnr.Substring(0, 4));
            int mounth = int.Parse(_pnr.Substring(4, 2));
            int day = int.Parse(_pnr.Substring(6, 2));

            return new DateTime(year, mounth, day) < DateTime.Now.AddYears(-18);
        }
    }

    public bool IsWoman
    {
        get
        {
            int checkno = int.Parse(_pnr.Substring(11, 1));

            return checkno % 2 == 0;
        }
    }
    public bool IsMaile
    {
        get
        {
            int checkno = int.Parse(_pnr.Substring(11, 1));

            return checkno % 2 == 1;
        }
    }

    public Gender Gender
    {
        get
        {
            int checkno = int.Parse(_pnr.Substring(11, 1));

            return (Gender)(checkno % 2);
        }
    }
    public override string ToString()
    {
        return $"{_pnr.Substring(2, 8)}-{_pnr.Substring(8, 4)}";
    }
}
