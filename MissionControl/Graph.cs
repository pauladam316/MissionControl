using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MissionControl
{
    class Graph
    {
        Canvas canvas;
        private int _MAX_WIDTH;
        private int _MAX_HEIGHT;

        public int DATA_SIZE;
        public float dataSpace;
        public float[][] data;

        public Graph(Canvas canvas, int dataSize)
        {
            this.canvas = canvas;
            _MAX_WIDTH = (int)canvas.Width;
            _MAX_HEIGHT = (int)canvas.Height;
            DATA_SIZE = dataSize;
            data = new float[3][];
            data[0] = new float[DATA_SIZE];
            data[1] = new float[DATA_SIZE];
            data[2] = new float[DATA_SIZE];
            for (int i = 0; i < DATA_SIZE; i++)
            {
                data[0][i] = 0;
                data[1][i] = 0;
                data[2][i] = 0;
            }
            dataSpace = (float)_MAX_WIDTH /(float)DATA_SIZE;
        }

        public void addData(float[] data)
        {
            for (int i = DATA_SIZE-1; i > 0; i--)
            {
                this.data[0][i] = this.data[0][i - 1];
                this.data[1][i] = this.data[1][i - 1];
                this.data[2][i] = this.data[2][i - 1];

            }
            this.data[0][0] = (((data[0] - -2f) * (((float)_MAX_HEIGHT / 2f) - ((float)_MAX_HEIGHT / -2))) / (2f - -2f)) + (float)_MAX_HEIGHT / -2f  + 50f;
            this.data[1][0] = (((data[1] - -2f) * (((float)_MAX_HEIGHT / 2f) - ((float)_MAX_HEIGHT / -2))) / (2f - -2f)) + (float)_MAX_HEIGHT / -2f  + 50f;
            this.data[2][0] = (((data[2] - -2f) * (((float)_MAX_HEIGHT / 2f) - ((float)_MAX_HEIGHT / -2))) / (2f - -2f)) + (float)_MAX_HEIGHT / -2f  + 50f;
        }



    }
}
