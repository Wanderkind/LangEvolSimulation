using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FsLib;


namespace LES
{
    public partial class Form1 : Form
    {
        private int xOffset = 0;
        private int yOffset = 0;
        private bool isRunning = false;
        private System.Timers.Timer simulationTimer;
        private const int ButtonMargin = 10;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true; // Enable double buffering for smoother drawing

            // Enable AutoScroll
            AutoScroll = true;

            // Subscribe to the Scroll event
            //Scroll += Form1_Scroll;

            // Initialize the Timer
            simulationTimer = new System.Timers.Timer();
            simulationTimer.Interval = 5; // Set the interval (adjust as needed)
            //simulationTimer.Tick += SimulationTimer_Tick;

            // Wire up the button click event handlers
            //startButton.Click += StartButton_Click;
            //stopButton.Click += StopButton_Click;
            //regenButton.Click += RegenButton_Click;
        }

        private void Form1_Scroll(object sender, ScrollEventArgs e)
        {
            // Update offsets based on scroll
            xOffset = HorizontalScroll.Value;
            yOffset = VerticalScroll.Value;

            // Redraw the grid
            Invalidate();
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            //UpdateGrid();
            Invalidate(); // Refresh the form
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            isRunning = true;
            simulationTimer.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            isRunning = false;
            simulationTimer.Stop();
        }

        private void RegenButton_Click(object sender, EventArgs e)
        {
            //InitializeGrid();
            Invalidate();
        }
    }
}