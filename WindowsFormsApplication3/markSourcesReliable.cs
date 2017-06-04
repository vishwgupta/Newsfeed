using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leap;
using System.Diagnostics;

namespace WindowsFormsApplication3
{
    public partial class markSourcesReliable : Form, ILeapEventDelegate
    {
        private dbConfig db;
        private Controller controller;
        private LeapEventListener listener;
        int count;
        List<string> sourceList;
        List<string> isReliableList;
        string reliableState = "";
        int screenWidth, screenHeight;
        Button b;
        
        List<Button> listOfButtons = new List<Button>();

        Boolean flag = true;
       
        public markSourcesReliable()
        {
            InitializeComponent();

            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            this.controller.AddListener(listener);
            this.db = new dbConfig();
            this.db.displaySourceData();
            count = this.db.getCountSource();
            sourceList = this.db.getSourceList();
            isReliableList = this.db.getReliableSourceList();
            //MessageBox.Show(Screen.FromControl(this).Bounds.Width.ToString());
           
            


            screenWidth = System.Windows.Forms.Screen.FromControl(this).Bounds.Width;
            screenHeight = System.Windows.Forms.Screen.FromControl(this).Bounds.Height;
            flowLayoutPanel1.Controls.Clear();
            for(int i=0; i<count; i++)
            {
                b = new Button();
                b.Name = sourceList[i];
                b.Text = sourceList[i];
                b.Width = System.Windows.Forms.Screen.FromControl(this).Bounds.Width / 4 - 7;
                b.Height = System.Windows.Forms.Screen.FromControl(this).Bounds.Height / 2 - 20;

                //b.Width = Leap.Screen.getCPtr().
                //b.Height = System.Windows.Forms.Screen.FromControl(this).Bounds.Height / 2 - 20;

                b.AutoSize = true;
                reliableState = isReliableList[i].ToString();
                if (reliableState == "true")
                {
                    b.BackColor = Color.LightSkyBlue;
                }
                else
                {
                    b.BackColor = Color.Gray;
                }
                //b.Click += new EventHandler(b_Click);
                flowLayoutPanel1.Controls.Add(b);
                listOfButtons.Add(b);
            }
        }

        delegate void LeapEventDelegate(string EventName);

        public void LeapEventNotification(string EventName)
        {
            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                        Debug.WriteLine("Init");
                        break;
                    case "onConnect":
                        this.connectHandler();
                        break;
                    case "onFrame":
                        if (!this.Disposing)
                            this.newFrameHandler(this.controller.Frame());
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }


        void connectHandler()
        {
            this.controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            this.controller.Config.SetFloat("Gesture.Circle.MinRadius", 40.0f);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            controller.Config.SetFloat("Gesture.ScreenTap.MinForwardVelocity", 30.0f);
            controller.Config.SetFloat("Gesture.ScreenTap.HistorySeconds", .5f);
            controller.Config.SetFloat("Gesture.ScreenTap.MinDistance", 1.0f);
            controller.Config.Save();
        }

        void newFrameHandler(Frame frame)
        {
            for (int g = 0; g < frame.Gestures().Count; g++)
            {
                switch (frame.Gestures()[g].Type)
                {
                     case Gesture.GestureType.TYPE_SCREEN_TAP:
                        //Handle screen tap gestures
                        isInside(frame.Gestures()[g]);
                        break;
                    case Gesture.GestureType.TYPE_SWIPE:
                        //Handle swipe gestures
                       
                        break;
                    default:
                        //Handle unrecognized gestures
                        
                        break;
                }
            }
        }

        private void isInside(Gesture gesture)
        {
            ScreenTapGesture screentapGesture = new ScreenTapGesture(gesture);
            Vector pokeLocation = screentapGesture.Position;

            int appWidth = screenWidth;
            int appHeight = screenHeight;

            InteractionBox iBox = controller.Frame().InteractionBox;
            Pointable pointable = controller.Frame().Pointables.Frontmost;

            Leap.Vector leapPoint = pointable.StabilizedTipPosition;
            Leap.Vector normalizedPoint = iBox.NormalizePoint(leapPoint, false);

            float appX = normalizedPoint.x * appWidth;
            float appY = (1 - normalizedPoint.y) * appHeight;


            flag = true;
                //MessageBox.Show(pokeLocation + " " + screenHeight );
                for (int i = 0; i < listOfButtons.Count; i++)
                {

                    //if (pokeLocation.x >= (listOfButtons[i].Location.X) && pokeLocation.x < (i+1)*screenWidth/4
                    //    && pokeLocation.y >= (listOfButtons[i].Location.Y) && pokeLocation.y < (i + 1) * screenHeight / 2)

                    if (appX > (listOfButtons[i].Location.X) && appX < (i + 1) * screenWidth / 4
                        && appY > (listOfButtons[i].Location.Y) && appY < (i + 1) * screenHeight / 2 && flag == true) 
                    {
                        MessageBox.Show(i + "  " + appX.ToString() + " " + appY.ToString() + " \nx:" + listOfButtons[i].Location.X + " y:" + listOfButtons[i].Location.Y + " \nxmax:" + (i + 1) * screenWidth / 4 + " ymax:" + (i + 1) * screenHeight / 2);
                        tappedButton(i);
                        flag = false;
                    }
                    else
                    {
                        //MessageBox.Show("test");
                    }
                
            }
        }


        //protected override void Dispose(bool disposing)
        //{
        //    try
        //    {
        //        if (disposing)
        //        {
        //            if (components != null)
        //            {
        //                components.Dispose();
        //            }
        //            this.controller.RemoveListener(this.listener);
        //            this.controller.Dispose();
        //        }
        //    }
        //    finally
        //    {
        //        base.Dispose(disposing);
        //    }
        //}

        public static bool IsWithinRangeOfCloud(int x, int y)
        {
            return (Enumerable.Range(150, 300).Contains(x)
                    && Enumerable.Range(140, 400).Contains(y));
        }

        //private void b_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show(" I am being called");
        //    try
        //    {
        //        Button b = sender as Button;
        //        this.db.updateSourceTable(b.Name.ToString());
        //        reliableState = this.db.getReliableState();
        //        if(reliableState=="true")
        //        {
        //            b.BackColor = Color.LightSkyBlue;
        //        }
        //        else
        //        {
        //            b.BackColor = Color.Gray;
        //        }

                
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        private void tappedButton(int i)
        {
            try
            {
                   this.db.updateSourceTable(listOfButtons[i].Name.ToString());         
                    reliableState = this.db.getReliableState();
                    if (reliableState == "true")
                    {
                        listOfButtons[i].BackColor = Color.LightSkyBlue;
                    }
                    else
                    {
                        listOfButtons[i].BackColor = Color.Gray;
                    }
           }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
