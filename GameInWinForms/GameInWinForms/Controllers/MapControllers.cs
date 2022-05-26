using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameInWinForms.Controllers
{
    public static class MapControllers
    {
        public const int MapSize = 8;//размер поля

        public const int CellSize = 26;//размер ячейки

        public static Image Sprites;//здесь хранятся наши главные картинки

        public static Form form;

        private static Panel panelEducation;

        private static Panel panelStory;

        private static int numberEducation;//чтобы закрывать или открывать панель обучения

        private static int numberStory;//чтобы закрывать или открывать панель сюжета

        /// <summary>
        /// инициализирует основные компоненты программы
        /// </summary>
        /// <param name="current">форма</param>
        public static void Initialize(Form current)
        {
            numberEducation = 0;
            numberStory = 0;
            current.MaximizeBox = false;//блокируем кнопку включение большеэкранного режима
            form = current;
            ClickStory();
            ClickNewGame();
            ClickEducation(current);
            Field1.Initialize1(current);
            Field2.Initialize2(current);
            Sprites = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\basicCells.png"));
            ConfigureMapSize(current);
            InitializeMap();
            InitializeButtons(current);
            InitializeTextList(current);
            current.Text = "Погоня за Отчётом";
            
        }

        /// <summary>
        /// инициализируем панель ОБучения
        /// </summary>
        public static void InitializePanelEducation()
        {
            panelEducation = new Panel();
            panelEducation.BackColor = Color.White;
            panelEducation.Location = new Point(15, 22);
            panelEducation.Size = new Size(200, 175);
            form.Controls.Add(panelEducation);
            var text = new Label();
            text.Size = new Size(200, 170);
            text.Text = @"
1)Левая кнопка мыши: “сделать шаг”.
2)	Правая кнопка мыши: отметить “флажком”, предполагаемое место нахождения листа.
Ниже каждого поля указано, сколько нужно найти листов(сначала нужно нажать на каждое поле).
Цель: открыть все клетки в двух полях.
Если в одной клетке окажется лист, то в другом поле в той же самой клетке не должно оказаться листа.
            ";
            panelEducation.Controls.Add(text);
        }

        /// <summary>
        /// Иницализируем панель Сюжета
        /// </summary>
        public static void InitializePanelStory()
        {
            panelStory = new Panel();
            panelStory.BackColor = Color.White;
            panelStory.Location = new Point(15, 22);
            panelStory.Size = new Size(200, 200);
            form.Controls.Add(panelStory);
            var textStory = new Label();
            textStory.Size = new Size(200, 300);
            textStory.Text = @"
Наша история повествует о студенте, подготовивший отчёт для сдачи проекта. Но неожиданно подул сильный ветер, который унес все листы отчета.
Нужно помочь студенту найти отчёт и спасти его от летних пересдач.
Для разнообразия игры автор решил связать теорию Мультивселенной. И теперь ваша задача помочь ДВУМ студентам из разных реальностей.
"; 
            panelStory.Controls.Add(textStory);
        }

        /// <summary>
        /// Кликать по кнопке Сюжета
        /// </summary>
        public static void ClickStory()
        {
            InitializePanelStory();
            panelStory.Hide();
            var buttonStory = new Button();
            buttonStory.Text = "Сюжет";
            buttonStory.Location = new Point(146, 0);
            buttonStory.MouseClick += new MouseEventHandler(OnButtonPressedMouseStory);//нажатие кнопки обучение
            form.Controls.Add(buttonStory);
            var lip = new ToolTip();//подсказка при наведении мышки на кнопку Обучение
            lip.SetToolTip(buttonStory, "Открывать и закрывать - левой клавишой");

        }

        /// <summary>
        /// Нажатие на кнопку Сюжета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnButtonPressedMouseStory(object sender, MouseEventArgs e)
        {
            if (numberStory == 0)
            {
                panelStory.Show();
                numberStory = 1;
            }
            else
            {
                panelStory.Hide();
                numberStory = 0;
            }
        }

        /// <summary>
        /// Кликнуть кнопку Новая игра
        /// </summary>
        public static void ClickNewGame()
        {
            var buttonNewGame = new Button();
            buttonNewGame.Text = "Новая игра";
            buttonNewGame.Location = new Point(0, 0);
            buttonNewGame.MouseClick += new MouseEventHandler(OnButtonPressedMouseNewGame);//нажатие кнопки обучение
            form.Controls.Add(buttonNewGame);
        }

        /// <summary>
        /// Нажатие на кнопку Новая игра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnButtonPressedMouseNewGame(object sender, MouseEventArgs e)
        {
            form.Controls.Clear();//зачищаем форму
            Initialize(form);
        }

        /// <summary>
        /// Кликнуть кнопку Обучения
        /// </summary>
        /// <param name="current"></param>
        public static void ClickEducation(Form current)
        {
            InitializePanelEducation();
            panelEducation.Hide();
            var buttonEducation = new Button();
            buttonEducation.Text = "Обучение";
            buttonEducation.Location = new Point(73, 0);
            buttonEducation.MouseClick += new MouseEventHandler(OnButtonPressedMouseEducation);//нажатие кнопки обучение
            current.Controls.Add(buttonEducation);
            var lip = new ToolTip();//подсказка при наведении мышки на кнопку Обучение
            lip.SetToolTip(buttonEducation, "Открывать и закрывать - левой клавишой");
        }

        /// <summary>
        /// открывает панель с обучением
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnButtonPressedMouseEducation(object sender, MouseEventArgs e)
        {
            if (numberEducation == 0)
            {
                panelEducation.Show();
                numberEducation = 1;
            }
            else
            {
                panelEducation.Hide();
                numberEducation = 0;
            }
        }

        /// <summary>
        /// Задаем размер формы
        /// </summary>
        /// <param name="current">форма</param>
        private static void ConfigureMapSize(Form current)
        {
            //размер формы
            current.Width = MapSize * CellSize * 3;
            current.Height = (MapSize + 1) * CellSize * 2;

        }

        /// <summary>
        /// инициализируем основу поля
        /// </summary>
        /// <param name="current">форма</param>
        private static void InitializeMap()
        {
            Field1.InitializeMap1();
            Field2.InitializeMap2();
        }

        public static void InitializeTextList(Form current)
        {
            Field1.InitializeTextList1(current);
            Field2.InitializeTextList2(current);
        }

        /// <summary>
        /// инициализирует кнопки для 1 и 2 полей
        /// </summary>
        /// <param name="current">форма</param>
        private static void InitializeButtons(Form current)
        {
            Field1.InitializeButtons1(CellSize, current);
            Field2.InitializeButtons2(CellSize, current);
        }

        /// <summary>
        /// ищем картинку с основными ячейками
        /// </summary>
        /// <param name="xPosition">позиция по X</param>
        /// <param name="yPosition">позиция по Y</param>
        /// <returns>возвращает картинку</returns>
        public static Image FindImage(int xPosition, int yPosition)
        {
            Image image = new Bitmap(CellSize, CellSize);//сама картинка
            Graphics graphics = Graphics.FromImage(image);//создаем графику из картинки
            graphics.DrawImage(Sprites, new Rectangle(new Point(0,0), new Size(CellSize, CellSize)), 0 + 32 * xPosition, 0 + 32 * yPosition, 33, 33, GraphicsUnit.Pixel);
            
            return image;
        }

        /// <summary>
        /// находится ли листочек вокруг нашей точки
        /// </summary>
        /// <returns></returns>
        public static bool IsInTheBorder(int i, int j)
        {
            //если вышла из поля
            if (i < 0 || j < 0 || j > MapSize - 1 || i > MapSize - 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// проверяем выиграли ли игру
        /// </summary>
        public static void CheckWin()
        {
            //если в двух полях все ячейки очищены кроме листов 
            if (MapControllers.MapSize * MapControllers.MapSize == Field1.CountList1 + Field1.OpenPoints1 && MapControllers.MapSize * MapControllers.MapSize == Field2.CountList2 + Field2.OpenPoints2)
            {
                MessageBox.Show("Победа");
                form.Controls.Clear();//зачищаем форму
                MapControllers.Initialize(form);
            }
        }
    }
}
