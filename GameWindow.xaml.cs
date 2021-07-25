using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
       private readonly int _width;
       private readonly int _height;
       private readonly int _cellSize;
       private readonly int[,] cells;
       private bool isXTurn = true;
       private readonly int numToWin;
       private bool GameEnd = false;
        public GameWindow(int width,int height,int numToWin, int cellSize)
        {
            InitializeComponent();
            this._width = width;
            this._height = height;
            this.cells = new int[width,height];
            this.numToWin = numToWin;
            this._cellSize = cellSize;
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    cells[i, j] = 0;
                }
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            canvas1.Width = _width * _cellSize;
            canvas1.Height = _height * _cellSize;
            for (int x = 0, left = 0; x < _width; x++, left += _cellSize)
            {
                for (int y = 0, top = 0; y < _height; y++, top += _cellSize)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = _cellSize,
                        Height = _cellSize,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };
                    canvas1.Children.Add(rect);
                    Canvas.SetLeft(rect, left);
                    Canvas.SetTop(rect, top);

                    TextBlock tb = new TextBlock
                    {
                        Text = $"{x};{y}",
                        Foreground = Brushes.DarkRed
                    };
                    canvas1.Children.Add(tb);
                    Canvas.SetLeft(tb, left+5);
                    Canvas.SetTop(tb, top);
                }
            }
        }

        private void Canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point pos = Mouse.GetPosition(canvas1);
            int x = Convert.ToInt32(pos.X)/_cellSize;
            int y = Convert.ToInt32(pos.Y)/_cellSize;
            Move(x, y);
        }
        private void Move(int x,int y)
        {
            if (GameEnd)
                return;

            if (x > _width || y > _height)
                return;
            if (cells[x, y] != 0)
                return;

            cells[x, y] = isXTurn?1:-1;
            if (isXTurn)
                DrawX(x, y);
            else
                DrawO(x, y);

            CheckForWinOrDraw(x, y);

            isXTurn = !isXTurn;

        }
        private void CheckForWinOrDraw(int x,int y)
        {
            bool win = CheckSides(x, y) || CheckVertical(x, y) || CheckDiagonals(x, y);

            //Check if all cells non-zero
            bool draw=cells.Cast<int>().All(c=>c!=0);
            
            GameEnd = win||draw;
            if (win)
                MessageBox.Show("Winner: "+(isXTurn?"X":"O"));
            else if (draw)
                MessageBox.Show("Draw");

        }

        private bool CheckSides(int colId,int rowId)
        {
            int X_or_O = isXTurn ? 1 : -1;
            int[] row = new int[numToWin];
            for(int left = colId - (numToWin-1),right=colId; right<=colId+(numToWin-1)&&right<_width; left++,right++)
            {
                if (left < 0 || right > _width-1)
                    continue;
                for (int i = 0; i < row.Length; i++)
                    row[i] = cells[left+i, rowId];
                if (row.All(c => c == X_or_O))
                    return true;
            }
            return false;
        }
        private bool CheckVertical(int colId,int rowId)
        {
            int X_or_O = isXTurn ? 1 : -1;
            int[] col = new int[numToWin];
            for(int left = rowId - (numToWin - 1), right = rowId; right <= rowId + (numToWin - 1) && right < _height; left++, right++)
            {
                if (left < 0 || right > _height - 1)
                    continue;
                for (int i = 0; i < col.Length; i++)
                    col[i] = cells[colId, left + i];
                if (col.All(c => c == X_or_O))
                    return true;
            }


            return false;
        }
        private bool CheckDiagonals(int colId, int rowId)
        {
            int X_or_O = isXTurn ? 1 : -1;
            int[] diag = new int[numToWin];

            int leftX = colId - (numToWin - 1);
            int leftY = rowId - (numToWin - 1);
            int rightX = colId;
            int rightY = rowId;


            leftX -= 1;leftY -= 1;rightX -= 1;rightY -= 1; //Because i need increment in the beginning of cycle
            while (rightX < colId + (numToWin - 1) && rightX < _width && rightY < rowId + (numToWin - 1) && rightY < _height)
            {
                leftX++;leftY++;rightX++;rightY++;
              
                if (leftX < 0 || leftY < 0 || rightX > _width-1 || rightY > _height-1)
                    continue;
                for (int i = 0; i < diag.Length; i++)
                    diag[i] = cells[leftX + i, leftY + i];
                if (diag.All(c => c == X_or_O))
                    return true;
            }

            leftX = colId;
            rightX = colId + (numToWin - 1);
            leftY = rowId;
            rightY = rowId - (numToWin - 1);

            for (; leftX >= 0 && leftY < _height && rightX >= colId && leftY >= rowId; leftX--, rightX--, leftY++, rightY++)
            {
                if (leftX<0 || leftY <0 || rightX >=_width || rightY >=_height||rightY<0)
                    continue;
                for (int i = 0; i < diag.Length; i++)
                    diag[i] = cells[rightX - i, rightY + i];
                if (diag.All(c => c == X_or_O))
                    return true;
            }


            return false;
        }

        private void DrawX(int x,int y)
        {
            Line line1 = new Line
            {
                X1 = x * _cellSize,
                Y1 = y * _cellSize,
                X2 = x * _cellSize + _cellSize,
                Y2 = y * _cellSize + _cellSize,
                Stroke = Brushes.Black,
                StrokeThickness = 1.5D
            };

            Line line2 = new Line
            {
                X1 = x * _cellSize + _cellSize,
                Y1 = y * _cellSize,
                X2 = x * _cellSize,
                Y2 = y * _cellSize + _cellSize,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            canvas1.Children.Add(line1);
            canvas1.Children.Add(line2);


        }
        private void DrawO(int x,int y)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = ellipse.Height = _cellSize;
            ellipse.Stroke = Brushes.Black;
            ellipse.StrokeThickness = 1.5D;
            canvas1.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, x * _cellSize);
            Canvas.SetTop(ellipse, y * _cellSize);

        }
    }
}
