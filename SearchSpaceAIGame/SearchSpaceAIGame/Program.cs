using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Skip to line ~200 for code that isnt testing.


namespace SearchSpaceAIGame
{
    class Program
    {
        

        static void Main(string[] args)
        {
            #region
            int[,] somedata = new int[3, 3];

            void PrintsomeData()
            {
                Console.WriteLine("---");
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        System.Console.Write(somedata[x, y]);
                    }
                    System.Console.WriteLine('|');
                }
            }

            void DebugChecker(bool expected, bool result, string testname)
            {
                if(expected == result)
                {
                    Console.WriteLine(testname + " ran successfully");
                }
                else
                {
                    Console.WriteLine(testname + " FAILED");
                }
            }
            void ValidStateTest()
            {
                HanoiState mystate = new HanoiState();
                
                for(int y = 0; y < 3; y++)
                {
                    somedata[0, y] = 3 - y;
                }

                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(true, mystate.VerifyValid(), "Test 1, manual starting state");


                somedata[1, 0] = 1;
                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(false, mystate.VerifyValid(), "Test 2, manual setting of incorrect state");


                somedata[1, 0] = 0;
                somedata[1, 1] = 1;
                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(false, mystate.VerifyValid(), "Test 3, manual setting of incorrect state");
                somedata[1, 1] = 0;


                for (int y = 0; y < 3; y++)
                {
                    somedata[0, y] = y+1;
                }
                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(false, mystate.VerifyValid(), "Test 3, testing larger on top of smaller");

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        somedata[x, y] = 0;
                    }
                }

                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(false, mystate.VerifyValid(), "Test 4, is empty state a valid state?");

            }

            void GoalVerifyTest()
            {
                HanoiState mystate = new HanoiState();
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        somedata[x, y] = 0;
                    }
                }

                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(false, mystate.VerifyGoal(), "Test 1, is empty state a goal state?");


                for(int y = 0; y < 3; y++)
                {
                    somedata[2, y] = 3 - y;
                }
                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(true, mystate.VerifyGoal(), "Test 2, is goal state a goal state?");

                for (int y = 0; y < 3; y++)
                {
                    somedata[0, y] = 3 - y;
                }
                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(false, mystate.VerifyGoal(), "Test 3, is an invalid state a goal state?");

                for (int y = 0; y < 3; y++)
                {
                    somedata[2, y] = 0;
                }
                PrintsomeData();
                mystate.StateData = somedata;
                DebugChecker(false, mystate.VerifyGoal(), "Test 4, is start state a goal state?");
            }

            void GenChildrenTest()
            {
                HanoiState mystate = new HanoiState();
                mystate.SetToStartState();
                mystate.printdata();

                mystate.GenerateChildren();
                for(int i = 0; i < mystate.GetNumberChildren(); i++)
                {
                    mystate.sending[i].printdata();
                }

                Console.WriteLine("End test 1");

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        somedata[x, y] = 0;
                    }
                }


                HanoiState mystate2 = new HanoiState();
                for (int x = 0; x < 3; x++)
                {
                    somedata[x, 0] = 3 - x;
                }
                //PrintsomeData();
                mystate2.StateData = somedata;
                mystate2.GenerateChildren();
                mystate2.printdata();
                for (int i = 0; i < mystate2.GetNumberChildren(); i++)
                {
                    mystate2.sending[i].printdata();
                }

                Console.WriteLine("End test 2");
            }

            void GenStateTest()
            {
                HanoiState mystate = new HanoiState();
                mystate.SetToStartState();

                HanoiArc arc1 = new HanoiArc(0, 1);
                HanoiArc arc2 = new HanoiArc(0, 2);

                if (mystate.GenerateState(arc1))
                {
                    mystate.sending[0].printdata();
                    if (mystate.GenerateState(arc2))
                    {
                        mystate.sending[1].printdata();
                        Console.WriteLine("Generating 2 arcs from start worked manually");
                        if (mystate.sending[0].GenerateState(arc2))
                        {
                            mystate.sending[0].sending[0].printdata();
                        }
                        else
                        {
                            Console.WriteLine("Failed to gen children of a child.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Generating 2 arcs from start FAILED manually");
                }
                
            }
            #endregion


            //Create a graph and a state to be the start state
            HanoiState otherstate = new HanoiState();
            Graph<HanoiArc> mygraph = new Graph<HanoiArc>();

            //Pass the start state to the graph and initialize it.
            mygraph.AddToStartStates(otherstate);

            //[Optional]Initialize the state to be a start state as defined by the class.
            mygraph.InitStartStates();

            //Uses the respective algorithm to find goal states. The bool is if you want the algorithm to be Verbose via the classes defined print functions.
            mygraph.DepthFirstSearch(false);
            mygraph.BreadthFirstSearch(false);


            Console.ReadKey();

        }

        
    }
}
