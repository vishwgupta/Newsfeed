using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Leap;


namespace WindowsFormsApplication3
{
    public partial class settings : Form, ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;
        private dbConfig db;
        List<string> filteredList;
        List<int> filteredID = new List<int>();


        public settings()
        {
            InitializeComponent();
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
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
            this.controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
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
                    case Gesture.GestureType.TYPE_CIRCLE:


                        break;

                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        //Handle screen tap gestures
                        isInside(frame.Gestures()[g]);
                        break;

                    default:

                        break;
                }
            }


            float appHeight = (float)744.0;
            float appWidth = (float)1382;

            InteractionBox iBox = controller.Frame().InteractionBox;
            Vector normalizedPosition = iBox.NormalizePoint(frame.Hands[0].StabilizedPalmPosition, false);


            // Console.WriteLine(appHeight + " " + appWidth);

            float appX = (normalizedPosition.x * appWidth);
            float appY = (((1 - normalizedPosition.y) * appHeight));

            // Console.WriteLine(appX + " " + appY);

            button1.BackColor = Color.Wheat;
            button2.BackColor = Color.Wheat;
            button3.BackColor = Color.Wheat;
            button4.BackColor = Color.Wheat;
            button5.BackColor = Color.Wheat;
            button6.BackColor = Color.Wheat;
            button7.BackColor = Color.Wheat;
            button8.BackColor = Color.Wheat;

            if (appX >= 0 && appY >= 0 && appX <= appWidth / 2 && appY <= appHeight / 4)
            {
                //this.Hide();
                //MessageBox.Show("edit" + " " + appX + " " + appY);
                Console.WriteLine(appX + " 11111 " + appY);
                button1.BackColor = Color.Red;
                //editVideo edit = new editVideo();
                //edit.show();

            }

            else if (appX >= appWidth / 2 && appY >= 0 && appX <= appWidth && appY <= appHeight / 4)
            {
                //this.Hide();
                //MessageBox.Show("open newsfeed" + " " + appX + " " + appY + " " + 0 + " " + appHeight / 4);
                //openNewsFeed feed = new openNewsFeed();
                button2.BackColor = Color.Red;
                //feed.show();
            }


            else if (appX >= 0 && appY >= appHeight / 4 && appX <= appWidth / 2 && appY <= appHeight / 2)
            {
                //this.Hide();
                // MessageBox.Show("open snippet");
                button3.BackColor = Color.Red;
                //openSnippet snippet = new openSnippet();
                //snippet.show();
            }

            else if (appX >= appWidth / 2 && appY >= appHeight / 4 && appX <= appWidth && appY <= appHeight / 2)
            {
                //this.Hide();

                button4.BackColor = Color.Red;
                //MessageBox.Show("combine");
                //combineSnippet comb = new combineSnippet();
                //comb.show();
            }
            else if (appX >= 0 && appY >= appHeight / 2 && appX <= appWidth / 2 && appY <= (3 * appHeight) / 4)
            {
                //this.Hide();
                //MessageBox.Show("mark");
                button5.BackColor = Color.Red;
                //markSourcesReliable markSource = new markSourcesReliable();
                //markSource.show();
            }

            else if (appX >= appWidth / 2 && appY >= appHeight / 2 && appX <= appWidth && appY <= (3 * appHeight) / 4)
            {
                //this.Hide();
                button6.BackColor = Color.Red;
                //MessageBox.Show("translate");
                //translateVideo trans = new translateVideo();
                //trans.show();
            }

            else if (appX >= 0 && appY >= (appHeight * 3.0) / 4 && appX <= appWidth / 2 && appY <= appHeight)
            {
                //this.Hide();
                //enableFilter enable = new enableFilter();
                //enable.show();
                /*MessageBox.Show("enable filter");
                this.db = new dbConfig();
                this.db.enableFilter();
                filteredList = this.db.getfilteredList();
                MessageBox.Show("1st name " + filteredList[0].ToString());
                MessageBox.Show("2nd name " + filteredList[1].ToString());*/
                button7.BackColor = Color.Red;



            }

            else if (appX >= appWidth / 2.0 && appY >= (3.0 * appHeight) / 4.0 && appX <= appWidth && appY <= appHeight)
            {
                //this.Hide();
                //disableFilter disable = new disableFilter();
                //MessageBox.Show("disable filter" + appX + " " + appY + " " + appWidth / 2 + " " + (3 * appHeight) / 4 + " " + appWidth + " " + appHeight);
                button8.BackColor = Color.Red;

                Console.WriteLine(appX + " 88888 " + appY);
                //disable.show();
            }
            else
            {
                //MessageBox.Show("result" + appX + " " + appY);

            }

        }
        private void isInside(Gesture gesture)
        {
            ScreenTapGesture screentapGesture = new ScreenTapGesture(gesture);

            int appHeight = 744; //settings.ActiveForm.Height;
            int appWidth = 1382;// settings.ActiveForm.Width;

            InteractionBox iBox = controller.Frame().InteractionBox;
            Pointable pointable = controller.Frame().Pointables.Frontmost;
            Vector pokeLocation = screentapGesture.Position;
            Frame frame = controller.Frame();
            Finger finger = frame.Fingers.Frontmost;
            //Leap.Vector leapPoint = pointable.StabilizedTipPosition;
            //Leap.Vector normalizedPoint = iBox.NormalizePoint(leapPoint, false);



            Vector normalizedPosition = iBox.NormalizePoint(frame.Hands[1].StabilizedPalmPosition, false);


            Console.WriteLine(appHeight + " " + appWidth);

            float appX = (normalizedPosition.x * appWidth);
            float appY = (((1 - normalizedPosition.y) * appHeight));

            Console.WriteLine(appX + " " + appY);



            if (appX >= 0 && appY >= 0 && appX <= appWidth / 2 && appY <= appHeight / 4)
            {
                //this.Hide();
                MessageBox.Show("edit" + " " + appX + " " + appY);

                //editVideo edit = new editVideo();
                //edit.show();

            }

            else if (appX >= appWidth / 2 && appY >= 0 && appX <= appWidth && appY <= appHeight / 4)
            {
                //this.Hide();
                MessageBox.Show("open newsfeed" + " " + appX + " " + appY + " " + 0 + " " + appHeight / 4);
                //openNewsFeed feed = new openNewsFeed();
                //feed.show();
            }


            else if (appX >= 0 && appY >= appHeight / 4 && appX <= appWidth / 2 && appY <= appHeight / 2)
            {
                //this.Hide();
                MessageBox.Show("open snippet");
                //openSnippet snippet = new openSnippet();
                //snippet.show();
            }

            else if (appX >= appWidth / 2 && appY >= appHeight / 4 && appX <= appWidth && appY <= appHeight / 2)
            {
                //this.Hide();
                MessageBox.Show("combine");
                //combineSnippet comb = new combineSnippet();
                //comb.show();
            }
            else if (appX >= 0 && appY >= appHeight / 2 && appX <= appWidth / 2 && appY <= (3 * appHeight) / 4)
            {
                //this.Hide();
                MessageBox.Show("mark");
                //markSourcesReliable markSource = new markSourcesReliable();
                //markSource.show();
            }

            else if (appX >= appWidth / 2 && appY >= appHeight / 2 && appX <= appWidth && appY <= (3 * appHeight) / 4)
            {
                //this.Hide();
                MessageBox.Show("translate");
                //translateVideo trans = new translateVideo();
                //trans.show();
            }

            else if (appX >= 0 && appY >= (appHeight * 3) / 4 && appX <= appWidth / 2 && appY <= appHeight)
            {
                //this.Hide();
                //enableFilter enable = new enableFilter();
                //enable.show();
                MessageBox.Show("enable filter");
                this.db = new dbConfig();
                this.db.enableFilter();
                filteredList = this.db.getfilteredList();
                MessageBox.Show("1st name " + filteredList[0].ToString());
                MessageBox.Show("2nd name " + filteredList[1].ToString());



            }

            else if (appX >= appWidth / 2 && appY >= (3 * appHeight) / 4 && appX <= appWidth && appY <= appHeight)
            {
                //this.Hide();
                //disableFilter disable = new disableFilter();
                MessageBox.Show("disable filter" + appX + " " + appY + " " + appWidth / 2 + " " + (3 * appHeight) / 4 + " " + appWidth + " " + appHeight);
                //disable.show();
            }
            else
            {
                MessageBox.Show("result" + appX + " " + appY);

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.db = new dbConfig();
            this.db.enableFilter();
            filteredList = this.db.getfilteredList();
            filteredID = this.db.getfilteredId();
            MessageBox.Show("1st name " + filteredList[0].ToString());
            MessageBox.Show("2nd name " + filteredList[1].ToString());
            MessageBox.Show("1st Id " + filteredID[0].ToString());
            MessageBox.Show("2nd Id " + filteredID[1].ToString());



        }


    }
}