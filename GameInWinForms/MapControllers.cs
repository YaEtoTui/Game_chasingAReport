using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class MapControllers
{
	public const int MapSize = 8;//размер поля

	public const int CellSize = 26;//размер ячейки

	public static int[,] Map = new int[MapSize, MapSize];

	public static Button[,] MapButtons = new Button[MapSize, MapSize];

    public static void Initialize(Form current)
    {
        ConfigureMapSize(current);
        InitializeMap();
        InitializeButtons(current);
    }

    public static void ConfigureMapSize(Form current)
    {
        //размер карты
        current.Width = MapSize * CellSize * 3;
        current.Height = (MapSize + 1) * CellSize * 2;

    }

    public static void InitializeMap(Form current)
    {
        for (var i = 0; i < MapSize; i++)
        {
            for (var j = 0; j < MapSize; j++)
            {
                Map[i, j] = 0;
            }
        }
    }

    public static void InitializeButtons(Form current)
    {
        for (var i = 0; i < MapSize; i++)
        {
            for (var j = 0; j < MapSize; j++)
            {
                var button = new Button();
                button.Location = new Point(j * CellSize + Width / 3 - 125, i * CellSize + Height / 3);
                button.Size = new Size(CellSize, CellSize);
                current.Controls.Add(button);
                MapButtons[i, j] = button;
            }
        }
    }
}
