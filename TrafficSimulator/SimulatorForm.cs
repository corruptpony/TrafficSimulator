﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TrafficSimulatorUi;

namespace TrafficSimulator
{
    public partial class SimulatorForm : Form
    {
        /// <summary>
        /// List to keep track of all road users.
        /// You can put roadusers on intersections to make them appear there.
        /// </summary>
        private List<LogicControl> logicControls;
        private List<IntersectionControl> intersections;


        public SimulatorForm()
        {
            InitializeComponent();
            intersections = new List<IntersectionControl>();

            intersections.Add(intersectionControl1);
            intersections.Add(intersectionControl2);
            intersections.Add(intersectionControl3);
            intersections.Add(intersectionControl4);

            logicControls = new List<LogicControl>();

            //LogicControls.Add(new LogicControlType1());
            logicControls.Add(new LogicControlType2(intersections));
            //LogicControls.Add(new LogicControlType3());
            //LogicControls.Add(new LogicControlType4());
            //LogicControls.Add(new LogicControlType5());
            //LogicControls.Add(new LogicControlRail());

            RoadUser TestCar1 = new BlueCar(new Point(-32, 216), 2);
            RoadUser TestCar2 = new GreenSportsCar(new Point(-32, 244), 2);
            intersectionControl1.AddRoadUser(TestCar1);
            intersectionControl1.AddRoadUser(TestCar2);

            progressTimer.Start();
        }

        private void progressTimer_Tick(object sender, EventArgs e)
        {
            UpdateWorld();
        }

        private void UpdateWorld()
        {
            foreach (LogicControl LC in logicControls)
            {
                foreach (RoadUser roadUser in LC.Intersection.RoadUsers)
                {
                    roadUser.Move();
                }

                LC.MakeTurn();
                LC.RemoveEndOFLaneRoadUser();
                LC.HandleHeadTailCollision();
                LC.HandleTrafficLight();
                LC.HandleQueue();
                LC.Intersection.Invalidate();
            }
        }

        private void intersectionControl_TrafficLightClick(object sender, TrafficLightClickEventArgs e)
        {
            // Example code for interacting with traffic lights
            // Note: The goal of this example is to shows some code mechanics. Nothing more.
            //       Probably you want to remove most of it, because it does not do wat you want over here.
            //
            // The example code shows 
            // - How to handle TrafficLightClick events.
            // - How to get the state of a traffic light.
            // - How to set thet state of a traffic light.

            Debug.WriteLine("Clicked traffic light with lane id: " + e.LaneId + ", of intersection: ");
            IntersectionControl intersection = (IntersectionControl)sender;
            TrafficLight trafficLight = intersection.GetTrafficLight(e.LaneId);
            trafficLight.SwitchTo(SignalState.STOP);
        }
    }
}
