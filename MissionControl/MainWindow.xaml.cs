using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MissionControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        USBReader reader;
        Graph accGraph;
        float[] accGraphData;

        public MainWindow()
        {
            InitializeComponent();
            accGraph = new Graph(DrawCanvas, 100);

            for (float i = 0; i <= accGraph.DATA_SIZE / 10; i++) //Create the grid
            {
                drawLine(GridCanvas, i * 10 * accGraph.dataSpace, i * 10 * accGraph.dataSpace, 0, (float)GridCanvas.Height, Brushes.LightGray);
                drawLine(GridCanvas, 0, (float)GridCanvas.Width, i * (float)GridCanvas.Height / 10, i * (float)GridCanvas.Height / 10,  Brushes.LightGray);

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            reader = new USBReader(9600);

            if (reader.openPort())
            {
                ConsoleBox.Text += "Port opened\n";
                Thread thread = new Thread(getData);
                thread.Start();
            }
            else
            {
                ConsoleBox.Text += "Failed to open port\n";
            }
        }

        private void getData()
        {
            while (true)
            {
                while ((accGraphData = reader.readData()) != null)
                {
                    
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (accGraphData[3] == 1)
                        {
                            accGraph.addData(accGraphData);
                            DrawCanvas.Children.Clear();
                           
                            float spacer = 0;

                            for (int i = 1; i < accGraph.DATA_SIZE; i++)
                            {
                                drawLine(DrawCanvas, spacer, spacer + accGraph.dataSpace, accGraph.data[0][i - 1], accGraph.data[0][i], Brushes.Red);
                                drawLine(DrawCanvas, spacer, spacer + accGraph.dataSpace, accGraph.data[1][i - 1], accGraph.data[1][i], Brushes.LimeGreen);
                                drawLine(DrawCanvas, spacer, spacer + accGraph.dataSpace, accGraph.data[2][i - 1], accGraph.data[2][i], Brushes.Blue);

                                spacer += accGraph.dataSpace;
                            }

                            updateText();
                        }
                        else if (accGraphData[3] == 2)
                        {
                            RocketX.RenderTransform = new RotateTransform(accGraphData[0]/-20 + 90);
                            RocketY.RenderTransform = new RotateTransform(accGraphData[1] / -20 + 90);

                            RollBar.Children.Clear();
                            drawBar(RollBar, normalizeBar(accGraphData[2], 0, 1000, RollBar), Colors.LightGray);

                        }

                    }));
                    Thread.Sleep(20);
                }
            }
        }

        private void updateText()
        {
            //AccX.Content = accGraphData[0];
            //AccY.Content = accGraphData[1];
            //AccZ.Content = accGraphData[2];
        }

        private void drawLine(Canvas graph, float x1, float x2, float y1, float y2, Brush color)
        {
            Line line = new Line();
            line.Stroke = color; ;
            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;
            line.StrokeThickness = 1;
            graph.Children.Add(line);
        }   
        
        private float normalizeBar(float value, int min, int max, Canvas bar)
        {
            return (float)(((value - min) * (bar.Width - 0)) / (max - min));
        }

        private void drawBar(Canvas bar, float value, Color color)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(color);
            if (value < 0) { value *= -1; }
            rect.Width = value;
            rect.Height = bar.Height;
            bar.Children.Add(rect);
            Canvas.SetTop(rect, 0);
            Canvas.SetLeft(rect, 0);
        }
    }
}
