using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameInWinForms.Controllers
{
    public static class Field2
    {
        public static int[,] Map1 = new int[MapControllers.MapSize, MapControllers.MapSize];//поле 1, значение с помошью которых будем обозначать картинки

        public static Button[,] MapButtons1 = new Button[MapControllers.MapSize, MapControllers.MapSize];

        public static bool isFirstStep1;//проверяем наличие первого нажатия на ячейку

        public static int actualPicture1;//текущая картинка, нужна сейчас для нажатия правой кнопки

        public static Point FirstCor1;//координаты поля 1

        public static Form form;//форма нашей программы

        public static int CountList2;

        public static Label Text2 = new Label();

        public static int OpenPoints2;

        /// <summary>
        /// инициализируем текст листов
        /// </summary>
        /// <param name="current"></param>
        public static void InitializeTextList2(Form current)
        {
            Text2.Location = new Point(416, 370);
            Text2.ForeColor = Color.Black;
            Text2.Font = new Font(Text2.Font.FontFamily, 24, Text2.Font.Style);
            Text2.Size = new Size(50, 35);
            Text2.BackColor = Color.White;
            Text2.Text = $"{CountList2}";
            current.Controls.Add(Text2);
        }

        /// <summary>
        /// иинициализирует кнопки 2 поля
        /// </summary>
        /// <param name="CellSize">размер кнопки</param>
        /// <param name="current">форма</param>
        public static void InitializeButtons2(int CellSize, Form current)
        {
            for (var i = 0; i < MapControllers.MapSize; i++)
            {
                for (var j = 0; j < MapControllers.MapSize; j++)
                {
                    var button = new Button();
                    button.Location = new Point(j * CellSize + current.Width / 3 + 130, i * CellSize + current.Height / 3);
                    button.Size = new Size(CellSize, CellSize);
                    //button.Image = MapControllers.FindImage(3, 1);//тест на работоспособность картинок(флажок)
                    button.Image = MapControllers.FindImage(0, 0);
                    button.MouseUp += new MouseEventHandler(OnButtonPressedMouse);
                    current.Controls.Add(button);
                    MapButtons1[i, j] = button;
                }
            }
        }

        /// <summary>
        /// инициализирует основные компоненты 2 поля
        /// </summary>
        /// <param name="current">форма</param>
        public static void Initialize2(Form current)
        {
            OpenPoints2 = 0;
            CountList2 = 0;
            form = current;
            actualPicture1 = 0;
            isFirstStep1 = true;
        }

        /// <summary>
        /// инициализируем основу поля 2
        /// </summary>
        /// <param name="current">форма</param>
        public static void InitializeMap2()
        {
            for (var i = 0; i < MapControllers.MapSize; i++)
            {
                for (var j = 0; j < MapControllers.MapSize; j++)
                {
                    Map1[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// здесь проставляем листочки для 2 поля
        /// </summary>
        public static void SeedMap()
        {
            Random random = new Random();
            var number = random.Next(6, 10);//количество листочков

            for (var i = 0; i < number; i++)
            {
                var positionI = random.Next(0, MapControllers.MapSize - 1);
                var positionJ = random.Next(0, MapControllers.MapSize - 1);
                //здесь проверяем, чтобы листочек не появлялся в месте, где лежит листочек
                while (Map1[positionI, positionJ] == -1 || (Math.Abs(positionI - FirstCor1.Y) <= 1 && Math.Abs(positionJ - FirstCor1.X) <= 1) || Field1.Map1[positionI, positionJ] == -1)
                {
                    positionI = random.Next(0, MapControllers.MapSize - 1);
                    positionJ = random.Next(0, MapControllers.MapSize - 1);
                }

                Map1[positionI, positionJ] = -1;//отмечаем, что здесь уже находится "листочек"
                CountList2++;
                Text2.Text = $"{CountList2}";
            }
        }

        /// <summary>
        /// считает для каждой ячейки какое количество листочков с ней расположено
        /// </summary>
        public static void CountCellLists()
        {
            for (var i = 0; i < MapControllers.MapSize; i++)
            {
                for (var j = 0; j < MapControllers.MapSize; j++)
                {
                    if (Map1[i, j] == -1)
                    {
                        for (var k = i - 1; k < i + 2; k++)
                        {
                            for (var m = j - 1; m < j + 2; m++)
                            {
                                if (!MapControllers.IsInTheBorder(k, m) || Map1[k, m] == -1)
                                    continue;
                                Map1[k, m] = Map1[k, m] + 1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// открывает все ячейки рядом
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void OpenCells(int i, int j)
        {
            OpenCell(i, j);
            OpenPoints2++;
            if (Map1[i, j] > 0)
                return;
            for (var k = i - 1; k < i + 2; k++)
            {
                for (var m = j - 1; m < j + 2; m++)
                {
                    if (!MapControllers.IsInTheBorder(k, m))
                        continue;
                    if (!MapButtons1[k, m].Enabled)
                        continue;
                    if (Map1[k, m] == 0)
                        OpenCells(k, m);
                    else if (Map1[k, m] > 0)
                    {
                        OpenCell(k, m);
                        OpenPoints2++;
                    }
                }
            }
            MapControllers.CheckWin();
        }

        /// <summary>
        /// здесь происходит действия нажатия кнопки мышкой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnButtonPressedMouse(object sender, MouseEventArgs e)
        {
            Button pressedButton = sender as Button;//нажатая кнопка
            switch (e.Button.ToString())
            {
                case "Right":
                    OnRightButtonPressed(pressedButton);
                    break;
                case "Left":
                    OnLeftButtonPressed(pressedButton);
                    break;
            }
            MapControllers.CheckWin();
            Text2.Text = $"{CountList2}";
        }

        /// <summary>
        /// нажатие кнопки правой клавишой
        /// </summary>
        /// <param name="pressedButton">нажатая кнопка</param>
        private static void OnRightButtonPressed(Button pressedButton)
        {
            actualPicture1++;
            actualPicture1 %= 2;

            var positionX = 0;
            var positionY = 0;
            switch (actualPicture1)
            {
                //чистая картинка
                case 0:
                    positionX = 0;
                    positionY = 0;
                    break;
                //флажок
                case 1:
                    positionX = 3;
                    positionY = 1;
                    break;
            }
            pressedButton.Image = MapControllers.FindImage(positionX, positionY);
        }

        /// <summary>
        /// нажатие кнопки левой клавишой
        /// </summary>
        /// <param name="pressedButton"></param>
        private static void OnLeftButtonPressed(Button pressedButton)
        {
            pressedButton.Enabled = false;
            var buttonI = pressedButton.Location.Y / MapControllers.CellSize - (form.Height / 3) / MapControllers.CellSize;
            var buttonJ = pressedButton.Location.X / MapControllers.CellSize - (form.Width / 3 + 130) / MapControllers.CellSize;
            //это чтобы, при первом нажатии не попасть по листочку
            if (isFirstStep1)
            {
                FirstCor1 = new Point(buttonJ, buttonI);
                SeedMap();
                CountCellLists();
                isFirstStep1 = false;
            }
            OpenCells(buttonI, buttonJ);
            if (Map1[buttonI, buttonJ] == -1)
            {
                ShowAllLists(buttonI, buttonJ);
                Field1.ShowAllLists(buttonI, buttonJ);
                MessageBox.Show("Игра Окончена");
                form.Controls.Clear();//зачищаем форму
                MapControllers.Initialize(form);
            }
        }

        /// <summary>
        /// открывает ячейку
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void OpenCell(int i, int j)
        {
            MapButtons1[i, j].Enabled = false;

            switch (Map1[i, j])
            {
                case 1:
                    MapButtons1[i, j].Image = MapControllers.FindImage(1, 0);
                    break;
                case 2:
                    MapButtons1[i, j].Image = MapControllers.FindImage(2, 0);
                    break;
                case 3:
                    MapButtons1[i, j].Image = MapControllers.FindImage(3, 0);
                    break;
                case 4:
                    MapButtons1[i, j].Image = MapControllers.FindImage(4, 0);
                    break;
                case 5:
                    MapButtons1[i, j].Image = MapControllers.FindImage(5, 0);
                    break;
                case 6:
                    MapButtons1[i, j].Image = MapControllers.FindImage(0, 1);
                    break;
                case 7:
                    MapButtons1[i, j].Image = MapControllers.FindImage(1, 1);
                    break;
                case 8:
                    MapButtons1[i, j].Image = MapControllers.FindImage(2, 1);
                    break;
                case -1:
                    MapButtons1[i, j].Image = MapControllers.FindImage(4, 1);
                    break;
                case 0:
                    MapButtons1[i, j].Image = MapControllers.FindImage(0, 0);
                    break;
            }
        }

        /// <summary>
        /// показывает все остальные листы листы
        /// </summary>
        /// <param name="listI">координаты листа по i</param>
        /// <param name="listJ">координаты листа по j</param>
        public static void ShowAllLists(int listI, int listJ)
        {
            for (var i = 0; i < MapControllers.MapSize; i++)
            {
                for (var j = 0; j < MapControllers.MapSize; j++)
                {
                    if (i == listI && j == listJ)
                        continue;
                    if (Map1[i, j] == -1)
                    {
                        MapButtons1[i, j].Image = MapControllers.FindImage(5, 1);
                    }
                }
            }
        }
    }
}
