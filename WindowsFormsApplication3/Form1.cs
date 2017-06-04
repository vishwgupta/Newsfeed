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
    public partial class Form1 : Form, ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;
        private dbConfig db;
        public Form1()
        {
            InitializeComponent();
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            this.controller.AddListener(listener);
            this.db = new dbConfig();
            db.insertData();
            db.displayData();
        
            //this.listener = new LeapEventListener(this);
            //controller.AddListener(listener);
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
                        //Handle circle gestures
                        this.button1.Text = "circle";
                        break;
                    case Gesture.GestureType.TYPE_KEY_TAP:
                        //Handle key tap gestures
                        this.button1.Text = "keytap";
                        break;
                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        //Handle screen tap gestures
                        this.button1.Text = "screen tap";
                       
                     //    var coordinate = GetNormalizedXAndY(fingers, screen);
                        isInside(frame.Gestures()[g]);
                        break;
                    case Gesture.GestureType.TYPE_SWIPE:
                        //Handle swipe gestures
                        this.button1.Text = "swipe";
                        break;
                    default:
                        //Handle unrecognized gestures
                        this.button1.Text = "unknown";
                        break;
                }
            }
         }

         private void isInside(Gesture gesture)
         {
             ScreenTapGesture screentapGesture = new ScreenTapGesture(gesture);
             Vector pokeLocation = screentapGesture.Position;

             

           if(  pokeLocation.x == this.button1.Location.X &&
             pokeLocation.y == this.button1.Location.Y)
           { 
             OnClick();
            }
           label2.Text = pokeLocation.x.ToString() + " " + pokeLocation.y.ToString();
           label3.Text = button1.Location.X.ToString() + " " +button1.Location.Y.ToString();
         }

         private void OnClick()
         {
             this.label1.Text = "you clicked the button";
         }

         protected override void Dispose(bool disposing)
         {
             try
             {
                 if (disposing)
                 {
                     if (components != null)
                     {
                         components.Dispose();
                     }
                     this.controller.RemoveListener(this.listener);
                     this.controller.Dispose();
                 }
             }
             finally
             {
                 base.Dispose(disposing);
             }
         }

       /*  public static Point GetNormalizedXAndY(FingerList fingers, Screen screen)
         {
             var xNormalized = screen.Intersect(fingers[0], true, 1.0F).x;
             var yNormalized = screen.Intersect(fingers[0], true, 1.0F).y;

             var x = (int)(xNormalized * screen.WidthPixels);
             var y = (int)screen.HeightPixels - (yNormalized * screen.HeightPixels);

             return new Point() { X = x, Y = y };
         }*/

         public static bool IsWithinRangeOfCloud(int x, int y)
         {
             return (Enumerable.Range(150, 300).Contains(x)
                     && Enumerable.Range(140, 400).Contains(y));
         }
    }

}
